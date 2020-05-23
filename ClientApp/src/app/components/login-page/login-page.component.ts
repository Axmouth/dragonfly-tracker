import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { catchError, takeUntil, map } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AuthResult } from 'src/app/auth/internal/auth-result';
import { AuthService, TokenService } from 'src/app/auth';
import { RouteStateService } from '../../services/route-state.service';
import { getBaseUrl } from '../../../main';
import { AntiForgeryService } from 'src/app/services/anti-forgery.service';
import { PrepareTokensService } from 'src/app/helpers/services/prepare-tokens.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  form = {
    type: 'local',
    userName: '',
    password: '',
    rememberMe: false,
  };
  errors: String[] = [];
  loginFailed = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private routeStateService: RouteStateService,
    private route: ActivatedRoute,
    private antiForgeryService: AntiForgeryService,
    private prepareTokensService: PrepareTokensService,
  ) {}

  ngOnInit() {}

  async onLoginClick() {
    const result$ = this.authService.authenticate(this.form).pipe(
      map(
        async (result: AuthResult) => {
          if (result.isSuccess()) {
            this.loginFailed = false;
            this.errors = [];
            await this.router.navigateByUrl(this.routeStateService.getPreviousUrl());
          } else {
            this.loginFailed = true;
            this.errors = result.getResponse().errors;
          }
        },
        (err) => {
          console.log(err);
        },
      ),
    );
    this.antiForgeryService.getAntiForgeryTokenBeforeAndAfter$(result$).subscribe(() => {});
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
