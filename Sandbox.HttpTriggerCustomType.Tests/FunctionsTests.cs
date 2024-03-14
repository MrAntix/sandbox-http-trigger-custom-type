using HttpTriggerCustomType;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Xunit.Abstractions;

namespace Sandbox.HttpTriggerCustomType.Tests;

public class FunctionsTests : IDisposable
{
    const int PORT = 7020;
    readonly IDisposable _functions;
    readonly HttpClient _http;

    public FunctionsTests(ITestOutputHelper output)
    {
        _functions = FunctionsHelper.StartHost(PORT, output);
        _http = new HttpClient();
        _http.BaseAddress = new Uri($"http://localhost:{PORT}/api/");
    }

    [Fact]
    public async Task TriggerModelAsync()
    {
        var response = await _http.PostAsync(
            ROUTES.TRIGGER,
            new StringContent(
                JsonSerializer.Serialize(new Model(99)),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            ));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BindModelAsync()
    {
        var response = await _http.PostAsync(
            ROUTES.BIND,
            new StringContent(
                JsonSerializer.Serialize(new Model(99)),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
            ));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public void Dispose()
    {
        _http.Dispose();
        _functions.Dispose();
    }
}
