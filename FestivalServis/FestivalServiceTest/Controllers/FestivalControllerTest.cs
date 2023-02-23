using AutoMapper;
using FestivalServis.Controllers;
using FestivalServis.Interfaces;
using FestivalServis.Models;
using FestivalServis.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace FestivalServiceTest.Controllers
{
    public class FestivalControllerTest
    {
        [Fact]
        public void GetFestivals_ValidFestivals_ReturnsCollection()
        {
            List<Festival> festivals = new List<Festival>()
            {
                new Festival() {Id = 1, Name = "Exit", TicketPrice = 350, YearOfFirstEvent = 2000, PlaceId = 1, Place = new Place(){Id = 1, Name = "Novi Sad", ZipCode = 21000} },

                new Festival() {Id = 2, Name = "Sea Dance", TicketPrice = 200,YearOfFirstEvent = 1995, PlaceId = 2, Place = new Place() {Id = 2, Name = "Budva", ZipCode = 222 } }
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(festivals.AsQueryable()) ;

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.GetFestivals() as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            List<FestivalDTO> listResult = (List<FestivalDTO>)actionResult.Value;

            for (int i = 0; i < listResult.Count; i++)
            {
                Assert.Equal(festivals[i].Id, listResult[i].Id);
                Assert.Equal(festivals[i].Name, listResult[i].Name);
                Assert.Equal(festivals[i].YearOfFirstEvent, listResult[i].YearOfFirstEvent);
                Assert.Equal(festivals[i].TicketPrice, listResult[i].TicketPrice);
                Assert.Equal(festivals[i].Place.Name, listResult[i].PlaceName);
                Assert.Equal(festivals[i].PlaceId, listResult[i].PlaceId);
            }

        }

        [Fact]
        public void GetFestival_ValidId_ReturnsObject()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Name = "Exit",
                TicketPrice = 350,
                YearOfFirstEvent = 2000,
                PlaceId = 1,
                Place = new Place() { Id = 1, Name = "Novi Sad", ZipCode = 21000 }
            };

            FestivalDTO festivalDTO = new FestivalDTO()
            {
                Id = 1,
                Name = "Exit",
                TicketPrice = 350,
                YearOfFirstEvent = 2000,
                PlaceId = 1,
                PlaceName = "Novi Sad"
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.GetFestival(1) as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            FestivalDTO dtoResult = (FestivalDTO)actionResult.Value;

            Assert.Equal(festival.Id, dtoResult.Id);
            Assert.Equal(festival.Name, dtoResult.Name);
            Assert.Equal(festival.TicketPrice, dtoResult.TicketPrice);
            Assert.Equal(festival.PlaceId, dtoResult.PlaceId);
            Assert.Equal(festival.Place.Name, dtoResult.PlaceName);

        }

        [Fact]
        public void GetFestival_InvalidId_ReturnsNotFound()
        {
            var mockRepository = new Mock<IFestivalRepository>();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.GetFestival(50) as NotFoundResult;

            Assert.NotNull(actionResult);
        }

        [Fact]
        public void PutFestival_ValidRequest_ReturnsObject()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Name = "Exit",
                TicketPrice = 350,
                YearOfFirstEvent = 2000,
                PlaceId = 1,
                Place = new Place() { Id = 1, Name = "Novi Sad", ZipCode = 21000 }
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.PutFestival(1, festival) as OkObjectResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);

            FestivalDTO dtoResult = (FestivalDTO)actionResult.Value;

            Assert.Equal(festival.Id, dtoResult.Id);
            Assert.Equal(festival.Name, dtoResult.Name);
            Assert.Equal(festival.TicketPrice, dtoResult.TicketPrice);
            Assert.Equal(festival.PlaceId, dtoResult.PlaceId);
            Assert.Equal(festival.Place.Name, dtoResult.PlaceName);

        }

        [Fact]
        public void PutFestival_InvalidId_ReturnsBadRequest()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Name = "Exit",
                TicketPrice = 350,
                YearOfFirstEvent = 2000,
                PlaceId = 1,
                Place = new Place() { Id = 1, Name = "Novi Sad", ZipCode = 21000 }
            };

            var mockRepository = new Mock<IFestivalRepository>();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.PutFestival(50, festival) as BadRequestResult;

            Assert.NotNull(actionResult);
        }


        [Fact]
        public void PostFestival_ValidRequest_SetsLocationHeader()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Name = "Exit",
                TicketPrice = 350,
                YearOfFirstEvent = 2000,
                PlaceId = 1,
                Place = new Place() { Id = 1, Name = "Novi Sad", ZipCode = 21000 }
            };

            var mockRepository = new Mock<IFestivalRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.PostFestival(festival) as CreatedAtActionResult;

            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult.Value);
            Assert.Equal("GetFestival", actionResult.ActionName);
            Assert.Equal(1, actionResult.RouteValues["id"]);
            Assert.Equal(festival, actionResult.Value);

        }

        [Fact]
        public void DeleteFestival_ValidId_ReturnsNoContent()
        {
            Festival festival = new Festival()
            {
                Id = 1,
                Name = "Exit",
                TicketPrice = 350,
                YearOfFirstEvent = 2000,
                PlaceId = 1,
                Place = new Place() { Id = 1, Name = "Novi Sad", ZipCode = 21000 }
            };

            var mockRepository = new Mock<IFestivalRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(festival);

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.DeleteFestival(1) as NoContentResult;
            Assert.NotNull(actionResult);
        }

        [Fact]
        public void DeleteFestival_InvalidId_ReturnsNotFound()
        {
            var mockRepository = new Mock<IFestivalRepository>();

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(new FestivalProfile()));
            IMapper mapper = new Mapper(mapperConfiguration);

            var controller = new FestivaliController(mockRepository.Object, mapper);

            var actionResult = controller.DeleteFestival(50) as NotFoundResult;
            Assert.NotNull(actionResult);
        }
    }
}
