import { Injectable, OnDestroy } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, Subject, of } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { takeUntil, map } from 'rxjs/operators';
import { IsBrowserService } from '../../helpers/services/is-browser.service';
import { AntiForgeryService } from '../../services/anti-forgery.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  /**
   *
   */
  constructor(
    private router: Router,
    private authService: AuthService,
    private isBrowserService: IsBrowserService,
    private antiForgeryService: AntiForgeryService,
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.isBrowserService.isInBrowser()) {
      return of(true);
    }
    return this.authService
      .isAuthenticatedOrRefresh(this.antiForgeryService.getAntiForgeryToken$())
      .pipe(
        map((auth) => {
          if (!auth) {
            this.router.navigate(['login'], { queryParams: { returnUrl: state.url } });
          }
          return auth;
        }),
      )
      .pipe(takeUntil(this.ngUnsubscribe));
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
