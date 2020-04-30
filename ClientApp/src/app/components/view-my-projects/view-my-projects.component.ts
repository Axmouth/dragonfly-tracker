import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Subscription, pipe, Subject } from 'rxjs';
import { ClrDatagridStateInterface } from '@clr/angular';
import { takeUntil } from 'rxjs/operators';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { Project } from 'src/app/models/project';

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

  constructor(
    private projectsService: ProjectsService,
    private authService: AuthService,
    private tokenService: TokenService,
  ) {}

  async ngOnInit() {
    this.username = (await (await this.tokenService.get().toPromise()).getPayload()).sub;
    /*
    this.projectSubscription$ = this.projectsService.getUsersProjects(this.username).subscribe(async projectsResult => {
      console.log(projectsResult);
      this.projectsList = projectsResult["data"];
      console.log(this.projectsList);
    });*/
  }

  onProjectEditClick(project: Project) {
    console.log(project);
  }

  onProjectDeleteClick(project: Project) {
    console.log(project);
    this.projectsService
      .deleteUsersProject(project.creator.username, project.name)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        console.log(result);
      });
    const index = this.projectsList.indexOf(project);
    console.log(index);
    this.projectsList = this.projectsList.slice(index, 1);
    // console.log(this.projectsList.filter((x: Project) => x.creator.username !== project.creator.username || x.name !== project.name));
  }

  async refresh(state: ClrDatagridStateInterface) {
    if (!this.username) {
      this.username = (await (await this.tokenService.get().toPromise()).getPayload()).sub;
    }
    this.loading = true;
    console.log(state);

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
        console.log(projectsResult);
        this.projectsList = projectsResult['data'];
        this.total = projectsResult['total'];
        console.log(this.projectsList);
        this.loading = false;
      });
  }

  ngOnDestroy() {}
}
