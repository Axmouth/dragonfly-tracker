import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription, Subject } from 'rxjs';
import { ClrDatagridStateInterface } from '@clr/angular';
import { takeUntil } from 'rxjs/operators';
import { IssuesService } from '../../services/issues.service';
import { Issue } from 'src/app/models/issue';
import { Project } from 'src/app/models/project';

const openStatusMap = {
  open: true,
  closed: false,
  all: undefined,
};

@Component({
  selector: 'app-view-user-project',
  templateUrl: './view-user-project.component.html',
  styleUrls: ['./view-user-project.component.scss'],
})
export class ViewUserProjectComponent implements OnInit, OnDestroy {
  issuesList: Issue[] = [];
  projectSubscription$: Subscription;
  total: number;
  loadingIssues = true;
  ngUnsubscribe = new Subject<void>();
  project: Project;
  projectSub$: Subscription;
  targetUsername: string;
  targetProjectName: string;
  loading = true;
  notFound = false;
  searchText: string;
  openStatus = 'open';
  state: ClrDatagridStateInterface;

  constructor(
    private projectsService: ProjectsService,
    private route: ActivatedRoute,
    private issuesService: IssuesService,
  ) {}

  async ngOnInit() {
    const params = this.route.snapshot.paramMap;
    this.targetUsername = params.get('username');
    this.targetProjectName = params.get('projectname');
    this.projectSub$ = this.projectsService
      .getUsersProject(this.targetUsername, this.targetProjectName)
      .subscribe((result) => {
        this.project = result.data;
        console.log(this.project);
      });
  }

  ngOnDestroy() {
    this.projectSub$.unsubscribe();
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  onIssueEditClick(project: Issue) {
    console.log(project);
  }

  onIssueDeleteClick(issue: Issue) {
    console.log(issue);
    this.issuesService
      .deleteUsersProjectIssue(this.targetUsername, this.targetProjectName, issue.number)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        console.log(result);
      });
    const index = this.issuesList.indexOf(issue);
    console.log(index);
    this.issuesList = this.issuesList.slice(index, 1);
    // console.log(this.projectsList.filter((x: Project) => x.creator.username !== project.creator.username || x.name !== project.name));
  }

  async onSearchClick() {
    const state = this.state;
    state.page.current = 1;
    this.refresh(state);
  }

  async refresh(state: ClrDatagridStateInterface) {
    this.state = state;
    if (!this.targetProjectName || !this.targetUsername) {
      const params = this.route.snapshot.paramMap;
      this.targetUsername = params.get('username');
      this.targetProjectName = params.get('projectname');
    }
    this.loadingIssues = true;
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

    this.projectSubscription$ = this.issuesService
      .getUsersProjectsIssues(
        this.targetUsername,
        this.targetProjectName,
        state.page.current,
        state.page.size,
        this.searchText,
        openStatusMap[this.openStatus],
      )
      .subscribe(async (issuessResult) => {
        console.log(issuessResult);
        this.issuesList = issuessResult['data'];
        this.total = issuessResult['total'];
        console.log(this.issuesList);
        this.loadingIssues = false;
      });
  }
}
