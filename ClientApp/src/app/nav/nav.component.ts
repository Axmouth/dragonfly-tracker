import { Component, OnInit, OnDestroy } from '@angular/core';
import { NbAuthService, NbTokenService } from '@nebular/auth';
import { NbMenuService } from '@nebular/theme';
import { filter, map } from 'rxjs/operators';
import { Observable, Subscribable, Subscription } from 'rxjs';
import { tokenGetter, myRefreshNbPasswordAuthStrategyOptions } from '../constants';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit, OnDestroy {
  isAuthenticatedOrRefresh$: Subscription;
  onAuthenticationChange$: Subscription;
  routeChange$: Subscription;
  isLoggedIn = false;
  username = 'username';

  constructor(protected authService: NbAuthService, private router: Router, private tokenService: NbTokenService) { }

  async ngOnInit() {

      this.onAuthenticationChange$ = this.authService.onAuthenticationChange().subscribe(async loggedIn => {
          if (!loggedIn) {
              // const refresh$ = this.authService.refreshToken(myRefreshNbPasswordAuthStrategyOptions.name, { token: tokenGetter() }).subscribe(async (loggedIn) => { this.isLoggedIn = loggedIn.isSuccess() });
              // refresh$.unsubscribe();
              this.isLoggedIn = loggedIn;
          } else {
            this.isLoggedIn = loggedIn;
          }
      });

      this.routeChange$ = this.router.events.subscribe(async val => {
          if (val instanceof NavigationStart) {
              const refresh$ = this.authService.isAuthenticatedOrRefresh().subscribe(async (loggedIn) => { this.isLoggedIn = loggedIn });
          }
      });

      this.username = (await (await this.tokenService.get().toPromise()).getPayload()).sub;
    }

    ngOnDestroy(): void {
       this.isAuthenticatedOrRefresh$.unsubscribe();
       this.onAuthenticationChange$.unsubscribe();
    }

}
