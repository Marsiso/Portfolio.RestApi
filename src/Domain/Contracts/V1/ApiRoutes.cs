namespace Domain.Contracts.V1;

public static class ApiRoutes
{
    public const string Root = "api";
    public const string Version = "v1";
    public const string Base = Root + "/" + Version;
    public static class Test
    {
        public const string GetAll = Base + "/test";
    }

    public static class User
    {
        public const string GetAll = Base + "/user";
        public const string GetById = Base + "/user/{userId:long}";
        public const string GetByUserName = Base + "/user/{userName}";
    }
}