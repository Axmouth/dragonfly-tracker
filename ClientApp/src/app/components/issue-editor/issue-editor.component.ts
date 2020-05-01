import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { Issue } from 'src/app/models/issue';
import { IssueStage } from '../../models/issue-stage';
import { IssueType } from '../../models/issue-type';

@Component({
  selector: 'app-issue-editor',
  templateUrl: './issue-editor.component.html',
  styleUrls: ['./issue-editor.component.scss'],
})
export class IssueEditorComponent implements OnInit, OnDestroy {
  issueTitle: string;
  issueContent: string;
  issueStage: IssueStage;
  issueTypes: IssueType[];
  @Input()
  oldIssue: Issue;
  @Output()
  submitIssue = new EventEmitter<Issue>();
  ngUnsubscribe = new Subject<void>();

  constructor() {}

  ngOnInit() {
    if (this.oldIssue) {
      this.issueTitle = this.oldIssue.title;
      this.issueContent = this.oldIssue.content;
      this.issueStage = this.oldIssue.stage;
      this.issueTypes = this.oldIssue.types;
    }
  }

  onProjectPublicChange(event) {
    // this.isPublic = event;
  }

  onProjectSubmitClick() {
    const newIssue: Issue = {
      title: this.issueTitle,
      content: this.issueContent,
      stage: this.issueStage,
      types: this.issueTypes,
    };
    this.submitIssue.next(newIssue);
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
