using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.Data.Abstract;
using GildedRose.Data.Services;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GildedRose.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserRepository _userRepository;

        public TransactionController(ITransactionService transactionService, IUserRepository userRepository)
        {
            _transactionService = transactionService;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "user")]
        [HttpPost]
        public IActionResult Order([FromBody] List<PurchaseOrderItemDTO> items)
        {
            if (items == null)
            {
                return BadRequest();
            }
            List<string> validationMessages;
            var valid = _transactionService.Validate(items, out validationMessages);
            if (!valid)
            {
                return BadRequest(validationMessages);
            }
            var authId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Check to see if we've stored the user locally
            var user = _userRepository.GetByAuthId(authId);
            if (user == null)
            {
                //Create new user
                user = new User
                {
                    AuthId = authId,
                    Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                };
                _userRepository.Add(user);
            }
            var transaction = _transactionService.Create(items, user.Id);
            return CreatedAtRoute("TransactionOrder", transaction.Id, transaction);
        }
    }
}
