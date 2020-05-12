import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { catchError, takeUntil } from 'rxjs/operators';
import { AuthResult } from '../../models/internal/auth-result';
import { Subject } from 'rxjs';

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

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {}

  onLoginClick() {
    this.authService
      .authenticate(this.form)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
        async (result: AuthResult) => {
          console.log(result);
          if (result.isSuccess()) {
            this.loginFailed = false;
            this.errors = [];
            await this.router.navigateByUrl('');
          } else {
            this.loginFailed = true;
            this.errors = result.getResponse().error.errors;
          }
        },
        (err) => {
          console.log(err);
        },
      );
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
