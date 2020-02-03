import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProjectsService } from '../projects.service';
import { NbAuthService, NbTokenService } from '@nebular/auth';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-own-project',
  templateUrl: './create-own-project.component.html',
  styleUrls: ['./create-own-project.component.scss']
})
export class CreateOwnProjectComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();

  constructor(private projectsService: ProjectsService, private authService: NbAuthService, private tokenService: NbTokenService, private router: Router) { }

  ngOnInit() {
  }

  async onProjectCreateClick(project) {
    const username = (await(await this.tokenService.get().toPromise()).getPayload()).sub;
    console.log(project);
    this.projectsService.createUsersProject(username, project).pipe(takeUntil(this.ngUnsubscribe)).subscribe(result => {
      console.log(result);
      this.router.navigateByUrl(`/user/${username}/${result['data']['name']}`);
    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

}
