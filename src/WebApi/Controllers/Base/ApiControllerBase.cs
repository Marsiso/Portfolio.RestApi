using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Base;

public class ApiControllerBase<T> : ControllerBase
{
    private ILogger<T>? _logger;
    private IRepositoryManager? _repository;
    
    protected ILogger<T>? Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    protected IRepositoryManager? Repository => _repository ??= HttpContext.RequestServices.GetService<IRepositoryManager>();
}