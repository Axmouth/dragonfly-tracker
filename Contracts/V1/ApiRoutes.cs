namespace DragonflyTracker.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Posts
        {
            public const string GetAll = Base + "/posts";

            public const string Update = Base + "/posts/{postId}";

            public const string Delete = Base + "/posts/{postId}";

            public const string Get = Base + "/posts/{postId}";

            public const string Create = Base + "/posts";
        }
        
        public static class Tags
        {
            public const string GetAll = Base + "/tags";
            
            public const string Get = Base + "/tags/{tagName}";
            
            public const string Create = Base + "/tags";
            
            public const string Delete = Base + "/tags/{tagName}";
        }

        public static class Organizations
        {
            public const string GetAll = Base + "/organizations";

            public const string Update = Base + "/organizations/{organizationName}";

            public const string Get = Base + "/organizations/{organizationName}";

            public const string Create = Base + "/organizations";

            public const string Delete = Base + "/organizations/{organizationName}";
        }

        public static class Users
        {
            public const string GetAll = Base + "/users";

            public const string Update = Base + "/users/{username}";

            public const string Get = Base + "/users/{username}";

            public const string GetAllIssuePosts = Base + "/users/{username}/issue-posts";

            public const string Create = Base + "/users";

            public const string Delete = Base + "/users/{username}";
        }

        public static class Projects
        {
            public const string GetAllByOrg = Organizations.Get + "/projects";

            public const string UpdateByOrg = Organizations.Get + "/projects/{projectName}";

            public const string GetByOrg = Organizations.Get + "/projects/{projectName}";

            public const string CreateByOrg = Organizations.Get + "/projects";

            public const string DeleteByOrg = Organizations.Get + "/projects/{projectName}";

            public const string GetAllByUser = Users.Get + "/projects";

            public const string UpdateByUser = Users.Get + "/projects/{projectName}";

            public const string GetByUser = Users.Get + "/projects/{projectName}";

            public const string CreateByUser = Users.Get + "/projects";

            public const string DeleteByUser = Users.Get + "/projects/{projectName}";

            public const string GetAll = Base + "/projects-search";
        }

        public static class Issues
        {
            public const string GetAllByOrg = Projects.GetByOrg + "/issues";

            public const string UpdateByOrg = Projects.GetByOrg + "/issues/{issueNumber}";

            public const string GetByOrg = Projects.GetByOrg + "/issues/{issueNumber}";

            public const string CreateByOrg = Projects.GetByOrg + "/issues";

            public const string DeleteByOrg = Projects.GetByOrg + "/issues/{issueNumber}";

            public const string GetAllByUser = Projects.GetByUser + "/issues";

            public const string UpdateByUser = Projects.GetByUser + "/issues/{issueNumber}";

            public const string GetByUser = Projects.GetByUser + "/issues/{issueNumber}";

            public const string CreateByUser = Projects.GetByUser + "/issues";

            public const string DeleteByUser = Projects.GetByUser + "/issues/{issueNumber}";
        }

        public static class IssueStages
        {
            public const string GetAllByOrg = Projects.GetByOrg + "/issue-stages";

            public const string UpdateByOrg = Projects.GetByOrg + "/issue-stages/{Id}";

            public const string GetByOrg = Projects.GetByOrg + "/issue-stages/{Id}";

            public const string CreateByOrg = Projects.GetByOrg + "/issue-stages";

            public const string DeleteByOrg = Projects.GetByOrg + "/issue-stages/{Id}";

            public const string GetAllByUser = Projects.GetByUser + "/issue-stages";

            public const string UpdateByUser = Projects.GetByUser + "/issue-stages/{Id}";

            public const string GetByUser = Projects.GetByUser + "/issue-stages/{Id}";

            public const string CreateByUser = Projects.GetByUser + "/issue-stages";

            public const string DeleteByUser = Projects.GetByUser + "/issue-stages/{Id}";
        }

        public static class IssueTypes
        {
            public const string GetAllByOrg = Projects.GetByOrg + "/issue-types";

            public const string UpdateByOrg = Projects.GetByOrg + "/issue-types/{Id}";

            public const string GetByOrg = Projects.GetByOrg + "/issue-types/{Id}";

            public const string CreateByOrg = Projects.GetByOrg + "/issue-types";

            public const string DeleteByOrg = Projects.GetByOrg + "/issue-types/{Id}";

            public const string GetAllByUser = Projects.GetByUser + "/issue-types";

            public const string UpdateByUser = Projects.GetByUser + "/issue-types/{Id}";

            public const string GetByUser = Projects.GetByUser + "/issue-types/{Id}";

            public const string CreateByUser = Projects.GetByUser + "/issue-types";

            public const string DeleteByUser = Projects.GetByUser + "/issue-types/{Id}";
        }

        public static class IssuePosts
        {
            public const string GetAllByOrg = Issues.GetByOrg + "/issue-posts";

            public const string UpdateByOrg = Issues.GetByOrg + "/issue-posts/{issuePostNumber}";

            public const string GetByOrg = Issues.GetByOrg + "/issue-posts/{issuePostNumber}";

            public const string CreateByOrg = Issues.GetByOrg + "/issue-posts";

            public const string DeleteByOrg = Issues.GetByOrg + "/issue-posts/{issuePostNumber}";

            public const string GetAllByUser = Issues.GetByUser + "/issue-posts";

            public const string UpdateByUser = Issues.GetByUser + "/issue-posts/{issuePostNumber}";

            public const string GetByUser = Issues.GetByUser + "/issue-posts/{issuePostNumber}";

            public const string CreateByUser = Issues.GetByUser + "/issue-posts";

            public const string DeleteByUser = Issues.GetByUser + "/issue-posts/{issuePostNumber}";
        }

        public static class IssuePostReactions
        {
            public const string GetAllByOrg = IssuePosts.GetByOrg + "/issue-posts";

            public const string UpdateByOrg = IssuePosts.GetByOrg + "/issue-posts/{reactionId}";

            public const string GetByOrg = IssuePosts.GetByOrg + "/issue-posts/{reactionId}";

            public const string CreateByOrg = IssuePosts.GetByOrg + "/issue-posts";

            public const string DeleteByOrg = IssuePosts.GetByOrg + "/issue-posts/{reactionId}";

            public const string GetAllByUser = IssuePosts.GetAllByUser + "/issue-posts";

            public const string UpdateByUser = IssuePosts.GetAllByUser + "/issue-posts/{reactionId}";

            public const string GetByUser = IssuePosts.GetAllByUser + "/issue-posts/{reactionId}";

            public const string CreateByUser = IssuePosts.GetAllByUser + "/issue-posts";

            public const string DeleteByUser = IssuePosts.GetAllByUser + "/issue-posts/{reactionId}";
        }

        public static class IssuePostUpdates
        {
            public const string GetAllByOrg = Issues.GetByOrg + "/issue-post-updates";

            public const string UpdateByOrg = Issues.GetByOrg + "/issue-post-updates/{issueUpdateNumber}";

            public const string GetByOrg = Issues.GetByOrg + "/issue-post-updates/{issueUpdateNumber}";

            public const string CreateByOrg = Issues.GetByOrg + "/issue-post-updates";

            public const string DeleteByOrg = Issues.GetByOrg + "/issue-post-updates/{issueUpdateNumber}";

            public const string GetAllByUser = Issues.GetByUser + "/issue-post-updates";

            public const string UpdateByUser = Issues.GetByUser + "/issue-post-updates/{issueUpdateNumber}";

            public const string GetByUser = Issues.GetByUser + "/issue-post-updates/{issueUpdateNumber}";

            public const string CreateByUser = Issues.GetByUser + "/issue-post-updates";

            public const string DeleteByUser = Issues.GetByUser + "/issue-post-updates/{issueUpdateNumber}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Logout = Base + "/identity/logout";

            public const string Register = Base + "/identity/register";
            
            public const string Refresh = Base + "/identity/refresh";
        }

        public static class Notifications
        {
            public const string GetAll = "/notifications";

            public const string Update = Base + "/notifications/{Id}";

            public const string Get = Base + "/notifications/{Id}";

            public const string Create = Base + "/notifications";

            public const string Delete = Base + "/notifications/{Id}";
        }
    }
}