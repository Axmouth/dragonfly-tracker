import { apiRoot } from '../../environments/environment';

const Base = apiRoot;

const Organizations = {
  GetAll: Base + '/organizations',
  Update: Base + '/organizations/{organizationName}',
  Get: Base + '/organizations/{organizationName}',
  Create: Base + '/organizations',
  Delete: Base + '/organizations/{organizationName}',
};

const Users = {
  GetAll: Base + '/users',
  Update: Base + '/users/{username}',
  Get: Base + '/users/{username}',
  Create: Base + '/users',
  Delete: Base + '/users/{username}',
};

const Projects = {
  GetAllByOrg: Organizations.Get + '/projects',
  UpdateByOrg: Organizations.Get + '/projects/{projectName}',
  GetByOrg: Organizations.Get + '/projects/{projectName}',
  CreateByOrg: Organizations.Get + '/projects',
  DeleteByOrg: Organizations.Get + '/projects/{projectName}',

  GetAllByUser: Users.Get + '/projects',
  UpdateByUser: Users.Get + '/projects/{projectName}',
  GetByUser: Users.Get + '/projects/{projectName}',
  CreateByUser: Users.Get + '/projects',
  DeleteByUser: Users.Get + '/projects/{projectName}',
  GetAll: Base + '/projects-search',
};

const Issues = {
  GetAllByOrgProject: Projects.GetByOrg + '/issues',
  UpdateByOrgProject: Projects.GetByOrg + '/issues/{issueNumber}',
  GetByOrgProject: Projects.GetByOrg + '/issues/{issueNumber}',
  CreateByOrgProject: Projects.GetByOrg + '/issues',
  DeleteByOrgProject: Projects.GetByOrg + '/issues/{issueNumber}',

  GetAllByUserProject: Projects.GetByUser + '/issues',
  UpdateByUserProject: Projects.GetByUser + '/issues/{issueNumber}',
  GetByUserProject: Projects.GetByUser + '/issues/{issueNumber}',
  CreateByUserProject: Projects.GetByUser + '/issues',
  DeleteByUserProject: Projects.GetByUser + '/issues/{issueNumber}',
};

const IssueStages = {
  GetAllByOrgProject: Projects.GetByOrg + '/issue-stages',
  UpdateByOrgProject: Projects.GetByOrg + '/issue-stages/{issueStageName}',
  GetByOrgProject: Projects.GetByOrg + '/issue-stages/{issueStageName}',
  CreateByOrgProject: Projects.GetByOrg + '/issue-stages',
  DeleteByOrgProject: Projects.GetByOrg + '/issue-stages/{issueStageName}',

  GetAllByUserProject: Projects.GetByUser + '/issue-stages',
  UpdateByUserProject: Projects.GetByUser + '/issue-stages/{issueStageName}',
  GetByUserProject: Projects.GetByUser + '/issue-stages/{issueStageName}',
  CreateByUserProject: Projects.GetByUser + '/issue-stages',
  DeleteByUserProject: Projects.GetByUser + '/issue-stages/{issueStageName}',

  GetByOrgProjectIssue: Issues.GetByOrgProject + '/issue-stage',
  GetByUserProjectIssue: Issues.GetByUserProject + '/issue-stage',
  CreateByOrgProjectIssue: Issues.GetByOrgProject + '/issue-stage',
  CreateByUserProjectIssue: Issues.GetByUserProject + '/issue-stage',
};

const IssueTypes = {
  GetAllByOrgProject: Projects.GetByOrg + '/issue-types',
  UpdateByOrgProject: Projects.GetByOrg + '/issue-types/{issueTypeName}',
  GetByOrgProject: Projects.GetByOrg + '/issue-types/{issueTypeName}',
  CreateByOrgProject: Projects.GetByOrg + '/issue-types',
  DeleteByOrgProject: Projects.GetByOrg + '/issue-types/{issueTypeName}',

  GetAllByUserProject: Projects.GetByUser + '/issue-types',
  UpdateByUserProject: Projects.GetByUser + '/issue-types/{issueTypeName}',
  GetByUserProject: Projects.GetByUser + '/issue-types/{issueTypeName}',
  CreateByUserProject: Projects.GetByUser + '/issue-types',
  DeleteByUserProject: Projects.GetByUser + '/issue-types/{issueTypeName}',

  GetAllByOrgProjectIssue: Issues.GetByOrgProject + '/issue-types',
  UpdateByOrgProjectIssue: Issues.GetByOrgProject + '/issue-types/{issueTypeName}',
  GetByOrgProjectIssue: Issues.GetByOrgProject + '/issue-types/{issueTypeName}',
  CreateByOrgProjectIssue: Issues.GetByOrgProject + '/issue-types',
  DeleteByOrgProjectIssue: Issues.GetByOrgProject + '/issue-types/{issueTypeName}',

  GetAllByUserProjectIssue: Issues.GetByUserProject + '/issue-types',
  UpdateByUserProjectIssue: Issues.GetByUserProject + '/issue-types/{issueTypeName}',
  GetByUserProjectIssue: Issues.GetByUserProject + '/issue-types/{issueTypeName}',
  CreateByUserProjectIssue: Issues.GetByUserProject + '/issue-types',
  DeleteByUserProjectIssue: Issues.GetByUserProject + '/issue-types/{issueTypeName}',
};

