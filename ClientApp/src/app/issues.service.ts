import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { NbAuthService } from '@nebular/auth';
import { apiRoute, pageSizeConst } from './constants';

@Injectable({
  providedIn: 'root'
})
export class IssuesService {

  constructor(private http: HttpClient, private authService: NbAuthService) { }

}
