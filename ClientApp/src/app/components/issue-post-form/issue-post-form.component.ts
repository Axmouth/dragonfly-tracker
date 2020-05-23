import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IssuePost } from '../../models/api/issue-post';

@Component({
  selector: 'app-issue-post-form',
  templateUrl: './issue-post-form.component.html',
  styleUrls: ['./issue-post-form.component.scss'],
})
export class IssuePostFormComponent implements OnInit {
  content: string;
  @Input()
  initialIssuePost: IssuePost;
  @Output()
  issuePostSubmit: EventEmitter<IssuePost> = new EventEmitter<IssuePost>();

  constructor() {}

  ngOnInit() {
    if (this.initialIssuePost) {
      this.content = this.initialIssuePost.content;
    }
  }

  onSubmitClick() {
    const newPost = new IssuePost();
    newPost.content = this.content;
    this.issuePostSubmit.next(newPost);
  }
}
