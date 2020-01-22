import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { RoutingModule } from './routing.module';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
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
import { myNbPasswordAuthStrategyOptions } from './constants';
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

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
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
        IssuePostEditorComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
      FormsModule, /*
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
        ]),*/
        RoutingModule,
        BrowserAnimationsModule,
        NbThemeModule.forRoot({ name: 'dragonfly' }),
        NbLayoutModule,
        NbEvaIconsModule,
        NbSidebarModule,
        NbActionsModule,
        NbButtonModule,
        NbTableModule,
        NbListModule,
        NbContextMenuModule,
        NbUserModule,
        NbSearchModule,
        NbBadgeModule,
        NbButtonModule,
        NbIconModule,
        NbInputModule,
      NbCheckboxModule,
      NbCardModule,
        NbMenuModule.forRoot(),
        NbAuthModule.forRoot({
          strategies: [
            NbPasswordAuthStrategy.setup(myNbPasswordAuthStrategyOptions),
          ],
          forms: {},
        }), 
    ],
    providers: [NbSidebarService],
    bootstrap: [AppComponent]
})
export class AppModule { }
