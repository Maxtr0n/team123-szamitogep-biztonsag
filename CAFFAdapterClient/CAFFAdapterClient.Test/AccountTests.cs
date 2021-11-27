using CAFFAdapterClient.DataTransferObjects.Account;
using CAFFAdapterClient.Domain.Enums;
using CAFFAdapterClient.Test.Factories;
using CAFFAdapterClient.ViewModels.Account;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CAFFAdapterClient.Test
{
    public class AccountTests : IClassFixture<CaffAdapterClientWebFactory>
    {
        private readonly CaffAdapterClientWebFactory _apiClientFactory;

        public AccountTests(CaffAdapterClientWebFactory apiClientFactory)
        {
            _apiClientFactory = apiClientFactory;
        }

        [Fact]
        public async Task Login()
        {
            var client = _apiClientFactory.CreateClient();

            var json = JsonConvert.SerializeObject(new LoginUserDto
            {
                Email = "admin@example.com",
                Password = "Admin123."
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("/account/login", data);

            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);

            var responseContent = await responseMessage.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<LoginUserViewModel>(responseContent);

            Assert.NotNull(response);
            Assert.Equal(true, response.IsSuccess);
            Assert.Equal(UserRoles.Admin, response.Role);
            Assert.NotEqual(string.Empty, response.Token);
        }
    }
}