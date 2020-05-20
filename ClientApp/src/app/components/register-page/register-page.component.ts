import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, map } from 'rxjs/operators';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { AntiForgeryService } from 'src/app/services/anti-forgery.service';

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

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder,
    private antiForgeryService: AntiForgeryService,
  ) {}

  ngOnInit() {}

  async onRegisterClick() {
    const form = {
      type: 'local',
      userName: this.registerForm.get('userName').value,
      email: this.registerForm.get('email').value,
      password: this.registerForm.get('password').value,
    };
    // await this.antiForgeryService.getAntiForgeryToken();

    const result$ = this.authService
      .register(form)
      .pipe(takeUntil(this.ngUnsubscribe))
      .pipe(
        map(
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
        ),
      );
    this.antiForgeryService.getAntiForgeryTokenBeforeAndAfter$(result$).pipe(takeUntil(this.ngUnsubscribe)).subscribe();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
