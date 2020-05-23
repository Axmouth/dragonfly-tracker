import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ProjectsService } from 'src/app/services/projects.service';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/auth/services/auth.service';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { User } from '../../models/api/user';
import { takeUntil, map } from 'rxjs/operators';
import { IsBrowserService } from '../../helpers/services/is-browser.service';
import { AuthResult } from '../../auth/internal/auth-result';
import { AntiForgeryService } from 'src/app/services/anti-forgery.service';

@Component({
  selector: 'app-user-preferences',
  templateUrl: './user-preferences.component.html',
  styleUrls: ['./user-preferences.component.scss'],
})
export class UserPreferencesComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  username: string;
  email: string;
  user: User = new User();
  emailVerificationRequestResult: AuthResult;
  emailVerificationRequestLoading = false;
  emailVerificationRequestSuccess = false;
  emailVerificationRequestErrors: string[] = [];
  emailVerificationRequestMessages: string[] = [];

  constructor(
    private projectsService: ProjectsService,
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private isBrowserService: IsBrowserService,
    private antiForgeryService: AntiForgeryService,
  ) {}

  async ngOnInit() {
    if (!this.isBrowserService.isInBrowser()) {
      return;
    }
    this.authService
      .getProfile()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((userResponse) => {
        this.user = userResponse.body.data;
        this.username = this.user.userName;
        this.email = this.user.email;
      });
  }

  onProfileSaveChangesClick() {}
  onAccountSaveChangesClick() {}

  onResendEmailVerificationClick() {
    this.emailVerificationRequestLoading = true;
    const result$ = this.authService.requestVerificationEmail({ email: this.email }).pipe(
      map(
        (result) => {
          this.emailVerificationRequestResult = result;
          if (result.isSuccess()) {
            this.emailVerificationRequestSuccess = true;
            this.emailVerificationRequestErrors = [];
            this.emailVerificationRequestMessages = result.getMessages();
          } else {
            this.emailVerificationRequestSuccess = false;
            this.emailVerificationRequestErrors = result.getResponse().errors;
          }
          this.emailVerificationRequestLoading = false;
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
