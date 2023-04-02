using Moq;
using ASPNETWebAPI.Controllers;
using ASPNETWebAPI.Models;
using ASPNETWebAPI.Services.SpendingService;
using ASPNETWebAPI.Dto;
using System.Collections.Generic;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Azure.Core;
using Azure;

namespace UnitTestProject
{
    public class SpendingTestController
    {
        private readonly Mock<ISpendingService> spendingService;
        public SpendingTestController()
        {
            spendingService = new Mock<ISpendingService>();
        }

        [Fact]
        public async Task GetSpendings_Should_Return_NotNull()
        {

            //arrange
            var spendingList = await GetSpendingsData();
            //string expectResult = JsonConvert.SerializeObject((spendingList.Data));

            spendingService.Setup(x => x.GetSpendings(1, ""))
                .ReturnsAsync(spendingList);
            var spendingController = new SpendingController(spendingService.Object);

            //act
            var spendingResult = await spendingController.Get(1);
            //string resultString = JsonConvert.SerializeObject(spendingResult.Result);
            //string finalResultString = resultString.Substring(resultString.IndexOf(":") + 1, resultString.IndexOf("]"));


            //assert
            Assert.NotNull(spendingResult);
            //Assert.Equal(GetSpendingsData().Count(), spendingResult.Count());
            //Assert.Equal(spendingList.Data, resultString);
            //Assert.True(spendingList.Equals(spendingResult));
        }

        [Fact]
        public async Task CreateSpending_Should_Return_NotNull()
        {
            //arrange

            SpendingDto newSpendingDto = new SpendingDto
            {
                UserId = 1,
                UserFullName = "Anthony Stark",
                Date = DateTime.Parse("2023-02-09T00:00:00"),
                SpendingTypeId = 1,
                SpendingTypeName = "Restaurant",
                Amount = 65,
                CurrencyName = "USD",
                Comment = "New restaurant in Paris"
            };

            var newSpending = new Spending
            {
                Date = newSpendingDto.Date,
                Amount = newSpendingDto.Amount,
                Comment = newSpendingDto.Comment,
                UserId = newSpendingDto.UserId,
                SpendingTypeId = newSpendingDto.SpendingTypeId
            };

            ServiceResponse<Spending>  newSpendingSR = new ServiceResponse<Spending> { Data = newSpending };

            spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);
            var spendingController = new SpendingController(spendingService.Object);

            //act
            var spendingResult = await spendingController.Create(newSpendingDto);

            //assert
            Assert.NotNull(spendingResult);
        }

        [Fact]
        public async Task CreateSpending_Date_Cannot_Be_Superior_To_Today()
        {
            //arrange

            SpendingDto newSpendingDto = new SpendingDto
            {
                UserId = 1,
                UserFullName = "Anthony Stark",
                Date = DateTime.Parse("2023-05-09T00:00:00"),
                SpendingTypeId = 1,
                SpendingTypeName = "Restaurant",
                Amount = 65,
                CurrencyName = "USD",
                Comment = "New restaurant in Paris"
            };

            Spending newSpending = new Spending
            {
                Date = newSpendingDto.Date,
                Amount = newSpendingDto.Amount,
                Comment = newSpendingDto.Comment,
                UserId = newSpendingDto.UserId,
                SpendingTypeId = newSpendingDto.SpendingTypeId
            };

            ServiceResponse<Spending> newSpendingSR = new ServiceResponse<Spending> { Data = newSpending };

            spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);
            var spendingController = new SpendingController(spendingService.Object);

            //act
            var spendingResult = await spendingController.Create(newSpendingDto);

            //assert
            //Assert.IsType(spendingResult.Result, typeof(BadRequestResult));
            //Assert.Equal("The date cannot be superior to the current date.", spendingResult.Message);
        }

        private async Task<ServiceResponse<List<SpendingDto>>> GetSpendingsData()
        {
            List<SpendingDto> spendingsData = new List<SpendingDto>
            {
                new SpendingDto
                {
                    Id= 8,
                    UserId= 1,
                    UserFullName= "Anthony Stark",
                    Date= DateTime.Parse("2023-02-17T00:00:00"),
                    SpendingTypeId= 1,
                    SpendingTypeName= "Restaurant",
                    Amount= 30,
                    CurrencyName= "USD",
                    Comment= "Nice lunch in Cannes on the beach"
                },
                 new SpendingDto
                {
                    Id= 10,
                    UserId= 1,
                    UserFullName= "Anthony Stark",
                    Date= DateTime.Parse("2023-02-16T00:00:00"),
                    SpendingTypeId= 1,
                    SpendingTypeName= "Restaurant",
                    Amount= 50,
                    CurrencyName= "USD",
                    Comment= "Nice lunch in Cannes on the beach"
                },
                 new SpendingDto
                {
                    Id= 3,
                    UserId= 1,
                    UserFullName= "Anthony Stark",
                    Date= DateTime.Parse("2023-03-27T22:55:12.1248562"),
                    SpendingTypeId= 2,
                    SpendingTypeName= "Hotel",
                    Amount= 110.9f,
                    CurrencyName= "USD",
                    Comment= "I went to the best hotel in town"
                },
                 new SpendingDto
                {
                    Id= 9,
                    UserId= 1,
                    UserFullName= "Anthony Stark",
                    Date= DateTime.Parse("2023-02-20T00:00:00"),
                    SpendingTypeId= 2,
                    SpendingTypeName= "Hotel",
                    Amount= 190,
                    CurrencyName= "USD",
                    Comment= "Luxury hotel in San Francisco"
                },
                 new SpendingDto
                {
                    Id= 1,
                    UserId= 1,
                    UserFullName= "Anthony Stark",
                    Date= DateTime.Parse("2023-04-01T22:55:12.1248517"),
                    SpendingTypeId= 3,
                    SpendingTypeName= "Misc",
                    Amount= 250,
                    CurrencyName= "USD",
                    Comment= "I bought a new screen"
                },
            };
            return new ServiceResponse<List<SpendingDto>>
            {
                Data = spendingsData
            };
        }
    }
}