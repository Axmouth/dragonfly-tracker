import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, Subject } from 'rxjs';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { TokenService } from '../../services/token.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent implements OnInit, OnDestroy {
  onAuthenticationChange$: Subscription;
  routeChange$: Subscription;
  isLoggedIn = false;
  vertNavCollapsed = true;
  username = '{{ username }}';
  ngUnsubscribe = new Subject<void>();

  constructor(protected authService: AuthService, private router: Router, private tokenService: TokenService) {}

  async ngOnInit() {
    this.onAuthenticationChange$ = this.authService
      .onAuthenticationChange()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(async (loggedIn) => {
        if (!loggedIn) {
        } else {
        }
        this.isLoggedIn = loggedIn;

        this.authService
          .getUsername()
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe((newUsername) => {
            this.username = newUsername;
          });
      });

    this.routeChange$ = this.router.events.subscribe(async (val) => {
      if (val instanceof NavigationStart) {
        const refresh$ = this.authService
          .isAuthenticatedOrRefresh()
          .pipe(takeUntil(this.ngUnsubscribe))
          .subscribe(async (loggedIn) => {
            this.isLoggedIn = loggedIn;
          });
      }
    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
