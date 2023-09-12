using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Chirp.Contracts.V1;
using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Responses;
using Chirp.Data;
using System.Text;

namespace Chirp.IntegrationTests
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private IServiceProvider _serviceProvider;

        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                    });
                });
            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var request = new UserRegistrationRequest
            {
                Email = "integrationtests@chirp.com",
                Password = "Test1234"
            };
            var stringPayload = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var response = await TestClient.PostAsync(ApiRoutes.Identity.Login, content);

            var registrationResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthSuccessResponse>(await response.Content.ReadAsStringAsync());
            return registrationResponse.Token;
        }

        protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
        {
            var stringPayload = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            var response = await TestClient.PostAsync(ApiRoutes.Posts.Create, content);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<PostResponse>(await response.Content.ReadAsStringAsync());
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }
    }
}