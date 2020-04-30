import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { pageSizeConst } from '../constants';
import { AuthService } from './auth.service';
import { apiRoot } from 'src/environments/environment';

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
  ) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
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
    return this.http.get(url, { headers });
  }

  getUsersProjectIssue(username: string, projectName: string, issueNumber: number) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}`;
    return this.http.get(url, { headers });
  }

  deleteUsersProjectIssue(username: string, projectName: string, issueNumber: number) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}`;
    return this.http.delete(url, { headers });
  }

  createUsersProjectIssue(username: string, projectName: string, newIssue: object) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues`;
    return this.http.post(url, newIssue, { headers });
  }

  updateUsersProjectIssue(username: string, projectName: string, issueNumber: number, updatedIssue: object) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}`;
    return this.http.put(url, updatedIssue, { headers });
  }

  getUsersProjectsIssuePosts(
    username: string,
    projectName: string,
    issueNumber: number,
    page: number = 1,
    myPageSize = pageSizeConst,
  ) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    let url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts?PageSize=${myPageSize}`;
    if (page) {
      url = url + `&PageNumber=${page}`;
    }
    return this.http.get(url, { headers });
  }

  getUsersProjectIssuePost(username: string, projectName: string, issueNumber: number, issuePostNumber: number) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}`;
    return this.http.get(url, { headers });
  }

  deleteUsersProjectIssuePost(username: string, projectName: string, issueNumber: number, issuePostNumber: number) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}`;
    return this.http.delete(url, { headers });
  }

  createUsersProjectIssuePost(username: string, projectName: string, issueNumber: number, newIssuePost: object) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/${projectName}/issues/${issueNumber}/issue-posts`;
    return this.http.post(url, newIssuePost, { headers });
  }

  updateUsersProjectIssuePost(
    username: string,
    projectName: string,
    issueNumber: number,
    issuePostNumber: number,
    updatedIssuePost: object,
  ) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}`;
    return this.http.put(url, updatedIssuePost, { headers });
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
  ) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/${projectName}/issues/${issueNumber}/issue-posts/${issuePostNumber}/issue-reactions`;
    return this.http.post(url, newIssuePostReaction, { headers });
  }
}
