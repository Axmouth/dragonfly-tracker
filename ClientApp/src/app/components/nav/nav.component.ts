import { Component, OnInit, OnDestroy, PLATFORM_ID, Inject, ChangeDetectionStrategy } from '@angular/core';
import { Subscription, Subject } from 'rxjs';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';
import { TokenService } from '../../auth/services/token.service';
import { takeUntil, map, share } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { RouteStateService } from 'src/app/services/route-state.service';
import { AntiForgeryService } from '../../services/anti-forgery.service';
import { IsBrowserService } from '../../auth/helpers/services/is-browser.service';

export enum AuthenticatedState {
  Loading = 1,
  True,
  False,
}

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit, OnDestroy {
  isLoggedIn: AuthenticatedState = AuthenticatedState.Loading;
  authenticatedStates = AuthenticatedState;
  vertNavCollapsed = true;
  username = '{{ username }}';
  ngUnsubscribe = new Subject<void>();

  constructor(
    protected authService: AuthService,
    private router: Router,
    private isBrowserService: IsBrowserService,
    private routeStateService: RouteStateService,
    private antiForgeryService: AntiForgeryService,
  ) {}

  async ngOnInit() {
    if (this.isBrowserService.isInBrowser()) {
      // await this.antiForgeryService.getAntiForgeryToken();
      this.authService
        .onAuthenticationChange()
        .pipe(
          map(
            async (loggedIn) => {
              // console.log('dfdff');
              console.log(loggedIn);
              if (!loggedIn) {
                this.isLoggedIn = AuthenticatedState.False;
              } else {
                this.isLoggedIn = AuthenticatedState.True;
              }

              this.authService
                .getUsername()
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe(
                  (newUsername) => {
                    this.username = newUsername;
                  },
                  (err) => {
                    console.log(err);
                  },
                );
            },
            (err) => {
              console.log(err);
            },
          ),
        )
        .subscribe(() => {});
      this.authService
        .isAuthenticatedOrRefresh(this.antiForgeryService.getAntiForgeryToken$())
        .pipe(takeUntil(this.ngUnsubscribe))
        .pipe(
          map(
            async (loggedIn) => {
              if (!loggedIn) {
                this.isLoggedIn = AuthenticatedState.False;
              } else {
                this.isLoggedIn = AuthenticatedState.True;
              }
            },
            (err) => {
              console.log(err);
            },
          ),
        )
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe(() => {});
      // this.antiForgeryService.getAntiForgeryTokenBefore$(refresh$).pipe(takeUntil(this.ngUnsubscribe)).subscribe();
      this.router.events
        .pipe(
          map(async (val) => {
            if (val instanceof NavigationStart) {
              // console.log('dddd');
              if (val?.url?.includes && val.url.includes('logout')) {
                return;
              }
              this.authService
                .isAuthenticatedOrRefresh(this.antiForgeryService.getAntiForgeryToken$())
                .pipe(takeUntil(this.ngUnsubscribe))
                .pipe(
                  map(
                    async (loggedIn) => {
                      if (!loggedIn) {
                        this.isLoggedIn = AuthenticatedState.False;
                      } else {
                        this.isLoggedIn = AuthenticatedState.True;
                      }
                    },
                    (err) => {
                      console.log(err);
                    },
                  ),
                )
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe(() => {});
            }
          }),
        )
        .subscribe(() => {});
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
