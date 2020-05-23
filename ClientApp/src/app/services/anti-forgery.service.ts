import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { apiRoot } from 'src/environments/environment';
import { CookieService } from '../helpers/services/cookie.service';
import { Observable } from 'rxjs';
import { mergeMap, switchMap, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class AntiForgeryService {
  constructor(private http: HttpClient, private cookieService: CookieService) {}

  async getAntiForgeryToken() {
    const result: any = await this.http.get(`${apiRoot}/antiforgery`, { withCredentials: true }).toPromise();
  }

  getAntiForgeryToken$() {
    return this.http.get(`${apiRoot}/antiforgery`, { withCredentials: true }).pipe(
      map(() => {
        console.log('getAntiForgeryToken$()');
      }),
    );
  }

  getAntiForgeryTokenBeforeAndAfter$(obs$: Observable<any>) {
    return this.getAntiForgeryToken$()
      .pipe(
        switchMap((obj) => {
          // console.log('getAntiForgeryTokenBeforeAndAfter$1');
          // console.log(obj);
          return obs$;
        }),
      )
      .pipe(
        switchMap((obj) => {
          // console.log('getAntiForgeryTokenBeforeAndAfter$2');
          // console.log(obj);
          return this.getAntiForgeryToken$();
        }),
      );
  }

  getAntiForgeryTokenBefore$(obs$: Observable<any>) {
    return this.getAntiForgeryToken$().pipe(
      switchMap((obj) => {
        // console.log('getAntiForgeryTokenBefore$');
        // console.log(obj);
        return obs$;
      }),
    );
  }
}
