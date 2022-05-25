using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class SongsControllerTest
    {
        private readonly Mock<ISongService> _songServiceMock;
        private readonly SongsController _songsController;

        public SongsControllerTest()
        {
            _songServiceMock = new Mock<ISongService>();
            _songsController = new SongsController(_songServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_list_of_songs()
        {
            // Arrange
            var paged = GetPagedSongListModel();
            _songServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                .ReturnsAsync(() => paged);

            // Act
            var result = await _songsController.Index(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(result.Model is PagedResult<SongListModel>);
        }

        [Fact]
        public async Task Index_should_return_default_view()
        {
            //Arrange
            string[] defaultNames = new[] { null, "Index" };
            var paged = GetPagedSongListModel();
            _songServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => paged);

            //Act
            var result = await _songsController.Index(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.Contains(result.ViewName, defaultNames);

        }

        [Fact]
        public async Task Index_should_survive_null_model()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.List(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Index(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Details(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _songServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Details(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_correct_result_when_artist_is_found()
        {
            //Arrange
            var model = GetSongListModel();
            var defaultViewNames = new[] { null, "Details" };
            _songServiceMock.Setup(serv => serv.GetForDetail(It.IsAny<int>()))
                              .ReturnsAsync(() => model);

            //Act
            var result = await _songsController.Details(model.ArtistId) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.IsType<SongListModel>(result.Model);
        }

        [Fact]
        public async Task Create_should_redirect_to_Index()
        {
            //Arrange
            var model = GetSongCreationModel();
            _songServiceMock.Setup(serv => serv.Create(It.IsAny<SongCreationModel>()));

            //Act
            var result = await _songsController.Create(model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);

        }

        [Fact]
        public async Task Create_should_stay_on_form_when_model_is_invalid()
        {
            var model = GetSongCreationModel();
            var defaultViewNames = new[] { null, "Create" };

            //Act
            _songsController.ModelState.AddModelError("key", "errorMessage");
            var result = await _songsController.Create(model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.Contains(result.ViewName, defaultViewNames);
            Assert.False(_songsController.ModelState.IsValid);
            Assert.IsType<SongCreationModel>(result.Model);
        }

        [Fact]
        public async Task Create_should_return_correct_view()
        {
            //Arrange
            var model = GetSongCreationModel();
            var defaultViewNames = new[] { null, "Create" };
            _songServiceMock.Setup(serv => serv.GetForCreate()).ReturnsAsync(() => model);

            //Act
            var result = await _songsController.Create() as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<SongCreationModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_return_badresult_when_model_is_null()
        {
            // Arrange
            var song = (SongEditModel)null;
            var songId = 1;

            // Act
            var result = await _songsController.Edit(songId, song) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_id_is_null()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Edit(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            int nonExistantid = -1;
            _songServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Edit(nonExistantid) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Edit_should_return_not_found_if_artist_does_not_exist_on_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetSongEditModel();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()))
                              .Throws(exception);
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Edit(model.SongId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_shold_throw_DbUpdateConcurrencyException()
        {
            //Arrange
            var model = GetSongEditModel();
            var song = GetSong();
            var exception = new DbUpdateConcurrencyException(string.Empty, new List<IUpdateEntry> { Mock.Of<IUpdateEntry>() });
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()))
                              .Throws(exception);
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>())).ReturnsAsync(() => song);

            //Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _songsController.Edit(model.SongId, model));
        }

        [Fact]
        public async Task Edit_Post_should_return_not_found_if_id_is_not_correct()
        {
            //Arrange
            var model = GetSongEditModel();

            //Act
            var result = await _songsController.Edit(1000) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_redirect_to_index_after_saving_model()
        {
            //Arrange
            var model = GetSongEditModel();
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()));

            //Act
            var result = await _songsController.Edit(model.SongId, model) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_return_correct_on_invalid_modelstate()
        {
            //Arrange
            var model = GetSongEditModel();
            var dict = new ModelStateDictionary();
            _songServiceMock.Setup(serv => serv.Edit(It.IsAny<SongEditModel>()));
            _songsController.ViewData.ModelState.AddModelError("key", "errorMessage");

            //Act
            var result = await _songsController.Edit(model.SongId, model) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<SongEditModel>(result.Model);
        }

        [Fact]
        public async Task Edit_should_return_correct_model()
        {
            //Arrange
            var model = GetSongEditModel();
            _songServiceMock.Setup(serv => serv.GetForEdit(It.IsAny<int>()))
                  .ReturnsAsync(() => model);

            //Act
            var result = await _songsController.Edit(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<SongEditModel>(result.Model);
        }

        [Fact]
        public async Task Delete_should_redirect_to_index()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.Delete(It.IsAny<int>()));

            //Act
            var result = await _songsController.DeleteConfirmed(1) as RedirectToActionResult;

            //Assert
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_shoul_return_not_found_if_id_is_null()
        {
            //Act
            var result = await _songsController.Delete(null) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_not_found_if_artist_is_null()
        {
            //Arrange
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(() => null);

            //Act
            var result = await _songsController.Delete(1) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_correct_model()
        {
            //Arrange
            var model = new Song
            {
                SongId = 1,
                Title = "Kodumaine Viis",
                Tempo = 1,
                ArtistId = 1,
                Artist = new Artist { ArtistId = 1, Name = "Heino Eller", Description = "Lorem ipsum." }
            };
            _songServiceMock.Setup(serv => serv.GetForDelete(It.IsAny<int>()))
                              .ReturnsAsync(model);

            //Act
            var result = await _songsController.Delete(1) as ViewResult;

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.IsType<Song>(result.Model);
        }

        [Fact]
        public async Task Edit_return_not_found_if_given_id_does_not_match_model_id()
        {
            //Arrange
            var model = GetSongEditModel();
            int passedId = 565643;

            //Act
            var result = await _songsController.Edit(passedId, model) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        private PagedResult<SongListModel> GetPagedSongListModel()
        {
            return new PagedResult<SongListModel>()
            {
                Results = new List<SongListModel>()
                {
                    new SongListModel{ SongId = 1, Title = "Kodumaine Viis", Tempo = 1, Code = "100a4", Artist = "Heino Eller", ArtistId = 1},
                    new SongListModel{ SongId = 2, Title = "Spigel im spigel", Tempo = 1, Code = "100b5", Artist = "Arvo Pärt", ArtistId = 2}
                },
                selectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "John Doe", Value = "1" },
                    new SelectListItem { Text = "Tom Oak", Value = "2" }
                },
                CurrentPage = 1,
                RowCount = 3,
                PageCount = 5,
                PageSize = 2
            };
        }

        private SongListModel GetSongListModel()
        {
            return new SongListModel { SongId = 2, Title = "Spigel im spigel", Tempo = 1, Code = "100b5", Artist = "Arvo Pärt", ArtistId = 2 };
        }

        private SongCreationModel GetSongCreationModel()
        {
            return new SongCreationModel {SongId = 1, Title = "Finlandia", Tempo = 2, ArtistId = 3};
        }

        private SongEditModel GetSongEditModel()
        {
            return new SongEditModel { SongId = 1, Title = "Kungla rahvas", Tempo = 3, ArtistId = 4 };
        }

        private Song GetSong()
        {
            return new Song { SongId = 10, Title = "Numb", Tempo = 1, ArtistId = 2, Artist = new Artist(), Storage = new Storage(), SongSchedules = new List<SongSchedule>()};
        }
    }
}
