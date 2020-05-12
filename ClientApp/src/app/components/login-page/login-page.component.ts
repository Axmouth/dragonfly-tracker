import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { catchError } from 'rxjs/operators';
import { AuthResult } from '../../models/internal/auth-result';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent implements OnInit {
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
    this.authService.authenticate(this.form).subscribe(
      (result: AuthResult) => {
        console.log(result);
        if (result.isSuccess()) {
          this.loginFailed = false;
          this.errors = [];
          this.router.navigateByUrl('');
        } else {
          this.loginFailed = true;
          this.errors = result.getResponse().error.errors;
        }
      },
      (err) => {
        // console.log(err);
      },
    );
  }
}
