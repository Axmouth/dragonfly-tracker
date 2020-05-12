import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject, Observable } from 'rxjs';
import { Project } from 'src/app/models/api/project';
import { PagedResponse } from 'src/app/models/api/paged-response';
import { User } from 'src/app/models/api/user';
import { ProjectsService } from 'src/app/services/projects.service';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { takeUntil } from 'rxjs/operators';
import { IssueStage } from 'src/app/models/api/issue-stage';
import { IssueType } from 'src/app/models/api/issue-type';

@Component({
  selector: 'app-update-project-wizard',
  templateUrl: './update-project-wizard.component.html',
  styleUrls: ['./update-project-wizard.component.scss'],
})
export class UpdateProjectWizardComponent implements OnInit, OnDestroy {
  oldProjectName: string;
  ngUnsubscribe = new Subject<void>();
  wizardOpen = false;
  project = new Project();
  newIssueType: string;
  newIssueStage: string;
  adminSearch$: Observable<PagedResponse<User>>;
  newAdminName: string;
  maintainerSearch$: Observable<PagedResponse<User>>;
  newMaintainerName: string;
  adminNotFound = true;
  maintainerNotFound = true;
  projectFound = false;
  queryingProjectName = true;
  username: string;

  constructor(
    private projectService: ProjectsService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.project.admins = [];
    this.project.maintainers = [];
    this.project.types = [];
    this.project.stages = [];

    this.authService
      .getUsername()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((newUsername) => {
        console.log(newUsername);
        this.username = newUsername;
      });
  }

  async onProjectNameType(event) {
    this.queryingProjectName = true;
    this.projectService
      .getUsersProject(this.username, this.project.name)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
        (result) => {
          this.projectFound = true;
          this.queryingProjectName = false;
        },
        (err) => {
          this.projectFound = false;
          this.queryingProjectName = false;
        },
      );
  }

  onMaintainerType(event) {
    this.maintainerSearch$ = this.userService.getAllUsers(1, 5, this.newMaintainerName);
    this.userService
      .getUser(this.newMaintainerName)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
        (result) => {
          this.maintainerNotFound = false;
        },
        (err) => {
          this.maintainerNotFound = true;
        },
      );
  }

  onMaintainerSubmit() {
    let index = -1;
    for (let i = 0; i < this.project.maintainers.length; i++) {
      if (this.project.maintainers[i].userName === this.newMaintainerName) {
        index = i;
      }
    }
    if (index > -1) {
      this.project.maintainers[index].userName = this.newMaintainerName;
    } else {
      this.project.maintainers.push({ userName: this.newMaintainerName });
    }
    this.newMaintainerName = undefined;
  }

  onAdminType(event) {
    this.adminSearch$ = this.userService.getAllUsers(1, 5, this.newAdminName).pipe(takeUntil(this.ngUnsubscribe));
    this.userService
      .getUser(this.newAdminName)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
        (result) => {
          this.adminNotFound = false;
        },
        (err) => {
          this.adminNotFound = true;
        },
      );
  }

  onAdminSubmit() {
    let index = -1;
    for (let i = 0; i < this.project.admins.length; i++) {
      if (this.project.admins[i].userName === this.newAdminName) {
        index = i;
      }
    }
    if (index > -1) {
      this.project.admins[index].userName = this.newAdminName;
    } else {
      this.project.admins.push({ userName: this.newAdminName });
    }
    this.newAdminName = undefined;
  }

  onCreateClick() {
    // this.newProject.
    this.wizardOpen = true;
  }

  onNewIssueTypeSubmit() {
    let index = -1;
    for (let i = 0; i < this.project.types.length; i++) {
      if (this.project.types[i].name === this.newIssueType) {
        index = i;
      }
    }
    if (index > -1) {
      this.project.types[index].name = this.newIssueType;
    } else {
      this.project.types.push({ name: this.newIssueType });
    }
    this.newIssueType = undefined;
  }

  onNewIssueStageSubmit() {
    let index = -1;
    for (let i = 0; i < this.project.stages.length; i++) {
      if (this.project.stages[i].name === this.newIssueStage) {
        index = i;
      }
    }
    if (index > -1) {
      this.project.stages[index].name = this.newIssueStage;
    } else {
      this.project.stages.push({ name: this.newIssueStage });
    }
    this.newIssueStage = undefined;
  }

  onStageLabelClick(stage: IssueStage) {
    const index = this.project.stages.indexOf(stage);
    if (index > -1) {
      this.project.stages.splice(index, 1);
    }
  }

  onTypeLabelClick(type: IssueType) {
    const index = this.project.types.indexOf(type);
    if (index > -1) {
      this.project.types.splice(index, 1);
    }
  }

  onAdminLabelClick(admin: User) {
    const index = this.project.admins.indexOf(admin);
    if (index > -1) {
      this.project.admins.splice(index, 1);
    }
  }

  onMaintainerLabelClick(maintainer: User) {
    const index = this.project.maintainers.indexOf(maintainer);
    if (index > -1) {
      this.project.maintainers.splice(index, 1);
    }
  }

  async onCommit() {
    console.log(this.project);
    this.projectService
      .updateUsersProject(this.username, this.oldProjectName, this.project)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        console.log(result);
        this.router.navigateByUrl(`/user/${this.username}/${result['data']['name']}`);
      });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
