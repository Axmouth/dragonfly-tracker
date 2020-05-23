import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../auth/services/auth.service';
import { AuthResult } from '../../auth/internal/auth-result';
import { IsBrowserService } from '../../helpers/services/is-browser.service';
import { Subject } from 'rxjs';
import { takeUntil, map } from 'rxjs/operators';
import { RouteStateService } from 'src/app/services/route-state.service';
import { AntiForgeryService } from 'src/app/services/anti-forgery.service';

@Component({
  selector: 'app-verify-email',
  templateUrl: './verify-email.component.html',
  styleUrls: ['./verify-email.component.scss'],
})
export class VerifyEmailComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  result: AuthResult;
  errors: string[] = [];
  loading = true;
  success: boolean;
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
    const result$ = this.authService.verifyEmail().pipe(
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
