using Domain.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebApi.Controllers;

public sealed class TestController : ApiControllerBase<TestController>
{
    private record TestData(int Id, Guid Payload);
    private static readonly IList<TestData> Data;

    static TestController()
    {
        Data = new List<TestData>();
        for (var index = 0; index < 10; index++)
        {
            Data.Add(new TestData(index, Guid.NewGuid() ));
        }
    }

    [HttpGet(ApiRoutes.Test.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(Data);
    }
}