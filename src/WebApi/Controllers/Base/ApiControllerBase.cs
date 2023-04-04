using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Base;

public class ApiControllerBase<T> : ControllerBase
{
    private ILogger<T>? _logger;
    protected ILogger<T>? Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
}