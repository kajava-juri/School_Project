using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class SongsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/Songs")]
        [InlineData("/Songs/Details/1")]
        [InlineData("/Songs/Edit/1")]
        [InlineData("/Songs/Delete/1")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Edit_EndpointReturnSuccessForCorrectModel()
        {
            // Arrange
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var id = 1;

            var formValues = new Dictionary<string, string>();
            formValues.Add("SongId", $"{id}");
            formValues.Add("ArtistId", $"{id}");
            formValues.Add("Title", "y");
            formValues.Add("Tempo", "1");

            var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await client.PostAsync("/Songs/Edit/" + id, content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task Edit_StaysOnThePageOnInvalidValidation()
        {
            // Arrange
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var id = 1;

            var formValues = new Dictionary<string, string>();
            formValues.Add("SongId", $"{id}");
            formValues.Add("ArtistId", $"{id}");
            formValues.Add("Title", "");
            formValues.Add("Tempo", "1");

            var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await client.PostAsync("/Songs/Edit/" + id, content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_EndpointReturnSuccessForCorrectModel()
        {
            // Arrange
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var id = 1;

            // Act
            var response = await client.PostAsync("/Songs/Delete/" + id, null);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task Create_EndpointReturnSuccessForCorrectModel()
        {
            // Arrange
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            int id = 1;

            var formValues = new Dictionary<string, string>();
            formValues.Add("ArtistId", $"{id}");
            formValues.Add("Title", "Everybody wants to rule the world");
            formValues.Add("Tempo", "1");
            formValues.Add("Storage.Kood", "K456J");

            var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await client.PostAsync("/Songs/Create/", content);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task Create_StaysOnThePageOnInvalidValidation()
        {
            // Arrange
            var client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            int invalidId = 100000;

            var formValues = new Dictionary<string, string>();
            formValues.Add("ArtistId", $"{invalidId}");
            formValues.Add("Title", "");
            formValues.Add("Tempo", "1");
            formValues.Add("Storage.Kood", "K456J");

            var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await client.PostAsync("/Songs/Create/", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
