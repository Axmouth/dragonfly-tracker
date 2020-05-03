import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Subscription, pipe, Subject } from 'rxjs';
import { ClrDatagridStateInterface } from '@clr/angular';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { Project } from 'src/app/models/api/project';
import { ActivatedRoute, Router, Params } from '@angular/router';

@Component({
  selector: 'app-view-my-projects',
  templateUrl: './view-my-projects.component.html',
  styleUrls: ['./view-my-projects.component.scss'],
})
export class ViewMyProjectsComponent implements OnInit, OnDestroy {
  projectsList: any[] = [];
  projectSubscription$: Subscription;
  total: number;
  loading = true;
  username: string;
  ngUnsubscribe = new Subject<void>();
  state: ClrDatagridStateInterface;
  currentPage: number;

  constructor(
    private projectsService: ProjectsService,
    private authService: AuthService,
    private tokenService: TokenService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
  ) {}

  async ngOnInit() {
    // this.username = (await this.authService.getToken().toPromise()).getName();
    this.username = (await (await this.tokenService.get().toPromise()).getPayload()).sub;
    const qParams = this.activatedRoute.snapshot.queryParams;
    if (qParams.page !== undefined && qParams.page !== null) {
      this.currentPage = +qParams.page;
    }
  }

  onProjectEditClick(project: Project) {
    console.log(project);
  }

  onProjectDeleteClick(project: Project) {
    this.projectsService
      .deleteUsersProject(project.creator.username, project.name)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        this.refresh(this.state);
      });
  }

  async refresh(state: ClrDatagridStateInterface) {
    this.username = await (await this.tokenService.get().toPromise()).getPayload().sub;
    this.state = state;
    const queryParams: Params = { page: state.page.current };
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: queryParams,
      queryParamsHandling: 'merge', // remove to replace all query params by provided
    });
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

    this.projectSubscription$ = this.projectsService
      .getUsersProjects(this.username, state.page.current, state.page.size)
      .subscribe(async (projectsResult) => {
        this.projectsList = projectsResult['data'];
        this.total = projectsResult['total'];
        this.loading = false;
      });
  }

  ngOnDestroy() {}
}
