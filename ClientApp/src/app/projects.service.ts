import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NbAuthService } from '@nebular/auth';
import { pageSizeConst, apiRoute } from './constants';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {


  constructor(private http: HttpClient, private authService: NbAuthService) { }


  getUsersProjects(username: string, page: number = 1, myPageSize = pageSizeConst) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    let url = `${apiRoute}/users/${username}/projects?PageSize=${myPageSize}`;
    if (page) {
      url = url + `&PageNumber=${page}`;
    }
    return this.http.get(url, { headers }).toPromise();
  }


  getUsersProject(username: string, projectName: string) {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    // tslint:disable-next-line: max-line-length
    let url = `${apiRoute}/users/${username}/projects/${projectName}`;
    return this.http.get(url, { headers }).toPromise();
  }
}
