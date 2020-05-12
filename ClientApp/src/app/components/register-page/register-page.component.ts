import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss'],
})
export class RegisterPageComponent implements OnInit {
  form = {
    type: 'local',
    userName: '',
    email: '',
    password: '',
    password2: '',
    rememberMe: false,
  };
  errors: String[] = [];
  registerFailed = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {}

  onRegisterClick() {
    this.authService.register(this.form).subscribe(
      (result) => {
        console.log(result);
        if (result.isSuccess()) {
          this.registerFailed = false;
          this.errors = [];
          this.router.navigateByUrl('');
        } else {
          this.registerFailed = true;
          this.errors = result.getResponse().error.errors;
        }
      },
      (err) => {
        console.log(err);
      },
    );
  }
}
