[![Build](https://github.com/christianhelle/refitter/actions/workflows/build.yml/badge.svg)](https://github.com/christianhelle/refitter/actions/workflows/build.yml)
[![Smoke Tests](https://github.com/christianhelle/refitter/actions/workflows/smoke-tests.yml/badge.svg)](https://github.com/christianhelle/refitter/actions/workflows/smoke-tests.yml)
[![NuGet](https://img.shields.io/nuget/v/refitter?color=blue)](http://www.nuget.org/packages/refitter)
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-13-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->

# Refitter
Refitter is a CLI tool for generating a C# REST API Client using the [Refit](https://github.com/reactiveui/refit) library. Refitter can generate the Refit interface from OpenAPI specifications

## Installation:

The tool is packaged as a .NET Tool and is published to nuget.org. You can install the latest version of this tool like this:

```shell
dotnet tool install --global Refitter
```

## Usage:

```shell
$ refitter --help
```

```
USAGE:
    refitter [URL or input file] [OPTIONS]

EXAMPLES:
    refitter ./openapi.json
    refitter https://petstore3.swagger.io/api/v3/openapi.yaml
    refitter ./openapi.json --namespace "Your.Namespace.Of.Choice.GeneratedCode" --output ./GeneratedCode.cs
    refitter ./openapi.json --namespace "Your.Namespace.Of.Choice.GeneratedCode" --internal
    refitter ./openapi.json --output ./IGeneratedCode.cs --interface-only
    refitter ./openapi.json --use-api-response
    refitter ./openapi.json --cancellation-tokens
    refitter ./openapi.json --no-operation-headers
    refitter ./openapi.json --use-iso-date-format

ARGUMENTS:
    [URL or input file]    URL or file path to OpenAPI Specification file

OPTIONS:
                                      DEFAULT                                                                           
    -h, --help                                         Prints help information                                          
    -n, --namespace                   GeneratedCode    Default namespace to use for generated types                     
    -o, --output                      Output.cs        Path to Output file                                              
        --no-auto-generated-header                     Don't add <auto-generated> header to output file                 
        --interface-only                               Don't generate contract types                                    
        --use-api-response                             Return Task<IApiResponse<T>> instead of Task<T>                  
        --internal                                     Set the accessibility of the generated types to 'internal'       
        --cancellation-tokens                          Use cancellation tokens                                          
        --no-operation-headers                         Don't generate operation headers                                 
        --no-logging                                   Don't log errors or collect telemetry                            
        --use-iso-date-format                          Explicitly format date query string parameters in ISO 8601   
                                                       standard date format using delimiters (2023-06-15)
```

To generate code from an OpenAPI specifications file, run the following:

```shell
$ refitter [path to OpenAPI spec file] --namespace "[Your.Namespace.Of.Choice.GeneratedCode]"
```

This will generate a file called `Output.cs` which contains the Refit interface and contract classes generated using [NSwag](https://github.com/RicoSuter/NSwag)

# Using the generated code


Here's an example generated output from the [Swagger Petstore example](https://petstore3.swagger.io) using the default settings

```bash
$ refitter ./openapi.json --namespace "Your.Namespace.Of.Choice.GeneratedCode"
```

```cs
using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Your.Namespace.Of.Choice.GeneratedCode
{
    public interface ISwaggerPetstore
    {
        /// <summary>
        /// Update an existing pet by Id
        /// </summary>
        [Put("/pet")]
        Task<Pet> UpdatePet([Body] Pet body);

        /// <summary>
        /// Add a new pet to the store
        /// </summary>
        [Post("/pet")]
        Task<Pet> AddPet([Body] Pet body);

        /// <summary>
        /// Multiple status values can be provided with comma separated strings
        /// </summary>
        [Get("/pet/findByStatus")]
        Task<ICollection<Pet>> FindPetsByStatus([Query] Status? status);

        /// <summary>
        /// Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
        /// </summary>
        [Get("/pet/findByTags")]
        Task<ICollection<Pet>> FindPetsByTags([Query(CollectionFormat.Multi)] IEnumerable<string> tags);

        /// <summary>
        /// Returns a single pet
        /// </summary>
        [Get("/pet/{petId}")]
        Task<Pet> GetPetById(long petId);

        [Post("/pet/{petId}")]
        Task UpdatePetWithForm(long petId, [Query] string name, [Query] string status);

        [Delete("/pet/{petId}")]
        Task DeletePet(long petId, [Header("api_key")] string api_key);

        [Post("/pet/{petId}/uploadImage")]
        Task<ApiResponse> UploadFile(long petId, [Query] string additionalMetadata, StreamPart body);

        /// <summary>
        /// Returns a map of status codes to quantities
        /// </summary>
        [Get("/store/inventory")]
        Task<IDictionary<string, int>> GetInventory();

        /// <summary>
        /// Place a new order in the store
        /// </summary>
        [Post("/store/order")]
        Task<Order> PlaceOrder([Body] Order body);

        /// <summary>
        /// For valid response try integer IDs with value <= 5 or > 10. Other values will generated exceptions
        /// </summary>
        [Get("/store/order/{orderId}")]
        Task<Order> GetOrderById(long orderId);

        /// <summary>
        /// For valid response try integer IDs with value < 1000. Anything above 1000 or nonintegers will generate API errors
        /// </summary>
        [Delete("/store/order/{orderId}")]
        Task DeleteOrder(long orderId);

        /// <summary>
        /// This can only be done by the logged in user.
        /// </summary>
        [Post("/user")]
        Task CreateUser([Body] User body);

        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        [Post("/user/createWithList")]
        Task<User> CreateUsersWithListInput([Body] IEnumerable<User> body);

        [Get("/user/login")]
        Task<string> LoginUser([Query] string username, [Query] string password);

        [Get("/user/logout")]
        Task LogoutUser();

        [Get("/user/{username}")]
        Task<User> GetUserByName(string username);

        /// <summary>
        /// This can only be done by the logged in user.
        /// </summary>
        [Put("/user/{username}")]
        Task UpdateUser(string username, [Body] User body);

        /// <summary>
        /// This can only be done by the logged in user.
        /// </summary>
        [Delete("/user/{username}")]
        Task DeleteUser(string username);
    }
}
```

Here's an example generated output from the [Swagger Petstore example](https://petstore3.swagger.io) configured to wrap the return type in `IApiResponse<T>`

```bash
$ refitter ./openapi.json --namespace "Your.Namespace.Of.Choice.GeneratedCode" --use-api-response
```

```cs
using Refit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Your.Namespace.Of.Choice.GeneratedCode.WithApiResponse
{
    public interface ISwaggerPetstore
    {
        /// <summary>
        /// Update an existing pet by Id
        /// </summary>
        [Put("/pet")]
        Task<IApiResponse<Pet>> UpdatePet([Body] Pet body);

        /// <summary>
        /// Add a new pet to the store
        /// </summary>
        [Post("/pet")]
        Task<IApiResponse<Pet>> AddPet([Body] Pet body);

        /// <summary>
        /// Multiple status values can be provided with comma separated strings
        /// </summary>
        [Get("/pet/findByStatus")]
        Task<IApiResponse<ICollection<Pet>>> FindPetsByStatus([Query] Status? status);

        /// <summary>
        /// Multiple tags can be provided with comma separated strings. Use tag1, tag2, tag3 for testing.
        /// </summary>
        [Get("/pet/findByTags")]
        Task<IApiResponse<ICollection<Pet>>> FindPetsByTags([Query(CollectionFormat.Multi)] IEnumerable<string> tags);

        /// <summary>
        /// Returns a single pet
        /// </summary>
        [Get("/pet/{petId}")]
        Task<IApiResponse<Pet>> GetPetById(long petId);

        [Post("/pet/{petId}")]
        Task UpdatePetWithForm(long petId, [Query] string name, [Query] string status);

        [Delete("/pet/{petId}")]
        Task DeletePet(long petId, [Header("api_key")] string api_key);

        [Post("/pet/{petId}/uploadImage")]
        Task<IApiResponse<ApiResponse>> UploadFile(long petId, [Query] string additionalMetadata, StreamPart body);

        /// <summary>
        /// Returns a map of status codes to quantities
        /// </summary>
        [Get("/store/inventory")]
        Task<IApiResponse<IDictionary<string, int>>> GetInventory();

        /// <summary>
        /// Place a new order in the store
        /// </summary>
        [Post("/store/order")]
        Task<IApiResponse<Order>> PlaceOrder([Body] Order body);

        /// <summary>
        /// For valid response try integer IDs with value <= 5 or > 10. Other values will generated exceptions
        /// </summary>
        [Get("/store/order/{orderId}")]
        Task<IApiResponse<Order>> GetOrderById(long orderId);

        /// <summary>
        /// For valid response try integer IDs with value < 1000. Anything above 1000 or nonintegers will generate API errors
        /// </summary>
        [Delete("/store/order/{orderId}")]
        Task DeleteOrder(long orderId);

        /// <summary>
        /// This can only be done by the logged in user.
        /// </summary>
        [Post("/user")]
        Task CreateUser([Body] User body);

        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        [Post("/user/createWithList")]
        Task<IApiResponse<User>> CreateUsersWithListInput([Body] IEnumerable<User> body);

        [Get("/user/login")]
        Task<IApiResponse<string>> LoginUser([Query] string username, [Query] string password);

        [Get("/user/logout")]
        Task LogoutUser();

        [Get("/user/{username}")]
        Task<IApiResponse<User>> GetUserByName(string username);

        /// <summary>
        /// This can only be done by the logged in user.
        /// </summary>
        [Put("/user/{username}")]
        Task UpdateUser(string username, [Body] User body);

        /// <summary>
        /// This can only be done by the logged in user.
        /// </summary>
        [Delete("/user/{username}")]
        Task DeleteUser(string username);
    }
}
```

## RestService

Here's an example usage of the generated code above

```cs
using Refit;
using System;
using System.Threading.Tasks;

namespace Your.Namespace.Of.Choice.GeneratedCode;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var client = RestService.For<ISwaggerPetstore>("https://petstore3.swagger.io/api/v3");
        var pet = await client.GetPetById(1);

        Console.WriteLine("## Using Task<T> as return type ##");
        Console.WriteLine($"Name: {pet.Name}");
        Console.WriteLine($"Category: {pet.Category.Name}");
        Console.WriteLine($"Status: {pet.Status}");
        Console.WriteLine();

        var client2 = RestService.For<WithApiResponse.ISwaggerPetstore>("https://petstore3.swagger.io/api/v3");
        var response = await client2.GetPetById(2);

        Console.WriteLine("## Using Task<IApiResponse<T>> as return type ##");
        Console.WriteLine($"HTTP Status Code: {response.StatusCode}");
        Console.WriteLine($"Name: {response.Content.Name}");
        Console.WriteLine($"Category: {response.Content.Category.Name}");
        Console.WriteLine($"Status: {response.Content.Status}");
    }
}
```

The `RestService` class generates an implementation of `ISwaggerPetstore` that uses `HttpClient` to make its calls. 

The code above when run will output something like this:

```
## Using Task<T> as return type ##
Name: Gatitotototo
Category: Chaucito
Status: Sold

## Using Task<IApiResponse<T>> as return type ##
HTTP Status Code: OK
Name: Gatitotototo
Category: Chaucito
Status: Sold
```

## ASP.NET Core and HttpClientFactory

Here's an example Minimal API with the [`Refit.HttpClientFactory`](https://www.nuget.org/packages/Refit.HttpClientFactory) library:

```cs
using Refit;
using Your.Namespace.Of.Choice.GeneratedCode;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddRefitClient<ISwaggerPetstore>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://petstore3.swagger.io/api/v3"));

var app = builder.Build();
app.MapGet(
        "/pet/{id:long}",
        async (ISwaggerPetstore petstore, long id) =>
        {
            try
            {
                return Results.Ok(await petstore.GetPetById(id));
            }
            catch (Refit.ApiException e)
            {
                return Results.StatusCode((int)e.StatusCode);
            }
        })
    .WithName("GetPetById")
    .WithOpenApi();

app.UseHttpsRedirection();
app.UseSwaggerUI();
app.UseSwagger();
app.Run();
```

.NET Core supports registering the generated `ISwaggerPetstore` interface via `HttpClientFactory`

The following request to the API above
```shell
$ curl -X 'GET' 'https://localhost:5001/pet/1' -H 'accept: application/json'
```

Returns a response that looks something like this:
```json
{
  "id": 1,
  "name": "Special_char_owner_!@#$^&()`.testing",
  "photoUrls": [
    "https://petstore3.swagger.io/resources/photos/623389095.jpg"
  ],
  "tags": [],
  "status": "Sold"
}
```

## System requirements
.NET 6.0 (LTS)

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/neoGeneva"><img src="https://avatars.githubusercontent.com/u/804724?v=4?s=100" width="100px;" alt="Philip Cox"/><br /><sub><b>Philip Cox</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=neoGeneva" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://cam.macfar.land/"><img src="https://avatars.githubusercontent.com/u/1298847?v=4?s=100" width="100px;" alt="Cameron MacFarland"/><br /><sub><b>Cameron MacFarland</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=distantcam" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://kgame.tw"><img src="https://avatars.githubusercontent.com/u/3646532?v=4?s=100" width="100px;" alt="kgame"/><br /><sub><b>kgame</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=kgamecarter" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="http://yrki.no"><img src="https://avatars.githubusercontent.com/u/11573601?v=4?s=100" width="100px;" alt="Thomas Pettersen / Yrki"/><br /><sub><b>Thomas Pettersen / Yrki</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><img src="?s=100" width="100px;" alt="1kvin"/><br /><sub><b>1kvin</b></sub><br /><a href="https://github.com/christianhelle/refitter/issues?q=author%3A1kvin" title="Bug reports">🐛</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/m7clarke"><img src="https://avatars.githubusercontent.com/u/47439144?v=4?s=100" width="100px;" alt="m7clarke"/><br /><sub><b>m7clarke</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/issues?q=author%3Am7clarke" title="Bug reports">🐛</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/kirides"><img src="https://avatars.githubusercontent.com/u/13602143?v=4?s=100" width="100px;" alt="kirides"/><br /><sub><b>kirides</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/issues?q=author%3Akirides" title="Bug reports">🐛</a></td>
    </tr>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/guillaumeserale"><img src="https://avatars.githubusercontent.com/u/6672406?v=4?s=100" width="100px;" alt="guillaumeserale"/><br /><sub><b>guillaumeserale</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=guillaumeserale" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/Roflincopter"><img src="https://avatars.githubusercontent.com/u/1690243?v=4?s=100" width="100px;" alt="Dennis Brentjes"/><br /><sub><b>Dennis Brentjes</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=Roflincopter" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/hickeydamian/"><img src="https://avatars.githubusercontent.com/u/57436?v=4?s=100" width="100px;" alt="Damian Hickey"/><br /><sub><b>Damian Hickey</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/issues?q=author%3Adamianh" title="Bug reports">🐛</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/richardhu-lmg"><img src="https://avatars.githubusercontent.com/u/126430787?v=4?s=100" width="100px;" alt="richardhu-lmg"/><br /><sub><b>richardhu-lmg</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/issues?q=author%3Arichardhu-lmg" title="Bug reports">🐛</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/brease-colin"><img src="https://avatars.githubusercontent.com/u/47358935?v=4?s=100" width="100px;" alt="brease-colin"/><br /><sub><b>brease-colin</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/issues?q=author%3Abrease-colin" title="Bug reports">🐛</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://github.com/angelofb"><img src="https://avatars.githubusercontent.com/u/2032257?v=4?s=100" width="100px;" alt="angelofb"/><br /><sub><b>angelofb</b></sub></a><br /><a href="https://github.com/christianhelle/refitter/commits?author=angelofb" title="Code">💻</a></td>
    </tr>
  </tbody>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

#

For tips and tricks on software development, check out [my blog](https://christianhelle.com)

If you find this useful and feel a bit generous then feel free to [buy me a coffee ☕](https://www.buymeacoffee.com/christianhelle)
