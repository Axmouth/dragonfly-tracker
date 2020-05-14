import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { AuthResult } from 'src/app/auth/internal/auth-result';
import { AuthService } from 'src/app/auth';
import { IsBrowserService } from 'src/app/helpers/services/is-browser.service';
import { RouteStateService } from 'src/app/services/route-state.service';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-request-password-reset',
  templateUrl: './request-password-reset.component.html',
  styleUrls: ['./request-password-reset.component.scss'],
})
export class RequestPasswordResetComponent implements OnInit, OnDestroy {
  previousUrl: string;
  ngUnsubscribe = new Subject<void>();
  result: AuthResult;
  errors: string[] = [];
  loading = false;
  success: boolean;
  successMessages: string[];
  email: string;

  constructor(
    private authService: AuthService,
    private isBrowserService: IsBrowserService,
    private routeStateService: RouteStateService,
  ) {}

  ngOnInit() {
    this.previousUrl = this.routeStateService.getPreviousUrl();
    if (!this.isBrowserService.isInBrowser()) {
      return;
    }
  }

  onPasswordResetRequestSubmit() {
    this.loading = true;
    this.authService
      .requestPasswordReset({ email: this.email })
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(
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
      );
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
