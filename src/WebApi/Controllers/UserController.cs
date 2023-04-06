using Domain.Contracts.V1;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Base;

namespace WebApi.Controllers;

public sealed class UserController : ApiControllerBase<UserController>
{
    [HttpGet(ApiRoutes.User.GetById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(long userId)
    {
        try
        {
            var userEntity = await Repository!.User.GetByIdAsync(userId, false);
            if (userEntity == null) return NotFound();
            return Ok(userEntity);
        }
        catch (Exception exception)
        {
            Logger?.LogError("[Controller]: '{Controller}' [Message]: '{Message}'",
                nameof(UserController),
                exception.ToString());

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet(ApiRoutes.User.GetByUserName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByUserName(string userName)
    {
        try
        {
            var userEntity = await Repository!.User.GetByUserNameAsync(userName, false);
            if (userEntity == null) return NotFound();
            return Ok(userEntity);
        }
        catch (Exception exception)
        {
            Logger?.LogError("[Controller]: '{Controller}' [Message]: '{Message}'",
                nameof(UserController),
                exception.ToString());

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet(ApiRoutes.User.GetAll)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userEntities = await Repository!.User.GetAllAsync(false);
            if (userEntities?.Count() < 1) return NoContent();
            return Ok(userEntities);
        }
        catch (Exception exception)
        {
            Logger?.LogError("[Controller]: '{Controller}' [Message]: '{Message}'",
                nameof(UserController),
                exception.ToString());

            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}