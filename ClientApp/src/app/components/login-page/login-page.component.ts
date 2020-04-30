import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent implements OnInit {
  form = {
    type: 'local',
    username: '',
    password: '',
    rememberMe: false,
  };
  isSuccess = false;
  isFailure = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {}

  onLoginClick() {
    console.log({ email: this.form.username, password: this.form.password });
    this.authService.authenticate({ email: this.form.username, password: this.form.password }).subscribe((result) => {
      console.log(result);
      if (result.isSuccess) {
        this.isFailure = false;
        this.isSuccess = true;
        this.router.navigateByUrl('');
      } else {
        this.isFailure = true;
        this.isSuccess = false;
      }
    });
  }
}
