import { Component, OnInit } from '@angular/core';
import { ProjectsService } from '../../services/projects.service';
import { Project } from 'src/app/models/api/project';
import { IssueType } from '../../models/api/issue-type';
import { IssueStage } from '../../models/api/issue-stage';

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

  constructor(private projectService: ProjectsService) {}

  ngOnInit() {
    this.newProject.admins = [];
    this.newProject.maintainers = [];
    this.newProject.types = [];
    this.newProject.stages = [];
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
}
