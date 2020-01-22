import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NbAuthComponent, NbLoginComponent, NbLogoutComponent, NbRegisterComponent, NbRequestPasswordComponent, NbResetPasswordComponent } from '@nebular/auth';
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

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'profile', component: ViewProfileComponent },
  { path: 'settings', component: ViewProfileComponent },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'my-projects', component: ViewMyProjectsComponent },
  { path: 'my-projects/create', component: CreateOwnProjectComponent },
  { path: 'user/:username', component: ViewProfileComponent },
  { path: 'user/:username/:projectname', component: ViewUserProjectComponent },
  { path: 'org/:orgname/:projectname', component: ViewOrgProjectComponent },
  { path: 'user/:username/:projectname/issues', component: ViewUserProjectIssuesComponent },
  { path: 'org/:orgname/:projectname/issues', component: ViewOrgProjectIssuesComponent },
  {
    path: 'auth',
    component: NbAuthComponent,
    children: [
      {
        path: '',
        component: NbLoginComponent,
      },
      {
        path: 'login',
        component: NbLoginComponent,
      },
      {
        path: 'register',
        component: NbRegisterComponent,
      },
      {
        path: 'logout',
        component: NbLogoutComponent,
      },
      {
        path: 'request-password',
        component: NbRequestPasswordComponent,
      },
      {
        path: 'reset-password',
        component: NbResetPasswordComponent,
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [
    RouterModule
  ]
})
export class RoutingModule { };
