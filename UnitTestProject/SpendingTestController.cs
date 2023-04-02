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
        private readonly Mock<ISpendingService> _spendingService;
        private readonly SpendingController _sut;

        public SpendingTestController()
        {
            _spendingService = new Mock<ISpendingService>();
            _sut = new SpendingController(_spendingService.Object);
        }

        [Fact]
        public async Task GetSpendings_Should_Return_NotNull_And_Ok()
        {

            //arrange
            var spendingList = await GetSpendingsData();
            //string expectResult = JsonConvert.SerializeObject((spendingList.Data));

            _spendingService.Setup(x => x.GetSpendings(1, ""))
                .ReturnsAsync(spendingList);

            //act
            var spendingResult = _sut.Get(1);
            //string resultString = JsonConvert.SerializeObject(spendingResult.Result);
            //string finalResultString = resultString.Substring(resultString.IndexOf(":") + 1, resultString.IndexOf("]"));


            //assert
            Assert.NotNull(spendingResult);
            Assert.IsType<OkObjectResult>(spendingResult.Result.Result);
            //Assert.Equal(GetSpendingsData().Count(), spendingResult.Count());
            //Assert.Equal(spendingList.Data, resultString);
            //Assert.True(spendingList.Equals(spendingResult));
        }

        [Fact]
        public async Task CreateSpending_Should_Return_NotNull_And_Ok()
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

            _spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);

            //act
            var spendingResult = await _sut.Create(newSpendingDto);

            //assert
            Assert.NotNull(spendingResult);
            Assert.IsType<OkObjectResult>(spendingResult.Result);
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

            //Spending newSpending = new Spending
            //{
            //    Date = newSpendingDto.Date,
            //    Amount = newSpendingDto.Amount,
            //    Comment = newSpendingDto.Comment,
            //    UserId = newSpendingDto.UserId,
            //    SpendingTypeId = newSpendingDto.SpendingTypeId
            //};

            ServiceResponse<Spending> newSpendingSR = new ServiceResponse<Spending> 
            { 
                Data = null,
                Success = false,
                Message = "The date cannot be superior to the current date."
            };

            _spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);

            //act
            var spendingResult = await _sut.Create(newSpendingDto);

            //assert
            Assert.IsType<BadRequestObjectResult>(spendingResult.Result);

            var spendingResultCasted = (BadRequestObjectResult)spendingResult.Result; 
            var spendingResultMessage = spendingResultCasted.Value; 
            Assert.Equal("The date cannot be superior to the current date.", spendingResultMessage);
        }

        [Fact]
        public async Task CreateSpending_Date_Cannot_Be_Inferior_To_ThreeMonthsAgo()
        {
            //arrange
            SpendingDto newSpendingDto = new SpendingDto
            {
                UserId = 1,
                UserFullName = "Anthony Stark",
                Date = DateTime.Parse("2022-12-09T00:00:00"),
                SpendingTypeId = 1,
                SpendingTypeName = "Restaurant",
                Amount = 65,
                CurrencyName = "USD",
                Comment = "New restaurant in Paris"
            }; 

            ServiceResponse<Spending> newSpendingSR = new ServiceResponse<Spending>
            {
                Data = null,
                Success = false,
                Message = "The date cannot be set to before 3 months ago. It must be between three months ago and today."
            };

            _spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);

            //act
            var spendingResult = await _sut.Create(newSpendingDto);

            //assert
            Assert.IsType<BadRequestObjectResult>(spendingResult.Result);

            var spendingResultCasted = (BadRequestObjectResult)spendingResult.Result;
            var spendingResultMessage = spendingResultCasted.Value;
            Assert.Equal("The date cannot be set to before 3 months ago. It must be between three months ago and today.", spendingResultMessage);
        }

        [Fact]
        public async Task CreateSpending_Comment_Is_Mandatory()
        {
            //arrange
            SpendingDto newSpendingDto = new SpendingDto
            {
                UserId = 1,
                UserFullName = "Anthony Stark",
                Date = DateTime.Parse("2022-03-09T00:00:00"),
                SpendingTypeId = 1,
                SpendingTypeName = "Restaurant",
                Amount = 65,
                CurrencyName = "USD",
                Comment = " "
            };

            ServiceResponse<Spending> newSpendingSR = new ServiceResponse<Spending>
            {
                Data = null,
                Success = false,
                Message = "The comment is mandatory."
            };

            _spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);

            //act
            var spendingResult = await _sut.Create(newSpendingDto);

            //assert
            Assert.IsType<BadRequestObjectResult>(spendingResult.Result);

            var spendingResultCasted = (BadRequestObjectResult)spendingResult.Result;
            var spendingResultMessage = spendingResultCasted.Value;
            Assert.Equal("The comment is mandatory.", spendingResultMessage);
        }

        [Fact]
        public async Task CreateSpending_Cannot_Have_SameDate_SameAmount_ForOneUser()
        {
            //arrange
            SpendingDto newSpendingDto = new SpendingDto
            {
                UserId = 1,
                UserFullName = "Anthony Stark",
                Date = DateTime.Parse("2023-02-16T00:00:00"),
                SpendingTypeId = 1,
                SpendingTypeName = "Restaurant",
                Amount = 50,
                CurrencyName = "USD",
                Comment = "Nice lunch in Nice on the market place"
            };

            ServiceResponse<Spending> newSpendingSR = new ServiceResponse<Spending>
            {
                Data = null,
                Success = false,
                Message = "There is already a spending with the same date and the same amount for this user."
            };

            _spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);

            //act
            var spendingResult = await _sut.Create(newSpendingDto);

            //assert
            Assert.IsType<BadRequestObjectResult>(spendingResult.Result);

            var spendingResultCasted = (BadRequestObjectResult)spendingResult.Result;
            var spendingResultMessage = spendingResultCasted.Value;
            Assert.Equal("There is already a spending with the same date and the same amount for this user.", spendingResultMessage);
        }

        [Fact]
        public async Task CreateSpending_Currency_Must_Be_Identical_ToTheUser()
        {
            //arrange
            SpendingDto newSpendingDto = new SpendingDto
            {
                UserId = 1,
                UserFullName = "Anthony Stark",
                Date = DateTime.Parse("2023-02-16T00:00:00"),
                SpendingTypeId = 1,
                SpendingTypeName = "Restaurant",
                Amount = 50,
                CurrencyName = "EUR",
                Comment = "Nice lunch in Nice on the market place"
            };

            ServiceResponse<Spending> newSpendingSR = new ServiceResponse<Spending>
            {
                Data = null,
                Success = false,
                Message = "The currency must be the same as the user's currency."
            };

            _spendingService.Setup(x => x.CreateSpending(newSpendingDto)).ReturnsAsync(newSpendingSR);

            //act
            var spendingResult = await _sut.Create(newSpendingDto);

            //assert
            Assert.IsType<BadRequestObjectResult>(spendingResult.Result);

            var spendingResultCasted = (BadRequestObjectResult)spendingResult.Result;
            var spendingResultMessage = spendingResultCasted.Value;
            Assert.Equal("The currency must be the same as the user's currency.", spendingResultMessage);
        }




        // Create a list of Spendings by user Anthony Stark
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