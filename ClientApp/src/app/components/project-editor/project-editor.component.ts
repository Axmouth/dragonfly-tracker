import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { Project } from 'src/app/models/api/project';
import { IssueStage } from '../../models/Api/issue-stage';
import { IssueType } from '../../models/Api/issue-type';
import { User } from '../../models/Api/user';

@Component({
  selector: 'app-project-editor',
  templateUrl: './project-editor.component.html',
  styleUrls: ['./project-editor.component.scss'],
})
export class ProjectEditorComponent implements OnInit, OnDestroy {
  projectName: string;
  projectDescription: string;
  private = true;
  projectMaintainers: User[];
  projectAdmins: User[];
  projectIssueStages: IssueStage[] = [{ name: 'test1' }, { name: 'name2' }];
  projectIssueTypes: IssueType[] = [{ name: 'derp' }, { name: 'dorp' }];
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
      this.private = this.oldProject.private;
      this.projectMaintainers = this.oldProject.maintainers;
      this.projectAdmins = this.oldProject.admins;
      this.projectIssueStages = this.oldProject.stages;
      this.projectIssueTypes = this.oldProject.types;
    }
  }

  onProjectPublicChange(event) {
    // this.isPublic = event;
  }

  onProjectSubmitClick() {
    const newProject: Project = {
      name: this.projectName,
      description: this.projectDescription,
      private: this.private,
      maintainers: this.projectMaintainers,
      admins: this.projectAdmins,
      stages: this.projectIssueStages,
      types: this.projectIssueTypes,
    };
    this.submitProject.next(newProject);
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
