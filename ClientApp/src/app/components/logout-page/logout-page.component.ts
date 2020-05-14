import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-logout-page',
  templateUrl: './logout-page.component.html',
  styleUrls: ['./logout-page.component.scss'],
})
export class LogoutPageComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.authService
      .logout()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        if (result.isSuccess()) {
          this.router.navigateByUrl('');
        } else {
        }
      });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
