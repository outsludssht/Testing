using Microsoft.Extensions.Configuration;
using RestSharp;
using NUnit.Framework;
using Serilog;

public class TestBase
{
    protected RestClient Client;
    protected string Token;
    protected Microsoft.Extensions.Configuration.IConfiguration Configuration;

    [SetUp]
    public void Setup()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
        Configuration = builder.Build();

        Token = Configuration["Token"];
        Client = new RestClient(Configuration["MainUrl"]);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
    }

    protected Books createdBook;

    [TearDown]
    public async Task Cleanup()
    {
        if (createdBook != null)
        {
            var request = new RestRequest($"Books/{createdBook.Id}", Method.Delete);
            request.AddHeader("Authorization", $"Bearer {Token}");

            var response = await Client.ExecuteAsync(request);

            Log.Information("Deleted test book with ID: {BookId}, Status: {StatusCode}", createdBook.Id, response.StatusCode);

            createdBook = null;
        }
    }
}

