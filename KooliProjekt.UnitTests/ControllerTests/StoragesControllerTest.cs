using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.Controllers;
using Moq;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.AspNetCore.Http;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class StoragesControllerTest
    {
        private readonly Mock<IStorageService> _storageServiceMock;
        private readonly StoragesController _storageController;

        public StoragesControllerTest()
        {
            _storageServiceMock = new Mock<IStorageService>();
            _storageController = new StoragesController(_storageServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_storages()
        {
            // Arrange
            var paged = GetStorageListModel();
            _storageServiceMock.Setup(serv => serv.StorageList(It.IsAny<int>()))
                .ReturnsAsync(() => paged);

            // Act
            var result = await _storageController.Index(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<StorageListModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Index" };
            var paged = GetStorageListModel();
            _storageServiceMock.Setup(serv => serv.StorageList(It.IsAny<int>()))
                              .ReturnsAsync(() => paged);

            //Act
            var result = await _storageController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.StorageList(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Index(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _storageServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Details(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_artist_is_found()
        {
            //Arrange
            var model = GetStorageDetailModel();
            var defaultViewNames = new[] { null, "Details" };
            _storageServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _storageController.Details(model.StorageID) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<StorageDetailModel>(result.Model);
        }

        [Fact]
        public async Task Delete_should_redirect_to_index()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.Delete(It.IsAny<int>()));

            //Act
            var result = await _storageController.DeleteConfirmed(1) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_shoul_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _storageController.Delete(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            _storageServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _storageController.Delete(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_correct_model()
        {
            //Arrange
            var model = GetStorage();
            _storageServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(model);

            //Act
            var result = await _storageController.Delete(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<Storage>(result.Model);
        }

        private PagedResult<StorageListModel> GetStorageListModel()
        {
            return new PagedResult<StorageListModel>()
            {
                Results = new List<StorageListModel>()
                {
                    new StorageListModel{ StorageID = 1, Kood = "123ABC", Song = new SongViewModel()},
                    new StorageListModel{ StorageID = 2, Kood = "456DEF", Song = new SongViewModel() }
                },
                selectList = new List<SelectListItem>(),
                CurrentPage = 1,
                RowCount = 3,
                PageCount = 5,
                PageSize = 2
            };
        }

        private StorageDetailModel GetStorageDetailModel()
        {
            return new StorageDetailModel { StorageID = 3, Kood = "321CBA", Name = "Numb", ArtistId = 8, Song = new SongViewModel() };
        }

        private Storage GetStorage()
        {
            return new Storage { StorageID = 5, Kood = "567VBN", SongId = 9, Song = new Song() };
        }
    }
}
