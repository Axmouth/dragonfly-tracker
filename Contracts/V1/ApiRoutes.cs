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
            public const string GetAllByOrgProject = Projects.GetByOrg + "/issues";

            public const string UpdateByOrgProject = Projects.GetByOrg + "/issues/{issueNumber}";

            public const string GetByOrgProject = Projects.GetByOrg + "/issues/{issueNumber}";

            public const string CreateByOrgProject = Projects.GetByOrg + "/issues";

            public const string DeleteByOrgProject = Projects.GetByOrg + "/issues/{issueNumber}";

            public const string GetAllByUserProject = Projects.GetByUser + "/issues";

            public const string UpdateByUserProject = Projects.GetByUser + "/issues/{issueNumber}";

            public const string GetByUserProject = Projects.GetByUser + "/issues/{issueNumber}";

            public const string CreateByUserProject = Projects.GetByUser + "/issues";

            public const string DeleteByUserProject = Projects.GetByUser + "/issues/{issueNumber}";
        }

        public static class IssueStages
        {
            public const string GetAllByOrg = Projects.GetByOrg + "/issue-stages";

            public const string UpdateByOrg = Projects.GetByOrg + "/issue-stages/{issueStageName}";

            public const string GetByOrg = Projects.GetByOrg + "/issue-stages/{issueStageName}";

            public const string CreateByOrg = Projects.GetByOrg + "/issue-stages";

            public const string DeleteByOrg = Projects.GetByOrg + "/issue-stages/{issueStageName}";

            public const string GetAllByUser = Projects.GetByUser + "/issue-stages";

            public const string UpdateByUser = Projects.GetByUser + "/issue-stages/{issueStageName}";

            public const string GetByUser = Projects.GetByUser + "/issue-stages/{issueStageName}";

            public const string CreateByUser = Projects.GetByUser + "/issue-stages";

            public const string DeleteByUser = Projects.GetByUser + "/issue-stages/{issueStageName}";


            public const string CreateByOrgProjectIssue = Issues.GetByOrgProject + "/issue-stage";

            public const string CreateByUserProjectIssue = Issues.GetByUserProject + "/issue-stage";
        }

        public static class IssueTypes
        {
            public const string GetAllByOrg = Projects.GetByOrg + "/issue-types";

            public const string UpdateByOrg = Projects.GetByOrg + "/issue-types/{issueTypeName}";

            public const string GetByOrg = Projects.GetByOrg + "/issue-types/{issueTypeName}";

            public const string CreateByOrg = Projects.GetByOrg + "/issue-types";

            public const string DeleteByOrg = Projects.GetByOrg + "/issue-types/{issueTypeName}";

            public const string GetAllByUser = Projects.GetByUser + "/issue-types";

            public const string UpdateByUser = Projects.GetByUser + "/issue-types/{issueTypeName}";

            public const string GetByUser = Projects.GetByUser + "/issue-types/{issueTypeName}";

            public const string CreateByUser = Projects.GetByUser + "/issue-types";

            public const string DeleteByUser = Projects.GetByUser + "/issue-types/{issueTypeName}";

            public const string GetAllByOrgProjectIssue = Issues.GetByOrgProject + "/issue-types";

            public const string UpdateByOrgProjectIssue = Issues.GetByOrgProject + "/issue-types/{issueTypeName}";

            public const string GetByOrgProjectIssue = Issues.GetByOrgProject + "/issue-types/{issueTypeName}";

            public const string CreateByOrgProjectIssue = Issues.GetByOrgProject + "/issue-types";

            public const string DeleteByOrgProjectIssue = Issues.GetByOrgProject + "/issue-types/{issueTypeName}";

            public const string GetAllByUserProjectIssue = Issues.GetByUserProject + "/issue-types";

            public const string UpdateByUserProjectIssue = Issues.GetByUserProject + "/issue-types/{issueTypeName}";

            public const string GetByUserProjectIssue = Issues.GetByUserProject + "/issue-types/{issueTypeName}";

            public const string CreateByUserProjectIssue = Issues.GetByUserProject + "/issue-types";

            public const string DeleteByUserProjectIssue = Issues.GetByUserProject + "/issue-types/{issueTypeName}";
        }

        public static class IssuePosts
        {
            public const string GetAllByOrgProjectIssue = Issues.GetByOrgProject + "/issue-posts";

            public const string UpdateByOrgProjectIssue = Issues.GetByOrgProject + "/issue-posts/{issuePostNumber}";

            public const string GetByOrgProjectIssue = Issues.GetByOrgProject + "/issue-posts/{issuePostNumber}";

            public const string CreateByOrgProjectIssue = Issues.GetByOrgProject + "/issue-posts";

            public const string DeleteByOrgProjectIssue = Issues.GetByOrgProject + "/issue-posts/{issuePostNumber}";

            public const string GetAllByUserProjectIssue = Issues.GetByUserProject + "/issue-posts";

            public const string UpdateByUserProjectIssue = Issues.GetByUserProject + "/issue-posts/{issuePostNumber}";

            public const string GetByUserProjectIssue = Issues.GetByUserProject + "/issue-posts/{issuePostNumber}";

            public const string CreateByUserProjectIssue = Issues.GetByUserProject + "/issue-posts";

            public const string DeleteByUserProjectIssue = Issues.GetByUserProject + "/issue-posts/{issuePostNumber}";

            public const string GetAllByUser = Users.Get + "/issue-posts";
        }

        public static class IssuePostReactions
        {
            public const string GetAllByOrg = IssuePosts.GetByOrgProjectIssue + "/issue-posts";

            public const string UpdateByOrg = IssuePosts.GetByOrgProjectIssue + "/issue-posts/{reactionId}";

            public const string GetByOrg = IssuePosts.GetByOrgProjectIssue + "/issue-posts/{reactionId}";

            public const string CreateByOrg = IssuePosts.GetByOrgProjectIssue + "/issue-posts";

            public const string DeleteByOrg = IssuePosts.GetByOrgProjectIssue + "/issue-posts/{reactionId}";

            public const string GetAllByUser = IssuePosts.GetAllByUserProjectIssue + "/issue-posts";

            public const string UpdateByUser = IssuePosts.GetAllByUserProjectIssue + "/issue-posts/{reactionId}";

            public const string GetByUser = IssuePosts.GetAllByUserProjectIssue + "/issue-posts/{reactionId}";

            public const string CreateByUser = IssuePosts.GetAllByUserProjectIssue + "/issue-posts";

            public const string DeleteByUser = IssuePosts.GetAllByUserProjectIssue + "/issue-posts/{reactionId}";
        }

        public static class IssuePostUpdates
        {
            public const string GetAllByOrg = Issues.GetByOrgProject + "/issue-post-updates";

            public const string UpdateByOrg = Issues.GetByOrgProject + "/issue-post-updates/{issueUpdateNumber}";

            public const string GetByOrg = Issues.GetByOrgProject + "/issue-post-updates/{issueUpdateNumber}";

            public const string CreateByOrg = Issues.GetByOrgProject + "/issue-post-updates";

            public const string DeleteByOrg = Issues.GetByOrgProject + "/issue-post-updates/{issueUpdateNumber}";

            public const string GetAllByUser = Issues.GetByUserProject + "/issue-post-updates";

            public const string UpdateByUser = Issues.GetByUserProject + "/issue-post-updates/{issueUpdateNumber}";

            public const string GetByUser = Issues.GetByUserProject + "/issue-post-updates/{issueUpdateNumber}";

            public const string CreateByUser = Issues.GetByUserProject + "/issue-post-updates";

            public const string DeleteByUser = Issues.GetByUserProject + "/issue-post-updates/{issueUpdateNumber}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Logout = Base + "/identity/logout";

            public const string Register = Base + "/identity/register";
            
            public const string Refresh = Base + "/identity/refresh";

            public const string Profile = Base + "/identity/profile";

            public const string PasswordReset = Base + "/identity/password-reset";

            public const string PasswordResetEmail = Base + "/identity/password-reset-email";

            public const string PasswordChange = Base + "/identity/password-change";

            public const string EmailConfirm = Base + "/identity/email-confirm";

            public const string EmailConfirmEmail = Base + "/identity/email-confirm-email";
        }

        public static class Notifications
        {
            public const string GetAll = Base + "/notifications";

            public const string Update = Base + "/notifications/{Id}";

            public const string Get = Base + "/notifications/{Id}";

            public const string Create = Base + "/notifications";

            public const string Delete = Base + "/notifications/{Id}";
        }

        public static class AntiForgery {
            public const string Get = Base + "/antiforgery";
        }

    }
}