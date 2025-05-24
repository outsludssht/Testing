using Allure.NUnit;
using NUnit.Framework;
using RestSharp;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using Serilog;
using Allure.NUnit.Attributes;

[AllureNUnit]
[AllureSuite("Books API Tests")]
public class BooksTests : TestBase
{
    private Books createdBook;

    [Test, Order(1)]
    [AllureTag("POST")]
    [AllureSubSuite("Create Book")]
    [AllureDescription("Verify that a valid book is successfully created")]
    public async Task CreateBook_ShouldReturn201AndCorrectData()
    {
        Log.Information("Creating a new book");

        var uniqueIsbn = $"111-{Guid.NewGuid().ToString().Substring(0, 10)}";

        var request = CreateBookRequest("Test Book", "Author 1", uniqueIsbn);
        var response = await Client.ExecuteAsync(request);

        Log.Information("Response: {StatusCode} - {Content}", response.StatusCode, response.Content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        createdBook = JsonConvert.DeserializeObject<Books>(response.Content);
        createdBook.Title.Should().Be("Test Book");
        createdBook.Author.Should().Be("Author 1");
        createdBook.Isbn.Should().Be(uniqueIsbn);
    }

    [Test, Order(2)]
    [AllureTag("POST")]
    [AllureSubSuite("Create Book")]
    [AllureDescription("Verify duplicate book creation handling")]
    public async Task CreateBook_ShouldReturnConflict_OnDuplicate()
    {
        var request = CreateBookRequest("Test Book", "Author 1", "111-1111111");
        var response = await Client.ExecuteAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Test, Order(3)]
    [AllureTag("GET")]
    [AllureSubSuite("Get All Books")]
    [AllureDescription("Verify the API returns a list of all books with required fields")]
    public async Task GetAllBooks_ShouldReturnListWithValidBooks()
    {
        var request = new RestRequest("Books", Method.Get);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var books = JsonConvert.DeserializeObject<List<Books>>(response.Content);
        books.Should().NotBeNullOrEmpty();
        books.First().Title.Should().NotBeNullOrWhiteSpace();
        books.First().Author.Should().NotBeNullOrWhiteSpace();
        books.First().PublishedDate.Should().BeBefore(DateTime.UtcNow.AddMinutes(1));
    }

    [Test, Order(4)]
    [AllureTag("GET")]
    [AllureSubSuite("Get Book by ID")]
    [AllureDescription("Verify fetching a valid book by ID returns correct data")]
    public async Task GetBookById_ShouldReturnCorrectBook_WhenIdIsValid()
    {
        var request = new RestRequest($"Books/{createdBook.Id}", Method.Get);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var book = JsonConvert.DeserializeObject<Books>(response.Content);
        book.Id.Should().Be(createdBook.Id);
    }

    [Test, Order(5)]
    [AllureTag("GET")]
    [AllureSubSuite("Get Book by ID")]
    [AllureDescription("Verify 404 for non-existent book ID")]
    public async Task GetBookById_ShouldReturn404_WhenIdDoesNotExist()
    {
        var request = new RestRequest("Books/00000000-0000-0000-0000-000000000000", Method.Get);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test, Order(6)]
    [AllureTag("GET")]
    [AllureSubSuite("Get Book by ID")]
    [AllureDescription("Verify 400 for invalid book ID format")]
    public async Task GetBookById_ShouldReturn400_WhenIdIsInvalid()
    {
        var request = new RestRequest("Books/invalid-id", Method.Get);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test, Order(7)]
    [AllureTag("PUT")]
    [AllureSubSuite("Update Book")]
    [AllureDescription("Verify successful book update and data changes")]
    public async Task UpdateBook_ShouldReturn204AndReflectChanges()
    {
        var request = new RestRequest($"Books/{createdBook.Id}", Method.Put);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var updatedData = new
        {
            title = "Updated Book",
            author = "Updated Author",
            isbn = createdBook.Isbn,
            publishedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        request.AddJsonBody(updatedData);

        var response = await Client.ExecuteAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getRequest = new RestRequest($"Books/{createdBook.Id}", Method.Get);
        getRequest.AddHeader("Authorization", $"Bearer {Token}");
        var getResponse = await Client.ExecuteAsync(getRequest);
        var updatedBook = JsonConvert.DeserializeObject<Books>(getResponse.Content);

        updatedBook.Title.Should().Be("Updated Book");
        updatedBook.Author.Should().Be("Updated Author");
    }

    [Test, Order(8)]
    [AllureTag("PUT")]
    [AllureSubSuite("Update Book")]
    [AllureDescription("Verify 404 when updating non-existent book")]
    public async Task UpdateBook_ShouldReturn404_WhenIdDoesNotExist()
    {
        var request = new RestRequest("Books/00000000-0000-0000-0000-000000000000", Method.Put);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var body = new
        {
            title = "Doesn't matter",
            author = "Nobody",
            isbn = "000-0000000000",
            publishedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        request.AddJsonBody(body);
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test, Order(9)]
    [AllureTag("PUT")]
    [AllureSubSuite("Update Book")]
    [AllureDescription("Verify 400 for invalid book ID during update")]
    public async Task UpdateBook_ShouldReturn400_WhenIdIsInvalid()
    {
        var request = new RestRequest("Books/invalid-id", Method.Put);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var body = new
        {
            title = "Invalid",
            author = "Data",
            isbn = "000-0000000000",
            publishedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        request.AddJsonBody(body);
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test, Order(10)]
    [AllureTag("DELETE")]
    [AllureSubSuite("Delete Book")]
    [AllureDescription("Verify successful book deletion and post-deletion retrieval failure")]
    public async Task DeleteBook_ShouldReturn204AndMakeBookUnavailable()
    {
        var deleteRequest = new RestRequest($"Books/{createdBook.Id}", Method.Delete);
        deleteRequest.AddHeader("Authorization", $"Bearer {Token}");
        var deleteResponse = await Client.ExecuteAsync(deleteRequest);
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getRequest = new RestRequest($"Books/{createdBook.Id}", Method.Get);
        getRequest.AddHeader("Authorization", $"Bearer {Token}");
        var getResponse = await Client.ExecuteAsync(getRequest);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test, Order(11)]
    [AllureTag("DELETE")]
    [AllureSubSuite("Delete Book")]
    [AllureDescription("Verify 404 when deleting non-existent book")]
    public async Task DeleteBook_ShouldReturn404_WhenIdDoesNotExist()
    {
        var request = new RestRequest("Books/00000000-0000-0000-0000-000000000000", Method.Delete);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test, Order(12)]
    [AllureTag("DELETE")]
    [AllureSubSuite("Delete Book")]
    [AllureDescription("Verify 400 when deleting with invalid book ID")]
    public async Task DeleteBook_ShouldReturn400_WhenIdIsInvalid()
    {
        var request = new RestRequest("Books/invalid-id", Method.Delete);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var response = await Client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private RestRequest CreateBookRequest(string title, string author, string isbn)
    {
        var request = new RestRequest("Books", Method.Post);
        request.AddHeader("Authorization", $"Bearer {Token}");
        var book = new
        {
            title = title,
            author = author,
            isbn = isbn,
            publishedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        request.AddJsonBody(book);
        return request;
    }
}