import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { RoutingModule } from './app-routing.module';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbThemeModule, NbLayoutModule, NbSidebarModule, NbSidebarService, NbActionsModule, NbButtonModule, NbTableModule, NbListModule, NbContextMenuModule, NbMenuService, NbMenuModule, NbUserModule, NbSearchModule, NbBadgeModule, NbIconModule, NbInputModule, NbCheckboxModule, NbCardModule } from '@nebular/theme';
import { NbPasswordAuthStrategy, NbAuthModule, NbOAuth2AuthStrategy } from '@nebular/auth';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { ViewProfileComponent } from './view-profile/view-profile.component';
import { ViewProjectIssuesComponent } from './view-project-issues/view-project-issues.component';
import { NavComponent } from './nav/nav.component';
import { myNbPasswordAuthStrategyOptions, tokenGetter, myRefreshNbPasswordAuthStrategyOptions } from './constants';
import { NbMenuInternalService } from '@nebular/theme/components/menu/menu.service';
import { ViewMyProjectsComponent } from './view-my-projects/view-my-projects.component';
import { ViewUserProjectComponent } from './view-user-project/view-user-project.component';
import { ViewOrgProjectComponent } from './view-org-project/view-org-project.component';
import { CreateOwnProjectComponent } from './create-own-project/create-own-project.component';
import { ViewOrgProjectIssuesComponent } from './view-org-project-issues/view-org-project-issues.component';
import { ViewUserProjectIssuesComponent } from './view-user-project-issues/view-user-project-issues.component';
import { ProjectEditorComponent } from './project-editor/project-editor.component';
import { IssueEditorComponent } from './issue-editor/issue-editor.component';
import { IssuePostEditorComponent } from './issue-post-editor/issue-post-editor.component';
import { ClarityModule } from '@clr/angular';
import { LoginPageComponent } from './login-page/login-page.component';
import { RegisterPageComponent } from './register-page/register-page.component';
import { NotFoundPageComponent } from './not-found-page/not-found-page.component';
import { LogoutPageComponent } from './logout-page/logout-page.component';
import { CreateIssueComponent } from './create-issue/create-issue.component';
import { CreateIssuePostComponent } from './create-issue-post/create-issue-post.component';
import { ViewUserProjectIssueComponent } from './view-user-project-issue/view-user-project-issue.component';
import { ViewOrgProjectIssueComponent } from './view-org-project-issue/view-org-project-issue.component';

@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        CounterComponent,
        FetchDataComponent,
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
        ViewOrgProjectIssueComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        RoutingModule,
        BrowserAnimationsModule,
        // NbThemeModule.forRoot({ name: 'dragonfly' }),
        NbAuthModule.forRoot({
          strategies: [
            NbPasswordAuthStrategy.setup(myRefreshNbPasswordAuthStrategyOptions),
          ],
          forms: {},
        }),
      JwtModule.forRoot({
        config: {
          tokenGetter: tokenGetter,
          whitelistedDomains: ['localhost:5001', 'localhost:4200', 'localhost:3000', 'localhost', 'knowledgebase.network', 'giorgosnikolopoulos.ddns.net',
            'giorgosnikolopoulos.ddns.net:4200', 'giorgosnikolopoulos.ddns.net:3000'],
          blacklistedRoutes: [],
          // authScheme: ""
        },
      }),
      ClarityModule,
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
