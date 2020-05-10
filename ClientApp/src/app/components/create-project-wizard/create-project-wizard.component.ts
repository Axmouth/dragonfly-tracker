import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Project } from 'src/app/models/api/project';
import { IssueType } from '../../models/api/issue-type';
import { IssueStage } from '../../models/api/issue-stage';
import { User } from '../../models/Api/user';
import { UserService } from '../../services/user.service';
import { Observable, Subject } from 'rxjs';
import { PagedResponse } from '../../models/Api/paged-response';
import { AuthService } from '../../services/auth.service';
import { takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-project-wizard',
  templateUrl: './create-project-wizard.component.html',
  styleUrls: ['./create-project-wizard.component.scss'],
})
export class CreateProjectWizardComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  wizardOpen = false;
  newProject = new Project();
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
  @Output()
  sendProject: EventEmitter<Project> = new EventEmitter<Project>();

  constructor(
    private projectsService: ProjectsService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.newProject.admins = [];
    this.newProject.maintainers = [];
    this.newProject.types = [];
    this.newProject.stages = [];
  }

  async onProjectNameType(event) {
    this.queryingProjectName = true;
    this.projectsService
      .getUsersProject(await this.authService.getUsername(), this.newProject.name)
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
    for (let i = 0; i < this.newProject.maintainers.length; i++) {
      if (this.newProject.maintainers[i].userName === this.newMaintainerName) {
        index = i;
      }
    }
    if (index > -1) {
      this.newProject.maintainers[index].userName = this.newMaintainerName;
    } else {
      this.newProject.maintainers.push({ userName: this.newMaintainerName });
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
    for (let i = 0; i < this.newProject.admins.length; i++) {
      if (this.newProject.admins[i].userName === this.newAdminName) {
        index = i;
      }
    }
    if (index > -1) {
      this.newProject.admins[index].userName = this.newAdminName;
    } else {
      this.newProject.admins.push({ userName: this.newAdminName });
    }
    this.newAdminName = undefined;
  }

  onCreateClick() {
    // this.newProject.
    this.wizardOpen = true;
  }

  onNewIssueTypeSubmit() {
    let index = -1;
    for (let i = 0; i < this.newProject.types.length; i++) {
      if (this.newProject.types[i].name === this.newIssueType) {
        index = i;
      }
    }
    if (index > -1) {
      this.newProject.types[index].name = this.newIssueType;
    } else {
      this.newProject.types.push({ name: this.newIssueType });
    }
    this.newIssueType = undefined;
  }

  onNewIssueStageSubmit() {
    let index = -1;
    for (let i = 0; i < this.newProject.stages.length; i++) {
      if (this.newProject.stages[i].name === this.newIssueStage) {
        index = i;
      }
    }
    if (index > -1) {
      this.newProject.stages[index].name = this.newIssueStage;
    } else {
      this.newProject.stages.push({ name: this.newIssueStage });
    }
    this.newIssueStage = undefined;
  }

  onStageLabelClick(stage: IssueStage) {
    const index = this.newProject.stages.indexOf(stage);
    if (index > -1) {
      this.newProject.stages.splice(index, 1);
    }
  }

  onTypeLabelClick(type: IssueType) {
    const index = this.newProject.types.indexOf(type);
    if (index > -1) {
      this.newProject.types.splice(index, 1);
    }
  }

  onAdminLabelClick(admin: User) {
    const index = this.newProject.admins.indexOf(admin);
    if (index > -1) {
      this.newProject.admins.splice(index, 1);
    }
  }

  onMaintainerLabelClick(maintainer: User) {
    const index = this.newProject.maintainers.indexOf(maintainer);
    if (index > -1) {
      this.newProject.maintainers.splice(index, 1);
    }
  }

  async onCommit() {
    // console.log(this.newProject);
    this.sendProject.next(this.newProject);
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