const IssuePosts = {
  GetAllByOrgProjectIssue: Issues.GetByOrgProject + '/issue-posts',
  UpdateByOrgProjectIssue: Issues.GetByOrgProject + '/issue-posts/{issuePostNumber}',
  GetByOrgProjectIssue: Issues.GetByOrgProject + '/issue-posts/{issuePostNumber}',
  CreateByOrgProjectIssue: Issues.GetByOrgProject + '/issue-posts',
  DeleteByOrgProjectIssue: Issues.GetByOrgProject + '/issue-posts/{issuePostNumber}',

  GetAllByUserProjectIssue: Issues.GetByUserProject + '/issue-posts',
  UpdateByUserProjectIssue: Issues.GetByUserProject + '/issue-posts/{issuePostNumber}',
  GetByUserProjectIssue: Issues.GetByUserProject + '/issue-posts/{issuePostNumber}',
  CreateByUserProjectIssue: Issues.GetByUserProject + '/issue-posts',
  DeleteByUserProjectIssue: Issues.GetByUserProject + '/issue-posts/{issuePostNumber}',

  GetAllByUser: Users.Get + '/issue-posts',
};

const IssuePostReactions = {
  GetAllByOrg: IssuePosts.GetByOrgProjectIssue + '/issue-posts',
  UpdateByOrg: IssuePosts.GetByOrgProjectIssue + '/issue-posts/{reactionId}',
  GetByOrg: IssuePosts.GetByOrgProjectIssue + '/issue-posts/{reactionId}',
  CreateByOrg: IssuePosts.GetByOrgProjectIssue + '/issue-posts',
  DeleteByOrg: IssuePosts.GetByOrgProjectIssue + '/issue-posts/{reactionId}',

  GetAllByUser: IssuePosts.GetAllByUserProjectIssue + '/issue-posts',
  UpdateByUser: IssuePosts.GetAllByUserProjectIssue + '/issue-posts/{reactionId}',
  GetByUser: IssuePosts.GetAllByUserProjectIssue + '/issue-posts/{reactionId}',
  CreateByUser: IssuePosts.GetAllByUserProjectIssue + '/issue-posts',
  DeleteByUser: IssuePosts.GetAllByUserProjectIssue + '/issue-posts/{reactionId}',
};

const IssuePostUpdates = {
  GetAllByOrg: Issues.GetByOrgProject + '/issue-post-updates',
  UpdateByOrg: Issues.GetByOrgProject + '/issue-post-updates/{issueUpdateNumber}',
  GetByOrg: Issues.GetByOrgProject + '/issue-post-updates/{issueUpdateNumber}',
  CreateByOrg: Issues.GetByOrgProject + '/issue-post-updates',
  DeleteByOrg: Issues.GetByOrgProject + '/issue-post-updates/{issueUpdateNumber}',

  GetAllByUser: Issues.GetByUserProject + '/issue-post-updates',
  UpdateByUser: Issues.GetByUserProject + '/issue-post-updates/{issueUpdateNumber}',
  GetByUser: Issues.GetByUserProject + '/issue-post-updates/{issueUpdateNumber}',
  CreateByUser: Issues.GetByUserProject + '/issue-post-updates',
  DeleteByUser: Issues.GetByUserProject + '/issue-post-updates/{issueUpdateNumber}',
};

const Identity = {
  Login: Base + '/identity/login',
  Logout: Base + '/identity/logout',
  Register: Base + '/identity/register',
  Refresh: Base + '/identity/refresh',
  Profile: Base + '/identity/profile',
  PasswordReset: Base + '/identity/password-reset',
  PasswordResetEmail: Base + '/identity/password-reset-email',
  PasswordChange: Base + '/identity/password-change',
  EmailConfirm: Base + '/identity/email-confirm',
  EmailConfirmEmail: Base + '/identity/email-confirm-email',
};

const Notifications = {
  GetAll: Base + '/notifications',
  Update: Base + '/notifications/{Id}',
  Get: Base + '/notifications/{Id}',
  Create: Base + '/notifications',
  Delete: Base + '/notifications/{Id}',
};

const AntiForgery = {
  Get: Base + '/antiforgery',
};

const ProjectAdmins = {
  GetByUserProject: Projects.GetByUser + '/admins/{adminUserName}',
  GetAllByUserProject: Projects.GetByUser + '/admins',
  CreateByUserProject: Projects.GetByUser + '/admins',
  DeleteByUserProject: Projects.GetByUser + '/admins/{adminUserName}',

  GetByOrgProject: Projects.GetByOrg + '/admins/{adminUserName}',
  GetAllByOrgProject: Projects.GetByOrg + '/admins',
  CreateByOrgProject: Projects.GetByOrg + '/admins',
  DeleteByOrgProject: Projects.GetByOrg + '/admins/{adminUserName}',
};

const ProjectMaintainers = {
  GetByUserProject: Projects.GetByUser + '/maintainers/{adminUserName}',
  GetAllByUserProject: Projects.GetByUser + '/maintainers',
  CreateByUserProject: Projects.GetByUser + '/maintainers',
  DeleteByUserProject: Projects.GetByUser + '/maintainers/{adminUserName}',

  GetByOrgProject: Projects.GetByOrg + '/maintainers/{maintainerUserName}',
  GetAllByOrgProject: Projects.GetByOrg + '/maintainers',
  CreateByOrgProject: Projects.GetByOrg + '/maintainers',
  DeleteByOrgProject: Projects.GetByOrg + '/maintainers/{maintainerUserName}',
};

const ProjectOwners = {
  GetByUserProject: Projects.GetByUser + '/owner',
  CreateByUserProject: Projects.GetByUser + '/owner',

  GetByOrgProject: Projects.GetByOrg + '/owner',
  CreateByOrgProject: Projects.GetByOrg + '/owner',
};

export const ApiRoutesV1 = {
  Base,
  Users,
  Organizations,
  Projects,
  Issues,
  IssueStages,
  IssueTypes,
  IssuePosts,
  IssuePostReactions,
  IssuePostUpdates,
  Identity,
  Notifications,
  AntiForgery,
  ProjectAdmins,
  ProjectMaintainers,
  ProjectOwners,
};
