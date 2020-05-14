import { Component, OnInit, OnDestroy, PLATFORM_ID, Inject } from '@angular/core';
import { Subscription, Subject } from 'rxjs';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';
import { TokenService } from '../../auth/services/token.service';
import { takeUntil } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { RouteStateService } from 'src/app/services/route-state.service';

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
    private tokenService: TokenService,
    @Inject(PLATFORM_ID) private platform: Object,
    private routeStateService: RouteStateService,
  ) {}

  async ngOnInit() {
    const log = this.isLoggedIn === AuthenticatedState.False;
    if (isPlatformBrowser(this.platform)) {
      this.authService
        .onAuthenticationChange()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe(async (loggedIn) => {
          if (!loggedIn) {
            this.isLoggedIn = AuthenticatedState.False;
          } else {
            this.isLoggedIn = AuthenticatedState.True;
          }

          this.authService
            .getUsername()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((newUsername) => {
              this.username = newUsername;
            });
        });
      this.authService
        .isAuthenticatedOrRefresh()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe(async (loggedIn) => {
          if (!loggedIn) {
            this.isLoggedIn = AuthenticatedState.False;
          } else {
            this.isLoggedIn = AuthenticatedState.True;
          }
        });
      this.router.events.subscribe(async (val) => {
        if (val instanceof NavigationStart) {
          const refresh$ = this.authService
            .isAuthenticatedOrRefresh()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe(async (loggedIn) => {
              if (!loggedIn) {
                this.isLoggedIn = AuthenticatedState.False;
              } else {
                this.isLoggedIn = AuthenticatedState.True;
              }
            });
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
