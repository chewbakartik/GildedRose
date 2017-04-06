using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;

namespace GildedRose.Data.Abstract
{
    public interface ITransactionService
    {
        Transaction Create(List<PurchaseOrderItemDTO> items, int userId);
        bool Validate(List<PurchaseOrderItemDTO> items, out List<string> messages);
    }
}
