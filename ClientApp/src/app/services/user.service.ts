import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth/services/auth.service';
import { pageSizeConst } from '../helpers/constants';
import { Observable } from 'rxjs';
import { PagedResponse } from '../models/api/paged-response';
import { Project } from '../models/api/project';
import { apiRoot } from 'src/environments/environment';
import { User } from '../models/Api/user';
import { Response } from '../models/Api/response';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getAllUsers(page: number = 1, myPageSize = pageSizeConst, search = ''): Observable<PagedResponse<User>> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    let url = `${apiRoot}/users?PageSize=${myPageSize}`;
    if (page) {
      url = url + `&PageNumber=${page}`;
    }
    return this.http.get<PagedResponse<User>>(url, { headers });
  }

  getUser(username: string): Observable<Response<User>> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'application/json');
    const url = `${apiRoot}/users/${username}`;
    return this.http.get<Response<User>>(url, { headers });
  }
}
