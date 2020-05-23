import { Component, OnInit, OnDestroy, PLATFORM_ID, Inject } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Subscription, pipe, Subject, Observable } from 'rxjs';
import { ClrDatagridStateInterface } from '@clr/angular';
import { takeUntil, first } from 'rxjs/operators';
import { AuthService } from '../../auth/services/auth.service';
import { TokenService } from '../../auth/services/token.service';
import { Project } from 'src/app/models/api/project';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { PagedResponse } from '../../models/Api/paged-response';
import { isPlatformBrowser } from '@angular/common';

const DEFAULT_TAB = 'owned';

@Component({
  selector: 'app-view-my-projects',
  templateUrl: './view-my-projects.component.html',
  styleUrls: ['./view-my-projects.component.scss'],
})
export class ViewMyProjectsComponent implements OnInit, OnDestroy {
  projectsList: any[] = [];
  total: number;
  loading = true;
  username: string;
  ngUnsubscribe = new Subject<void>();
  state: ClrDatagridStateInterface;
  currentPage: number;
  myOwnProjectsActive = true;
  myAdminedProjectsActive = false;
  myMaintainedProjectsActive = false;
  tab = DEFAULT_TAB;
  firstLoad = true;
  showDeleteProjectDialog = false;
  targetProject: Project;

  constructor(
    private projectService: ProjectsService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    @Inject(PLATFORM_ID) private platform: Object,
  ) {}

  async ngOnInit() {
    if (!isPlatformBrowser(this.platform)) {
      return;
    }
    this.authService
      .getUsername()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((newUsername) => {
        this.username = newUsername;
      });
    this.activatedRoute.queryParams.pipe(takeUntil(this.ngUnsubscribe)).subscribe((qParams) => {
      if (qParams.page !== undefined && qParams.page !== null) {
        this.currentPage = +qParams.page;
      }
      this.setTab(qParams.tab);
    });
  }

  setTab(tabName: string) {
    if (tabName !== undefined && tabName !== null) {
      this.tab = tabName;
      switch (this.tab) {
        case 'owned':
          this.myOwnProjectsActive = true;
          this.myAdminedProjectsActive = false;
          this.myMaintainedProjectsActive = false;
          break;
        case 'admined':
          this.myOwnProjectsActive = false;
          this.myAdminedProjectsActive = true;
          this.myMaintainedProjectsActive = false;
          break;
        case 'maintained':
          this.myOwnProjectsActive = false;
          this.myAdminedProjectsActive = false;
          this.myMaintainedProjectsActive = true;
          break;

        default:
          this.myOwnProjectsActive = true;
          this.myAdminedProjectsActive = false;
          this.myMaintainedProjectsActive = false;
          break;
      }
    }
  }

  onProjectEditClick(project: Project) {
    console.log(project);
  }

  onTabChange(tabName: string) {
    if (!this.firstLoad) {
      // this.$projectSubscription.unsubscribe();
    }
    const queryParams: Params = {};
    if (this.tab !== tabName) {
      queryParams.page = 1;
      queryParams.tab = tabName;
      this.tab = tabName;
    }
    this.firstLoad = true;
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: queryParams,
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
  }

  onProjectDeleteClick(project: Project) {
    this.targetProject = project;
    this.showDeleteProjectDialog = true;
  }

  deleteProject() {
    this.projectService
      .deleteUsersProject(this.targetProject.creator.userName, this.targetProject.name)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        this.refresh(this.state);
      });
    this.showDeleteProjectDialog = false;
  }

  async refresh(state: ClrDatagridStateInterface) {
    if (!this.firstLoad) {
      // this.$projectSubscription.unsubscribe();
    }
    this.state = state;
    if (!this.firstLoad) {
      const queryParams: Params = { page: state.page.current };
      this.router.navigate([], {
        relativeTo: this.activatedRoute,
        queryParams: queryParams,
        queryParamsHandling: 'merge', // remove to replace all query params by provided
      });
    } else {
      this.firstLoad = false;
    }
    this.loading = true;

    // We convert the filters from an array to a map,
    // because that's what our backend-calling service is expecting
    const filters: { [prop: string]: any[] } = {};
    if (state.filters) {
      for (const filter of state.filters) {
        const { property, value } = <{ property: string; value: string }>filter;
        filters[property] = [value];
      }
    } /*
    this.inventory.filter(filters)
      .sort(<{ by: string, reverse: boolean }>state.sort)
      .fetch(state.page.from, state.page.size)
      .then((result: FetchResult) => {
        this.users = result.users;
        this.total = result.length;
        this.loading = false;
      });*/

    let $projects: Observable<PagedResponse<Project>>;
    if (this.myOwnProjectsActive) {
      $projects = this.projectService
        .getUsersProjects(this.username, state.page.current, state.page.size)
        .pipe(takeUntil(this.ngUnsubscribe));
    } else if (this.myAdminedProjectsActive) {
      $projects = this.projectService
        .getAllProjects(state.page.current, state.page.size, '', true, false)
        .pipe(takeUntil(this.ngUnsubscribe));
    } else if (this.myMaintainedProjectsActive) {
      $projects = this.projectService
        .getAllProjects(state.page.current, state.page.size, '', false, true)
        .pipe(takeUntil(this.ngUnsubscribe));
    }
    $projects.subscribe(async (projectsResult) => {
      this.projectsList = projectsResult['data'];
      this.total = projectsResult['total'];
      this.loading = false;
    });
  }

  async onNewProjectSubmit(project: Project) {
    this.projectService
      .createUsersProject(this.username, project)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        this.router.navigateByUrl(`/user/${this.username}/${result.data.name}`);
      });
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
