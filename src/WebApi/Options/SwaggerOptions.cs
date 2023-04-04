namespace WebApi.Options;

/// <summary>
/// Model used for Swagger Open API configuration during application route configuration.
/// </summary>
public sealed class SwaggerOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string RouteTemplate { get; set; }

    /// <summary>
    /// Swagger Open API description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// User interface endpoint URL to access API via browser client. 
    /// </summary>
    public string UiEndpoint { get; set; }
}