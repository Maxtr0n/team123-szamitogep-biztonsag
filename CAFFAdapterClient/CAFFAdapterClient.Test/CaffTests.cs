using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.DataTransferObjects.CaffFiles;
using CAFFAdapterClient.Domain.Enums;
using CAFFAdapterClient.Test.Factories;
using CAFFAdapterClient.ViewModels;
using CAFFAdapterClient.ViewModels.Account;
using CAFFAdapterClient.ViewModels.CaffFiles;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CAFFAdapterClient.Test
{
    public class CaffTests : IClassFixture<CaffAdapterClientWebFactory>
    {
        private readonly CaffAdapterClientWebFactory _apiClientFactory;

        public CaffTests(CaffAdapterClientWebFactory apiClientFactory)
        {
            _apiClientFactory = apiClientFactory;
        }

        [Fact]
        public async Task Upload()
        {
            var client = _apiClientFactory.CreateClient();

            var responseMessage = await client.PostAsync("/account/login", new StringContent(JsonConvert.SerializeObject(new LoginUserDto
            {
                Email = "admin@example.com",
                Password = "Admin123."
            }), Encoding.UTF8, "application/json"));

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<LoginUserViewModel>(responseContent);
            var token = response.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var file = await File.ReadAllBytesAsync("Inputs/1.caff");
            var content = new StringContent(JsonConvert.SerializeObject(new CreateCaffFileDto
            {
                File = file
            }), Encoding.UTF8, "application/json");
            var responseCaffUploadMessage = await client.PostAsync("/cafffiles", content);

            Assert.Equal(HttpStatusCode.OK, responseCaffUploadMessage.StatusCode);
        }

        [Fact]
        public async Task Get()
        {
            var client = _apiClientFactory.CreateClient();

            var responseMessage = await client.PostAsync("/account/login", new StringContent(JsonConvert.SerializeObject(new LoginUserDto
            {
                Email = "admin@example.com",
                Password = "Admin123."
            }), Encoding.UTF8, "application/json"));

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<LoginUserViewModel>(responseContent);
            var token = response.Token;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(new CreateCaffFileDto
            {
                File = null
            }), Encoding.UTF8, "application/json");
            await client.PostAsync("/cafffiles", content);

            var responseCaffGetMessage = await client.GetAsync("/cafffiles");

            Assert.Equal(HttpStatusCode.OK, responseCaffGetMessage.StatusCode);

            var responseCaffGetContent = await responseCaffGetMessage.Content.ReadAsStringAsync();
            var caffFiles = JsonConvert.DeserializeObject<TableViewModel<CaffFileRowViewModel>>(responseCaffGetContent);

            Assert.Equal(1, caffFiles.Count);
        }
    }
}