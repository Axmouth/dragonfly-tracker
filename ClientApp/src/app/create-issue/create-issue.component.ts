import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { ProjectsService } from '../projects.service';
import { NbAuthService, NbTokenService } from '@nebular/auth';
import { Router, ActivatedRoute } from '@angular/router';
import { IssuesService } from '../issues.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-create-issue',
  templateUrl: './create-issue.component.html',
  styleUrls: ['./create-issue.component.scss']
})
export class CreateIssueComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  targetProjectName: string;

  constructor(private projectsService: ProjectsService, private authService: NbAuthService,
    private tokenService: NbTokenService, private router: Router, private issuesService: IssuesService,
    private route: ActivatedRoute) { }

  ngOnInit() {
  }

  async onIssueCreateClick(Issue) {
    const username = (await (await this.tokenService.get().toPromise()).getPayload()).sub;
    const params = this.route.snapshot.paramMap;
    this.targetProjectName = params.get("projectname");
    console.log(Issue);
    this.issuesService.createUsersProjectIssue(username, this.targetProjectName, Issue).pipe(takeUntil(this.ngUnsubscribe)).subscribe(result => {
      console.log(result);
      this.router.navigateByUrl(`/user/${username}/${this.targetProjectName}/issues/${result["data"]["number"]}`);

    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

}
