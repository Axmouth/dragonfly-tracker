import { Injectable, Inject, PLATFORM_ID, OnDestroy } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, Subject, of } from 'rxjs';
import { AuthService } from '..';
import { isPlatformBrowser } from '@angular/common';
import { map, takeUntil } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class GuestGuard implements CanActivate, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  constructor(
    @Inject(PLATFORM_ID) private platform: Object,
    private router: Router,
    private authService: AuthService,
  ) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!isPlatformBrowser(this.platform)) {
      return of(true);
    }
    return this.authService
      .isAuthenticatedOrRefresh()
      .pipe(
        map((auth) => {
          if (auth) {
            this.router.navigate(['']);
          }
          return !auth;
        }),
      )
      .pipe(takeUntil(this.ngUnsubscribe));
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
