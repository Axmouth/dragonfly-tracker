import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { Project } from 'src/app/models/project';

@Component({
  selector: 'app-project-editor',
  templateUrl: './project-editor.component.html',
  styleUrls: ['./project-editor.component.scss'],
})
export class ProjectEditorComponent implements OnInit, OnDestroy {
  projectName: string;
  projectDescription: string;
  isPublic = true;
  projectMaintainers: any[];
  projectAdmins: any[];
  projectIssueStages: any[];
  projectIssueTypes: any[];
  @Input()
  oldProject: Project;
  @Output()
  submitProject = new EventEmitter<Project>();
  ngUnsubscribe = new Subject<void>();

  constructor() {}

  ngOnInit() {
    if (this.oldProject) {
      this.projectName = this.oldProject.name;
      this.projectDescription = this.oldProject.name;
      this.isPublic = this.oldProject.isPublic;
      this.projectMaintainers = this.oldProject.maintainers;
      this.projectAdmins = this.oldProject.admins;
      this.projectIssueStages = this.oldProject.issueStages;
      this.projectIssueTypes = this.oldProject.issueTypes;
    }
  }

  onProjectPublicChange(event) {
    // this.isPublic = event;
  }

  onProjectSubmitClick() {
    const newProject: Project = {
      name: this.projectName,
      description: this.projectDescription,
      isPublic: this.isPublic,
      maintainers: this.projectMaintainers,
      admins: this.projectAdmins,
      issueStages: this.projectIssueStages,
      issueTypes: this.projectIssueTypes,
    };
    this.submitProject.next(newProject);
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
