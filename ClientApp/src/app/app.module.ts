import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RoutingModule } from './app-routing.module';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ViewProfileComponent } from './components/view-profile/view-profile.component';
import { ViewProjectIssuesComponent } from './components/view-project-issues/view-project-issues.component';
import { NavComponent } from './components/nav/nav.component';
import { ViewMyProjectsComponent } from './components/view-my-projects/view-my-projects.component';
import { ViewUserProjectComponent } from './components/view-user-project/view-user-project.component';
import { ViewOrgProjectComponent } from './components/view-org-project/view-org-project.component';
import { CreateOwnProjectComponent } from './components/create-own-project/create-own-project.component';
import { ViewOrgProjectIssuesComponent } from './components/view-org-project-issues/view-org-project-issues.component';
import { ViewUserProjectIssuesComponent } from './components/view-user-project-issues/view-user-project-issues.component';
import { ProjectEditorComponent } from './components/project-editor/project-editor.component';
import { IssueEditorComponent } from './components/issue-editor/issue-editor.component';
import { IssuePostEditorComponent } from './components/issue-post-editor/issue-post-editor.component';
import { ClarityModule } from '@clr/angular';
import { LoginPageComponent } from './components/login-page/login-page.component';
import { RegisterPageComponent } from './components/register-page/register-page.component';
import { NotFoundPageComponent } from './components/not-found-page/not-found-page.component';
import { LogoutPageComponent } from './components/logout-page/logout-page.component';
import { CreateIssueComponent } from './components/create-issue/create-issue.component';
import { CreateIssuePostComponent } from './components/create-issue-post/create-issue-post.component';
import { ViewUserProjectIssueComponent } from './components/view-user-project-issue/view-user-project-issue.component';
import { ViewOrgProjectIssueComponent } from './components/view-org-project-issue/view-org-project-issue.component';
import { tokenGetter } from './constants';
import { UserPreferencesComponent } from './components/user-preferences/user-preferences.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ViewProfileComponent,
    ViewProjectIssuesComponent,
    NavComponent,
    ViewMyProjectsComponent,
    ViewUserProjectComponent,
    ViewOrgProjectComponent,
    CreateOwnProjectComponent,
    ViewOrgProjectIssuesComponent,
    ViewUserProjectIssuesComponent,
    ProjectEditorComponent,
    IssueEditorComponent,
    IssuePostEditorComponent,
    LoginPageComponent,
    RegisterPageComponent,
    NotFoundPageComponent,
    LogoutPageComponent,
    CreateIssueComponent,
    CreateIssuePostComponent,
    ViewUserProjectIssueComponent,
    ViewOrgProjectIssueComponent,
    UserPreferencesComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RoutingModule,
    BrowserAnimationsModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: [
          'localhost:5001',
          'localhost:4200',
          'localhost:3000',
          'localhost',
          'knowledgebase.network',
          'giorgosnikolopoulos.ddns.net',
          'giorgosnikolopoulos.ddns.net:4200',
          'giorgosnikolopoulos.ddns.net:3000',
        ],
        blacklistedRoutes: [],
        // authScheme: ""
      },
    }),
    ClarityModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
