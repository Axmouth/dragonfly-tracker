import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { AuthResult } from 'src/app/auth/internal/auth-result';
import { AuthService } from 'src/app/auth';
import { IsBrowserService } from 'src/app/helpers/services/is-browser.service';
import { RouteStateService } from 'src/app/services/route-state.service';
import { takeUntil, map } from 'rxjs/operators';
import { AntiForgeryService } from 'src/app/services/anti-forgery.service';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss'],
})
export class PasswordResetComponent implements OnInit, OnDestroy {
  previousUrl: string;
  ngUnsubscribe = new Subject<void>();
  result: AuthResult;
  errors: string[] = [];
  loading = false;
  success: boolean;
  newPassword: string;
  newPassword2: string;
  successMessages: string[];

  constructor(
    private authService: AuthService,
    private isBrowserService: IsBrowserService,
    private antiForgeryService: AntiForgeryService,
  ) {}

  ngOnInit() {
    if (!this.isBrowserService.isInBrowser()) {
      return;
    }
  }

  onPasswordChangeSubmit() {
    this.loading = true;
    const result$ = this.authService.passwordReset({ newPassword: this.newPassword }).pipe(
      map(
        (result) => {
          this.result = result;
          if (result.isSuccess()) {
            this.success = true;
            this.errors = [];
            this.successMessages = result.getMessages();
          } else {
            this.success = false;
            this.errors = result.getResponse().error.errors;
            console.log(this.errors);
          }
          this.loading = false;
        },
        (err) => {
          console.log(err);
        },
      ),
    );
    this.antiForgeryService.getAntiForgeryTokenBefore$(result$).pipe(takeUntil(this.ngUnsubscribe)).subscribe();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
