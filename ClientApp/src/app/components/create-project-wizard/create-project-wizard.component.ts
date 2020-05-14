import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Project } from 'src/app/models/api/project';
import { IssueType } from '../../models/api/issue-type';
import { IssueStage } from '../../models/api/issue-stage';
import { User } from '../../models/Api/user';
import { UserService } from '../../services/user.service';
import { Observable, Subject } from 'rxjs';
import { PagedResponse } from '../../models/Api/paged-response';
import { AuthService } from '../../auth/services/auth.service';
import { takeUntil, debounceTime } from 'rxjs/operators';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-create-project-wizard',
  templateUrl: './create-project-wizard.component.html',
  styleUrls: ['./create-project-wizard.component.scss'],
})
export class CreateProjectWizardComponent implements OnInit, OnDestroy {
  nameInfoForm = this.formBuilder.group({
    name: [''],
    description: [''],
  });
  adminForm = this.formBuilder.group({
    admin: [''],
  });
  maintainerForm = this.formBuilder.group({
    maintainer: [''],
  });
  issueTypeForm = this.formBuilder.group({
    type: [''],
  });
  issueStageForm = this.formBuilder.group({
    stage: [''],
  });
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
  username: string;
  @Output()
  sendProject: EventEmitter<Project> = new EventEmitter<Project>();

  constructor(
    private projectsService: ProjectsService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder,
  ) {}

  ngOnInit() {
    this.newProject.admins = [];
    this.newProject.maintainers = [];
    this.newProject.types = [];
    this.newProject.stages = [];

    this.authService
      .getUsername()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((newUsername) => {
        console.log(newUsername);
        this.username = newUsername;
      });

    this.nameInfoForm
      .get('name')
      .valueChanges.pipe(debounceTime(200))
      .subscribe((name) => {
        this.queryingProjectName = true;
        this.newProject.name = name;
        this.projectsService
          .getUsersProject(this.username, name)
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
      });

    this.nameInfoForm.get('description').valueChanges.subscribe((description) => {
      this.newProject.description = description;
    });

    this.issueTypeForm.get('type').valueChanges.subscribe((type) => {
      this.newIssueType = type;
    });

    this.issueStageForm.get('stage').valueChanges.subscribe((stage) => {
      this.newIssueStage = stage;
    });

    this.adminForm
      .get('admin')
      .valueChanges.pipe(debounceTime(200))
      .subscribe((admin) => {
        if (!admin) {
          this.adminNotFound = true;
          this.newAdminName = admin;
          return;
        }
        this.adminSearch$ = this.userService.getAllUsers(1, 5, admin).pipe(takeUntil(this.ngUnsubscribe));
        this.userService
          .getUser(admin)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe(
            (result) => {
              this.newAdminName = admin;
              this.adminNotFound = false;
            },
            (err) => {
              this.adminNotFound = true;
              this.newAdminName = admin;
            },
          );
      });

    this.maintainerForm
      .get('maintainer')
      .valueChanges.pipe(debounceTime(200))
      .subscribe((maintainer) => {
        if (!maintainer) {
          this.maintainerNotFound = true;
          this.newMaintainerName = maintainer;
          return;
        }
        this.maintainerSearch$ = this.userService.getAllUsers(1, 5, maintainer);
        this.userService
          .getUser(maintainer)
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe(
            (result) => {
              this.newMaintainerName = maintainer;
              this.maintainerNotFound = false;
            },
            (err) => {
              this.maintainerNotFound = true;
              this.newMaintainerName = maintainer;
            },
          );
      });
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
