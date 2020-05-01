import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ViewMyProjectsComponent } from './components/view-my-projects/view-my-projects.component';
import { ViewProfileComponent } from './components/view-profile/view-profile.component';
import { ViewProjectIssuesComponent } from './components/view-project-issues/view-project-issues.component';
import { CreateOwnProjectComponent } from './components/create-own-project/create-own-project.component';
import { ViewUserProjectComponent } from './components/view-user-project/view-user-project.component';
import { ViewOrgProjectComponent } from './components/view-org-project/view-org-project.component';
import { ViewUserProjectIssuesComponent } from './components/view-user-project-issues/view-user-project-issues.component';
import { ViewOrgProjectIssuesComponent } from './components/view-org-project-issues/view-org-project-issues.component';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { LogoutPageComponent } from './components/logout-page/logout-page.component';
import { CreateIssueComponent } from './components/create-issue/create-issue.component';
import { ViewOrgProjectIssueComponent } from './components/view-org-project-issue/view-org-project-issue.component';
import { ViewUserProjectIssueComponent } from './components/view-user-project-issue/view-user-project-issue.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginPageComponent },
  { path: 'logout', component: LogoutPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'profile', component: ViewProfileComponent },
  { path: 'settings', component: ViewProfileComponent },
  { path: 'my-projects', component: ViewMyProjectsComponent },
  { path: 'my-projects/create', component: CreateOwnProjectComponent },
  { path: 'user/:username', component: ViewProfileComponent },
  { path: 'user/:username/:projectname', component: ViewUserProjectComponent },
  { path: 'org/:orgname/:projectname', component: ViewOrgProjectComponent },
  { path: 'user/:username/:projectname?tab=issues', component: ViewUserProjectComponent },
  { path: 'org/:orgname/:projectname?tab=issues', component: ViewOrgProjectComponent },
  { path: 'user/:username/:projectname/create-issue', component: CreateIssueComponent },
  { path: 'org/:orgname/:projectname/create-issue', component: CreateIssueComponent },
  { path: 'user/:username/:projectname/issues/:issueNumber', component: ViewUserProjectIssueComponent },
  { path: 'org/:orgname/:projectname/issues/:issueNumber', component: ViewOrgProjectIssueComponent },
  { path: '**', component: NotFoundPageComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class RoutingModule {}
