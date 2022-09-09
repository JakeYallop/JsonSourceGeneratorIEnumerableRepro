using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonSourceGenIEnumerableRepro;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var l = new List<MyClass> { new(), new() };
        //using the STJ source gen default infrastructure succeeds without failing
        var output = JsonSerializer.Serialize(l, JsonContext.Default.IEnumerableMyClass);

        builder.Services
            .AddMvcCore()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.AddContext<JsonContext>();
            });


        var app = builder.Build();

        app.MapControllers();
        app.Run();
    }
}


[Route("api/test")]
public class MyController : ControllerBase
{
    public MyController()
    {
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var l = new List<MyClass> { new(), new() };
        return Ok(l);
    }
}

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(IEnumerable<MyClass>))]
public partial class JsonContext : JsonSerializerContext
{

}

public class MyClass
{
    public int MyProp { get; set; }
}