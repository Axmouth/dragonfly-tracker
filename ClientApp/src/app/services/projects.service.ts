import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { pageSizeConst } from '../constants';
import { AuthService } from './auth.service';
import { apiRoot } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Project } from '../models/project';
import { Response } from '../models/response';
import { PagedResponse } from '../models/paged-response';

@Injectable({
  providedIn: 'root',
})
export class ProjectsService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getUsersProjects(username: string, page: number = 1, myPageSize = pageSizeConst): Observable<PagedResponse<Project>> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    let url = `${apiRoot}/users/${username}/projects?PageSize=${myPageSize}`;
    if (page) {
      url = url + `&PageNumber=${page}`;
    }
    return this.http.get<PagedResponse<Project>>(url, { headers });
  }

  getUsersProject(username: string, projectName: string): Observable<Response<Project>> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}`;
    return this.http.get<Response<Project>>(url, { headers });
  }

  deleteUsersProject(username: string, projectName: string) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}`;
    return this.http.delete(url, { headers });
  }

  createUsersProject(username: string, newProject: object): Observable<Response<Project>> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects`;
    return this.http.post<Response<Project>>(url, newProject, { headers });
  }

  updateUsersProject(username: string, projectName: string, newProject: object): Observable<Response<Project>> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}/projects/${projectName}`;
    return this.http.put<Response<Project>>(url, newProject, { headers });
  }
}
