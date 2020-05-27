import { Component, OnInit, Input, ChangeDetectionStrategy, Output, EventEmitter } from '@angular/core';
import { IssuePost } from '../../models/api/issue-post';

@Component({
  selector: 'app-issue-post',
  templateUrl: './issue-post.component.html',
  styleUrls: ['./issue-post.component.scss'],
  // changeDetection: ChangeDetectionStrategy.OnPush
})
export class IssuePostComponent implements OnInit {
  @Input()
  issuePost: IssuePost;
  showReplyForm = false;
  @Output()
  issuePostDelete: EventEmitter<IssuePost> = new EventEmitter<IssuePost>();

  constructor() {}

  ngOnInit() {}

  onIssuePostDeleteClick() {
    this.issuePostDelete.next(this.issuePost);
  }
}
