using Refit;
using System;
using System.Threading.Tasks;

namespace Chirp.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;

            var identityApi = RestService.For<IIdentityApi>("https://localhost:5001");

            var chirpApi = RestService.For<IChirpApi>("https://localhost:5001", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var registerResponse = await identityApi.RegisterAsync(new Contracts.V1.Requests.UserRegistrationRequest
            {
                Email = "sdkaccount12345@mail.com",
                Password = "Test1234!"
            });

            var loginResponse = await identityApi.LoginAsync(new Contracts.V1.Requests.UserLoginRequest
            {
                Email = "sdkaccount12345@mail.com",
                Password = "Test1234!"
            });

            cachedToken = loginResponse.Content.Token;

            var allPosts = await chirpApi.GetAllPostsAsync();
            var createdPost = await chirpApi.CreatePostAsync(new Contracts.V1.Requests.CreatePostRequest
            {
                Name = "this was created by sdk",
                Tags = new[] { "sdk" }
            });

            var retrievedPost = await chirpApi.GetPostAsync(createdPost.Content.Id);

            var updatedPost = await chirpApi.UpdatePostAsync(createdPost.Content.Id, new Contracts.V1.Requests.UpdatePostRequest
            {
                Name = "this was updated by sdk"
            });

            var deletePost = await chirpApi.DeletePostAsync(createdPost.Content.Id);
        }
    }
}
