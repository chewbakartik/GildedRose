using System;
using System.Collections.Generic;
using System.Security.Claims;
using GildedRose.Controllers;
using GildedRose.Data.Abstract;
using GildedRose.Entities;
using GildedRose.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GildedRose.Tests.Unit.Controllers
{
    public class TransactionControllerTest
    {
        [Fact]
        public void Get_ReturnsNotFoundResultWhenTransactionDoesNotExist()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionService = new Mock<ITransactionService>();
            var userRepository = new Mock<IUserRepository>();

            var userId = 1;
            var testAuthId = "auth|testId";
            var testEmail = "test@test.test";
            userRepository.Setup(p => p.GetByAuthId(testAuthId)).Returns(new User
            {
                AuthId = testAuthId,
                Email = testEmail,
                Id = userId
            });

            var controller = new TransactionController(transactionRepository.Object, transactionService.Object, userRepository.Object);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, testAuthId),
                new Claim(ClaimTypes.Email, testEmail)
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var result = controller.Get(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Get_ReturnsForbiddenResultWhenTransactionOwnedByDifferentNonAdminUser()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionService = new Mock<ITransactionService>();
            var userRepository = new Mock<IUserRepository>();

            transactionRepository.Setup(p => p.GetProperties(1)).Returns(new Transaction
            {
                Details = new List<TransactionDetail>(),
                OrderDate = DateTime.Now,
                Total = 10.00,
                UserId = 2
            });

            var userId = 1;
            var testAuthId = "auth|testId";
            var testEmail = "test@test.test";
            userRepository.Setup(p => p.GetByAuthId(testAuthId)).Returns(new User
            {
                AuthId = testAuthId,
                Email = testEmail,
                Id = userId
            });

            var controller = new TransactionController(transactionRepository.Object, transactionService.Object, userRepository.Object);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, testAuthId),
                new Claim(ClaimTypes.Email, testEmail)
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            var result = controller.Get(1);
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void Order_ReturnsBadRequestWhenTransactionDetailsAreNull()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionService = new Mock<ITransactionService>();
            var userRepository = new Mock<IUserRepository>();
            var controller = new TransactionController(transactionRepository.Object, transactionService.Object, userRepository.Object);
            List<PurchaseOrderItemDTO> items = null;
            var result = controller.Order(items);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Order_ReturnsBadRequestWhenTransactionDetailsAreInvalid()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionService = new Mock<ITransactionService>();
            List<PurchaseOrderItemDTO> items = null;
            var validationMessages = new List<string>();
            transactionService.Setup(p => p.Validate(items, out validationMessages)).Returns(false);
            var userRepository = new Mock<IUserRepository>();
            var controller = new TransactionController(transactionRepository.Object, transactionService.Object, userRepository.Object);
            var result = controller.Order(items);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void Order_ReturnsCreatedAtRouteRequestWhenTransactionDetailsAreValid()
        {
            var transactionRepository = new Mock<ITransactionRepository>();
            var transactionService = new Mock<ITransactionService>();
            var items = new List<PurchaseOrderItemDTO>{new PurchaseOrderItemDTO
            {
                ItemId = 1,
                Quantity = 1
            }};
            var userId = 1;
            var validationMessages = new List<string>();
            transactionService.Setup(p => p.Validate(items, out validationMessages)).Returns(true);
            transactionService.Setup(p => p.Create(items, userId)).Returns(new Transaction
            {
                OrderDate = DateTime.Now,
                Total = 19.99,
                UserId = userId
            });

            var testAuthId = "auth|testId";
            var testEmail = "test@test.test";

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(p => p.GetByAuthId(testAuthId)).Returns(new User
            {
                AuthId = testAuthId,
                Email = testEmail,
                Id = userId
            });
            var controller = new TransactionController(transactionRepository.Object, transactionService.Object, userRepository.Object);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, testAuthId), 
                new Claim(ClaimTypes.Email, testEmail) 
            }));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext {User = claimsPrincipal}
            };
            var result = controller.Order(items);
            Assert.IsType<CreatedAtRouteResult>(result);
        }
    }
}
