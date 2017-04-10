using System.Collections.Generic;
using System.Linq;
using GildedRose.Data.Abstract;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GildedRose.Controllers
{
    [Route("api/transactions")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public TransactionController(ITransactionRepository transactionRepository, ITransactionService transactionService, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionService = transactionService;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "user")]
        [HttpGet("{id}", Name = "GetTransaction")]
        public IActionResult Get(int id)
        {
            // Get calling user
            var authId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = _userRepository.GetByAuthId(authId);
            
            // Get transaction
            var transaction = _transactionRepository.GetProperties(id);
            if (transaction == null)
            {
                return new NotFoundResult();
            }
            if (User.Claims.Where(c => c.Type == ClaimTypes.Role).All(c => c.Value == "admin") || (user != null && transaction.UserId != user.Id))
            {
                return new ForbidResult();
            }
            return new OkObjectResult(transaction);
        }

        [Authorize(Roles = "user")]
        [HttpPost("order")]
        public IActionResult Order([FromBody] List<PurchaseOrderItemDTO> items)
        {
            if (items == null)
            {
                return new BadRequestResult();
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
            return CreatedAtRoute("GetTransaction", new {transaction.Id}, transaction);
        }
    }
}
