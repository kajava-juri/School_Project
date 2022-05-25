using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.FileAccess;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ScheduleServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IScheduleRepository> _scheduleRepositoryMock;
        private readonly Mock<IFileClient> _fileClientMock;
        private readonly Mock<IMapper> _objectMapperMock;

        private readonly ScheduleService _scheduleService;

        public ScheduleServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _fileClientMock = new Mock<IFileClient>();
            _objectMapperMock = new Mock<IMapper>();
            _scheduleRepositoryMock = new Mock<IScheduleRepository>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(Program).Assembly);
            });
            var mapper = mapperConfig.CreateMapper();

            _uowMock.SetupGet(uow => uow.Schedule)
                           .Returns(_scheduleRepositoryMock.Object);

            _scheduleService = new ScheduleService(_uowMock.Object, mapper);
        }

        [Fact]
        public async Task List_returns_list_of_artist_models()
        {
            // Arrange
            int page = 1;
            _scheduleRepositoryMock.Setup(pr => pr.Paged(page))
                                  .ReturnsAsync(() => new PagedResult<Schedule>())
                                  .Verifiable();

            // Act
            var result = await _scheduleService.List(page);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.IsType<PagedResult<ScheduleListModel>>(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForDetail_should_return_null_if_schedule_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullSchedule = (Schedule)null;
            _scheduleRepositoryMock.Setup(pr => pr.GetSchedule(nonExistentId))
                                  .ReturnsAsync(() => nullSchedule)
                                  .Verifiable();

            // Act
            var result = await _scheduleService.GetForDetail(nonExistentId);

            // Assert
            Assert.Null(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForDetail_should_return_artist()
        {
            // Arrange
            var id = 1;
            var schedule = new Schedule { ScheduleId = id };
            _scheduleRepositoryMock.Setup(pr => pr.GetSchedule(id))
                                  .ReturnsAsync(() => schedule)
                                  .Verifiable();

            // Act
            var result = await _scheduleService.GetForDetail(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScheduleDetailModel>(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_null_if_schedule_was_not_found()
        {
            // Arrange
            var nonExistentId = -1;
            var nullSchedule = (Schedule)null;
            _scheduleRepositoryMock.Setup(pr => pr.Get(nonExistentId))
                                  .ReturnsAsync(() => nullSchedule)
                                  .Verifiable();

            // Act
            var result = await _scheduleService.GetForEdit(nonExistentId);

            // Assert
            Assert.Null(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public async Task GetForEdit_should_return_schedule()
        {
            // Arrange
            var id = 1;
            var schedule = new Schedule { ScheduleId = id };
            _scheduleRepositoryMock.Setup(pr => pr.Get(id))
                                  .ReturnsAsync(() => schedule)
                                  .Verifiable();

            // Act
            var result = await _scheduleService.GetForEdit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScheduleEditModel>(result);
            _scheduleRepositoryMock.VerifyAll();
        }

        [Fact]
        public void GetForCreate_should_return_model()
        {
            //Act
            var result = _scheduleService.GetForCreate();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ScheduleCreateModel>(result);
        }

        [Fact]
        public async Task Save_should_survive_null_model()
        {
            // Arrange
            var model = (ScheduleEditModel)null;

            // Act
            var response = await _scheduleService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_handle_missing_schedule()
        {
            // Arrange
            var id = 1;
            var model = new ScheduleEditModel { ScheduleId = id };
            var nullSchedule = (Schedule)null;
            _scheduleRepositoryMock.Setup(ar => ar.GetSchedule(id))
                                  .ReturnsAsync(() => nullSchedule)
                                  .Verifiable();

            // Act
            var response = await _scheduleService.Save(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Save_should_save_valid_schedule()
        {
            // Arrange
            var id = 1;
            var schedule = new Schedule { ScheduleId = id };
            var scheduleModel = new ScheduleEditModel { ScheduleId = id, Date = DateTime.Now };

            _scheduleRepositoryMock.Setup(pr => pr.GetSchedule(id))
                                  .ReturnsAsync(() => schedule)
                                  .Verifiable();
            _uowMock.Setup(uow => uow.CompleteAsync())
                                     .Verifiable();

            // Act
            var response = await _scheduleService.Save(scheduleModel);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _scheduleRepositoryMock.VerifyAll();
            _uowMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_survive_null_id()
        {
            // Arrange
            int? id = null;

            // Act
            var response = await _scheduleService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_should_survive_null_schedule()
        {
            // Arrange
            int id = 1;
            var nullSchedule = (Schedule)null;

            _scheduleRepositoryMock.Setup(sr => sr.Get(id)).ReturnsAsync(() => nullSchedule);

            // Act
            var response = await _scheduleService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Delete_should_delete_schedule()
        {
            // Arrange
            int id = 1;
            var schedule = new Schedule { ScheduleId = id, Date = DateTime.Parse("01-01-2022 00:00:00") };

            _scheduleRepositoryMock.Setup(sr => sr.Get(id)).ReturnsAsync(() => schedule);
            _scheduleRepositoryMock.Setup(st => st.Delete(id)).Verifiable();
            _uowMock.Setup(uow => uow.CompleteAsync()).Verifiable();

            // Act
            var response = await _scheduleService.Delete(id);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _scheduleRepositoryMock.VerifyAll();
            _uowMock.VerifyAll();
        }


        [Fact]
        public async Task Create_should_survive_null_model()
        {
            // Arrange
            var model = (ScheduleCreateModel)null;

            // Act
            var response = await _scheduleService.Create(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task Create_should_create_schedule()
        {
            // Arrange
            int id = 1;
            var model = new ScheduleCreateModel { ScheduleId = id, Date = DateTime.Parse("01-01-2000 00:00:00") };
            var songSchedules = new List<SongSchedule>();

            _scheduleRepositoryMock.Setup(sr => sr.Save(It.IsAny<Schedule>())).Verifiable();
            _uowMock.Setup(st => st.CompleteAsync()).Verifiable();

            // Act
            var response = await _scheduleService.Create(model);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Success);
            _uowMock.VerifyAll();
        }

        [Fact]
        public async Task Create_should_handle_missing_scheudle()
        {
            // Arrange
            var id = 1;
            var model = new ScheduleCreateModel { ScheduleId = id, Date = DateTime.Now, SongSchedules = new List<SongSchedule>() };
            var nullSchedule = (IEnumerable<IGrouping<int, Song>>)null;
            _scheduleRepositoryMock.Setup(sr => sr.SongsByTempo())
                                  .Returns(nullSchedule)
                                  .Verifiable();

            // Act
            var response = await _scheduleService.Create(model);

            // Assert
            Assert.NotNull(response);
            Assert.False(response.Success);
            _uowMock.VerifyAll();
        }

    }
}
