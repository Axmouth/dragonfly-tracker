import { Component, OnInit, OnDestroy } from '@angular/core';
import { Issue } from '../../models/api/issue';
import { IssuePost } from '../../models/api/issue-post';
import { Subscription, Subject } from 'rxjs';
import { ProjectsService } from 'src/app/services/projects.service';
import { ActivatedRoute } from '@angular/router';
import { IssuesService } from 'src/app/services/issues.service';
import { takeUntil } from 'rxjs/operators';
import { IsBrowserService } from '../../auth/helpers/services/is-browser.service';
import { UserService } from '../../services/user.service';
import { User } from '../../models/api/user';
import { AuthService } from '../../auth/services/auth.service';

@Component({
  selector: 'app-view-user-project-issue',
  templateUrl: './view-user-project-issue.component.html',
  styleUrls: ['./view-user-project-issue.component.scss'],
})
export class ViewUserProjectIssueComponent implements OnInit, OnDestroy {
  issue = new Issue();
  issuePostList: IssuePost[] = [];
  issuePostsSub$: Subscription;
  issueSub$: Subscription;
  loadingIssuePosts = true;
  ngUnsubscribe = new Subject<void>();
  targetUsername = '';
  targetProjectName = '';
  targetIssueNumber = 0;
  loading = true;
  found = true;
  replyToIssue = false;
  showDeleteIssueDialog = false;
  showDeleteIssuePostDialog = false;
  targetIssuePost: IssuePost = new IssuePost();

  constructor(
    private projectsService: ProjectsService,
    private route: ActivatedRoute,
    private issuesService: IssuesService,
    private isBrowserService: IsBrowserService,
    private userService: UserService,
    private authService: AuthService,
  ) {}

  ngOnInit() {
    const params = this.route.snapshot.paramMap;
    this.targetUsername = params.get('username');
    this.targetProjectName = params.get('projectname');
    this.targetIssueNumber = +params.get('issueNumber');
    this.issueSub$ = this.issuesService
      .getUsersProjectIssue(this.targetUsername, this.targetProjectName, this.targetIssueNumber)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
        (result) => {
          this.issue = result.data;
          this.loading = false;
          this.found = true;
          console.log(this.issue);
          this.issuesService
            .getUsersProjectsIssuePosts(this.targetUsername, this.targetProjectName, this.issue.number)
            .subscribe((postsResult) => {
              this.issuePostList = postsResult.data;
            });
        },
        (err) => {
          this.found = false;
          this.loading = false;
          console.log(err);
        },
      );
  }

  onReplyClick() {
    this.replyToIssue = !this.replyToIssue;
  }

  onIssueReplySubmit(issuePost: IssuePost) {
    console.log(issuePost);
    this.issuesService
      .createUsersProjectIssuePost(this.targetUsername, this.targetProjectName, this.issue.number, issuePost)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(async (result) => {
        const user = new User();
        user.userName = await this.authService.getUsername().toPromise();
        result.data.author = user;
        this.issuePostList.push(result.data);
        console.log(result);
      });
  }

  onIssueDeleteClick() {
    this.showDeleteIssueDialog = true;
  }

  deleteIssue() {
    console.log('deleto');
    this.showDeleteIssueDialog = false;
  }

  onIssuePostDelete(issuePost: IssuePost) {
    this.showDeleteIssuePostDialog = true;
    this.targetIssuePost = issuePost;
  }

  deleteIssuePost() {
    this.showDeleteIssuePostDialog = false;
    console.log('deleteIssuePost');
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
