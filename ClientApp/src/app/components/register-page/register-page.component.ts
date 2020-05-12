import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss'],
})
export class RegisterPageComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();

  registerForm = this.formBuilder.group({
    type: ['local'],
    userName: [''],
    email: [''],
    password: [''],
    password2: [''],
  });
  errors: String[] = [];
  registerFailed = false;

  constructor(private authService: AuthService, private router: Router, private formBuilder: FormBuilder) {}

  ngOnInit() {}

  onRegisterClick() {
    const form = {
      type: 'local',
      userName: this.registerForm.get('userName').value,
      email: this.registerForm.get('email').value,
      password: this.registerForm.get('password').value,
    };

    this.authService
      .register(form)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
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

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
