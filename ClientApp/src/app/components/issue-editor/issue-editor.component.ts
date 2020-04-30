import { Component, OnInit, OnDestroy, Input, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { Issue } from 'src/app/models/issue';

@Component({
  selector: 'app-issue-editor',
  templateUrl: './issue-editor.component.html',
  styleUrls: ['./issue-editor.component.scss'],
})
export class IssueEditorComponent implements OnInit, OnDestroy {
  issueTitle: string;
  issueContent: string;
  issueStages: any[];
  issueTypes: any[];
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
      this.issueStages = this.oldIssue.stages;
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
      stages: this.issueStages,
      types: this.issueTypes,
    };
    this.submitIssue.next(newIssue);
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
