import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { apiRoot } from 'src/environments/environment';
import { CookieService } from '../helpers/services/cookie.service';
import { Observable } from 'rxjs';
import { mergeMap, switchMap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AntiForgeryService {
  constructor(private http: HttpClient, private cookieService: CookieService) {}

  async getAntiForgeryToken() {
    const result: any = await this.http.get(`${apiRoot}/antiforgery`, { withCredentials: true }).toPromise();
  }

  getAntiForgeryToken$() {
    return this.http.get(`${apiRoot}/antiforgery`, { withCredentials: true });
  }

  getAntiForgeryTokenBeforeAndAfter$(obs$: Observable<any>) {
    return this.getAntiForgeryToken$()
      .pipe(
        switchMap(() => {
          return obs$;
        }),
      )
      .pipe(
        switchMap(() => {
          return this.getAntiForgeryToken$();
        }),
      );
  }
}
