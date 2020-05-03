import { Component, OnInit } from '@angular/core';
import { Issue } from '../../models/api/issue';
import { IssuePost } from '../../models/api/issue-post';
import { Subscription, Subject } from 'rxjs';
import { ProjectsService } from 'src/app/services/projects.service';
import { ActivatedRoute } from '@angular/router';
import { IssuesService } from 'src/app/services/issues.service';

@Component({
  selector: 'app-view-user-project-issue',
  templateUrl: './view-user-project-issue.component.html',
  styleUrls: ['./view-user-project-issue.component.scss'],
})
export class ViewUserProjectIssueComponent implements OnInit {
  issue = new Issue();
  issuesPostList: IssuePost[] = [];
  issuePostsSub$: Subscription;
  issueSub$: Subscription;
  loadingIssuePosts = true;
  ngUnsubscribe = new Subject<void>();
  targetUsername: string;
  targetProjectName: string;
  targetIssueNumber: number;
  loading = true;
  notFound = false;

  constructor(
    private projectsService: ProjectsService,
    private route: ActivatedRoute,
    private issuesService: IssuesService,
  ) {}

  ngOnInit() {
    const params = this.route.snapshot.paramMap;
    this.targetUsername = params.get('username');
    this.targetProjectName = params.get('projectname');
    this.targetIssueNumber = +params.get('issueNumber');
    this.issueSub$ = this.issuesService
      .getUsersProjectIssue(this.targetUsername, this.targetProjectName, this.targetIssueNumber)
      .subscribe((result) => {
        this.issue = result.data;
        console.log(this.issue);
      });
  }
}
