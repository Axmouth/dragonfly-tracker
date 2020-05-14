import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { AuthResult } from '../../auth/internal/auth-result';
import { IsBrowserService } from '../../helpers/services/is-browser.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { RouteStateService } from 'src/app/services/route-state.service';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss'],
})
export class VerifyEmailComponent implements OnInit, OnDestroy {
  previousUrl: string;
  ngUnsubscribe = new Subject<void>();
  result: AuthResult;
  errors: string[] = [];
  loading = true;
  success: boolean;
  successMessages: string[];

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
    this.authService
      .verifyEmail()
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
