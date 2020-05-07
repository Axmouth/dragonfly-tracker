import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Project } from 'src/app/models/api/project';
import { IssueType } from '../../models/api/issue-type';
import { IssueStage } from '../../models/api/issue-stage';
import { User } from '../../models/Api/user';
import { UserService } from '../../services/user.service';
import { Observable } from 'rxjs';
import { PagedResponse } from '../../models/Api/paged-response';

@Component({
  selector: 'app-create-project-wizard',
  templateUrl: './create-project-wizard.component.html',
  styleUrls: ['./create-project-wizard.component.scss'],
})
export class CreateProjectWizardComponent implements OnInit {
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

  constructor(private projectService: ProjectsService, private userService: UserService) {}

  ngOnInit() {
    this.newProject.admins = [];
    this.newProject.maintainers = [];
    this.newProject.types = [];
    this.newProject.stages = [];
  }

  onMaintainerType(event) {
    console.log(event);
    this.maintainerSearch$ = this.userService.getAllUsers(1, 5, this.newMaintainerName);
    console.log(event);
    this.userService.getUser(this.newMaintainerName).subscribe(
      (result) => {
        console.log(result);
        this.maintainerNotFound = false;
      },
      (err) => {
        console.log(err);
        this.maintainerNotFound = true;
      },
    );
  }

  onMaintainerSubmit() {
    let index = -1;
    for (let i = 0; i < this.newProject.maintainers.length; i++) {
      if (this.newProject.maintainers[i].username === this.newMaintainerName) {
        index = i;
      }
    }
    if (index > -1) {
      this.newProject.maintainers[index].username = this.newMaintainerName;
    } else {
      this.newProject.maintainers.push({ username: this.newMaintainerName });
    }
    this.newMaintainerName = undefined;
  }

  onAdminType(event) {
    console.log(event);
    this.adminSearch$ = this.userService.getAllUsers(1, 5, this.newAdminName);
    this.userService.getUser(this.newAdminName).subscribe(
      (result) => {
        console.log(result);
        this.adminNotFound = false;
      },
      (err) => {
        console.log(err);
        this.adminNotFound = true;
      },
    );
  }

  onAdminSubmit() {
    let index = -1;
    for (let i = 0; i < this.newProject.admins.length; i++) {
      if (this.newProject.admins[i].username === this.newAdminName) {
        index = i;
      }
    }
    if (index > -1) {
      this.newProject.admins[index].username = this.newAdminName;
    } else {
      this.newProject.admins.push({ username: this.newAdminName });
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

  onCommit() {
    console.log(this.newProject);
  }
}
