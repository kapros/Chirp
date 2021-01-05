namespace Chirp.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public const string Version = "v1";

        public const string Root = Base + "/" + Version;

        public static class Posts
        {
            public const string PostsRoot = Root + "/posts";

            private const string WithId = Root + "/posts/{postId}";

            public const string Get = PostsRoot;

            public const string Create = PostsRoot;

            public const string GetById = WithId;

            public const string Update = WithId;

            public const string Delete = WithId;
        }

        public static class Tags
        {
            public const string TagsRoot = Root + "/tags";

            public const string WithName = TagsRoot + "/{tagName}";

            public const string GetByName = WithName;

            public const string Delete = WithName;

            public const string Put = WithName;
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";

            public const string Delete = Base + "/identity/delete";
        }

        public static class Jobs
        {
            public const string Get = Base + "/jobs/{jobId}";

            public const string AdminGet = Base + "/admin/{jobId}";
        }
    }
}
