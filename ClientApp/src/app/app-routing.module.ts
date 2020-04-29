import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { ViewMyProjectsComponent } from './view-my-projects/view-my-projects.component';
import { ViewProfileComponent } from './view-profile/view-profile.component';
import { ViewProjectIssuesComponent } from './view-project-issues/view-project-issues.component';
import { CreateOwnProjectComponent } from './create-own-project/create-own-project.component';
import { ViewUserProjectComponent } from './view-user-project/view-user-project.component';
import { ViewOrgProjectComponent } from './view-org-project/view-org-project.component';
import { ViewUserProjectIssuesComponent } from './view-user-project-issues/view-user-project-issues.component';
import { ViewOrgProjectIssuesComponent } from './view-org-project-issues/view-org-project-issues.component';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { NotFoundPageComponent } from './not-found-page/not-found-page.component';
import { LogoutPageComponent } from './logout-page/logout-page.component';
import { CreateIssueComponent } from './create-issue/create-issue.component';
import { ViewOrgProjectIssueComponent } from './view-org-project-issue/view-org-project-issue.component';
import { ViewUserProjectIssueComponent } from './view-user-project-issue/view-user-project-issue.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginPageComponent },
  { path: 'logout', component: LogoutPageComponent },
  { path: 'register', component: RegisterPageComponent },
  { path: 'profile', component: ViewProfileComponent },
  { path: 'settings', component: ViewProfileComponent },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
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
  exports: [
    RouterModule
  ]
})
export class RoutingModule { };
