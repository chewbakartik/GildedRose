using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRose.Data.Abstract;
using GildedRose.Entities.DTOs;
using GildedRose.Entities;

namespace GildedRose.Data.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IItemRepository _itemRepository;

        public TransactionService(IItemRepository itemRepository, ITransactionDetailRepository transactionDetailRepository, ITransactionRepository transactionRepository)
        {
            _itemRepository = itemRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionRepository = transactionRepository;
        }

        public Transaction Create(List<PurchaseOrderItemDTO> items, int userId)
        {
            double total = 0;
            var details = new List<TransactionDetail>();
            foreach (var itemDTO in items)
            {
                var item = _itemRepository.Get(itemDTO.ItemId);
                var detail = new TransactionDetail
                {
                    ItemId = itemDTO.ItemId,
                    Quantity = itemDTO.Quantity,
                    Price = item.Price,
                    SubTotal = item.Price * itemDTO.Quantity
                };
                total += detail.SubTotal;
                _transactionDetailRepository.Add(detail);
                _transactionDetailRepository.Commit();
                details.Add(detail);
            }
            var transaction = new Transaction
            {
                Details = details,
                OrderDate = DateTime.Now,
                Total = total,
                UserId = userId
            };
            _transactionRepository.Add(transaction);
            _transactionRepository.Commit();
            return transaction;
        }

        public bool Validate(List<PurchaseOrderItemDTO> items, out List<string> validationMessages)
        {
            validationMessages = new List<string>();
            bool isValid = true;
            foreach (var itemDTO in items)
            {
                var item = _itemRepository.Get(itemDTO.ItemId);
                if (item == null)
                {
                    isValid = false;
                    validationMessages.Add(String.Format("Item {0}: Does not exist", itemDTO.ItemId));
                }
                else if (item.Quantity < itemDTO.Quantity)
                {
                    isValid = false;
                    validationMessages.Add(String.Format("Item {0}: Ordering {1} quantity, but only {2} are available", itemDTO.ItemId, itemDTO.Quantity, item.Quantity));
                }
            }
            return isValid;
        }
    }
}
