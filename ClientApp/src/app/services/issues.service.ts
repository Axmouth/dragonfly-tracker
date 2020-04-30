import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { pageSizeConst } from '../constants';
import { AuthService } from './auth.service';
import { apiRoot } from 'src/environments/environment';
import { PagedResponse } from '../models/paged-response';
import { Issue } from '../models/issue';
import { ErrorResponse } from '../models/error-response';
import { Observable } from 'rxjs';
import { Response } from '../models/response';
import { IssuePost } from '../models/issue-post';
import { IssuePostReaction } from '../models/issue-post-reaction';

@Injectable({
  providedIn: 'root',
})
export class IssuesService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getUsersProjectsIssues(
    username: string,
    projectName: string,
    page: number = 1,
    myPageSize = pageSizeConst,
    search: string,
    isOpen: boolean,
  ): Observable<PagedResponse<Issue> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    let url = `${apiRoot}/users/${username}/projects/${projectName}/issues?PageSize=${myPageSize}`;
    if (page !== undefined) {
      url = url + `&PageNumber=${page}`;
    }
    if (search) {
      url = url + `&Search=${search}`;
    }
    if (isOpen !== undefined) {
      url = url + `&open=${isOpen}`;
    }
    return this.http.get<PagedResponse<Issue> | ErrorResponse>(url, { headers });
  }

  getUsersProjectIssue(
    username: string,
    projectName: string,
    issueNumber: number,
  ): Observable<Response<Issue> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}`;
    return this.http.get<Response<Issue> | ErrorResponse>(url, { headers });
  }

  deleteUsersProjectIssue(username: string, projectName: string, issueNumber: number) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}`;
    return this.http.delete(url, { headers });
  }

  createUsersProjectIssue(
    username: string,
    projectName: string,
    newIssue: object,
  ): Observable<Response<Issue> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues`;
    return this.http.post<Response<Issue> | ErrorResponse>(url, newIssue, { headers });
  }

  updateUsersProjectIssue(
    username: string,
    projectName: string,
    issueNumber: number,
    updatedIssue: object,
  ): Observable<Response<Issue> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}`;
    return this.http.put<Response<Issue> | ErrorResponse>(url, updatedIssue, { headers });
  }

  getUsersProjectsIssuePosts(
    username: string,
    projectName: string,
    issueNumber: number,
    page: number = 1,
    myPageSize = pageSizeConst,
  ): Observable<PagedResponse<IssuePost> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    let url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts?PageSize=${myPageSize}`;
    if (page) {
      url = url + `&PageNumber=${page}`;
    }
    return this.http.get<PagedResponse<IssuePost> | ErrorResponse>(url, { headers });
  }

  getUsersProjectIssuePost(
    username: string,
    projectName: string,
    issueNumber: number,
    issuePostNumber: number,
  ): Observable<Response<IssuePost> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}`;
    return this.http.get<Response<IssuePost> | ErrorResponse>(url, { headers });
  }

  deleteUsersProjectIssuePost(username: string, projectName: string, issueNumber: number, issuePostNumber: number) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}`;
    return this.http.delete(url, { headers });
  }

  createUsersProjectIssuePost(
    username: string,
    projectName: string,
    issueNumber: number,
    newIssuePost: object,
  ): Observable<Response<IssuePost> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/${projectName}/issues/${issueNumber}/issue-posts`;
    return this.http.post<Response<IssuePost> | ErrorResponse>(url, newIssuePost, { headers });
  }

  updateUsersProjectIssuePost(
    username: string,
    projectName: string,
    issueNumber: number,
    issuePostNumber: number,
    updatedIssuePost: object,
  ): Observable<Response<IssuePost> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}`;
    return this.http.put<Response<IssuePost> | ErrorResponse>(url, updatedIssuePost, { headers });
  }

  deleteUsersProjectIssuePostReaction(
    username: string,
    projectName: string,
    issueNumber: number,
    issuePostNumber: number,
    reactionID: string,
  ) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}/issue-reactions/${reactionID}`;
    return this.http.delete(url, { headers });
  }

  createUsersProjectIssuePostReaction(
    username: string,
    projectName: string,
    issueNumber: number,
    issuePostNumber: number,
    newIssuePostReaction: object,
    reactionType: string,
  ): Observable<Response<IssuePostReaction> | ErrorResponse> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}/issue-reactions`;
    return this.http.post<Response<IssuePostReaction> | ErrorResponse>(url, newIssuePostReaction, { headers });
  }
}
