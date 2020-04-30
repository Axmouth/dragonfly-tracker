import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { pageSizeConst } from '../constants';
import { AuthService } from './auth.service';
import { apiRoot } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ProjectsService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getUsersProjects(username: string, page: number = 1, myPageSize = pageSizeConst) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    let url = `${apiRoot}/users/${username}/projects?PageSize=${myPageSize}`;
    if (page) {
      url = url + `&PageNumber=${page}`;
    }
    return this.http.get(url, { headers });
  }

  getUsersProject(username: string, projectName: string) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}`;
    return this.http.get(url, { headers });
  }

  deleteUsersProject(username: string, projectName: string) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}`;
    return this.http.delete(url, { headers });
  }

  createUsersProject(username: string, newProject: object) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects`;
    return this.http.post(url, newProject, { headers });
  }

  updateUsersProject(username: string, projectName: string, newProject: object) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    const url = `${apiRoot}/users/${username}/projects/${projectName}`;
    return this.http.put(url, newProject, { headers });
  }
}
