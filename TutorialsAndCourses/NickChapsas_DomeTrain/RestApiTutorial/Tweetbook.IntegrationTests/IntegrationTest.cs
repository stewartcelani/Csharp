using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tweetbook.Cache;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Data;

namespace Tweetbook.IntegrationTests;

public class IntegrationTest : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    protected readonly HttpClient TestClient;

    protected IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<DataContext>));
                    services.RemoveAll(typeof(RedisCacheSettings));
                    services.AddSingleton(new RedisCacheSettings
                    {
                        Enabled = false
                    });
                    services.AddDbContext<DataContext>(options => { options.UseInMemoryDatabase("TestDb"); });
                });
            });

        _serviceProvider = appFactory.Services;
        TestClient = appFactory.CreateClient();
    }

    public void Dispose()
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<DataContext>();
        context.Database.EnsureDeleted();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
    {
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
        return (await response.Content.ReadAsAsync<Response<PostResponse>>()).Data;
    }

    private async Task<string> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
        {
            Email = "test@integration.com",
            Password = "SomePass1234!"
        });

        var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
        return registrationResponse.Token;
    }
}