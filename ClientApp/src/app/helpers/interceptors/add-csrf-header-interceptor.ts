import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IsBrowserService } from '../../auth/helpers/services/is-browser.service';

@Injectable()
export class AddCsrfHeaderInterceptor implements HttpInterceptor {
  /**
   *
   */
  constructor(private isBrowserService: IsBrowserService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const requestToken = this.getCookieValue('X-XSRF-TOKEN');
    return next.handle(
      req.clone({
        headers: req.headers.set('X-XSRF-TOKEN', requestToken),
      }),
    );
  }

  private getCookieValue(cookieName: string): string {
    if (!this.isBrowserService.isInBrowser()) {
      return '';
    }

    const allCookies = decodeURIComponent(document.cookie).split('; ');
    for (let i = 0; i < allCookies.length; i++) {
      const cookie = allCookies[i];
      if (cookie.startsWith(cookieName + '=')) {
        return cookie.substring(cookieName.length + 1);
      }
    }
    return '';
  }
}
