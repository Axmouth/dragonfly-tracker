import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth/services/auth.service';
import { Subject } from 'rxjs';
import { takeUntil, map } from 'rxjs/operators';
import { AntiForgeryService } from 'src/app/services/anti-forgery.service';

@Component({
  selector: 'app-logout-page',
  templateUrl: './logout-page.component.html',
  styleUrls: ['./logout-page.component.scss'],
})
export class LogoutPageComponent implements OnInit, OnDestroy {
  ngUnsubscribe = new Subject<void>();
  constructor(
    private authService: AuthService,
    private router: Router,
    private antiForgeryService: AntiForgeryService,
  ) {}

  ngOnInit() {
    const result$ = this.authService.logout().pipe(
      map(async (result) => {
        if (result.isSuccess()) {
          await this.antiForgeryService.getAntiForgeryToken();
          this.router.navigateByUrl('');
        } else {
          await this.antiForgeryService.getAntiForgeryToken();
        }
      }),
    );

    this.antiForgeryService.getAntiForgeryTokenBeforeAndAfter$(result$).pipe(takeUntil(this.ngUnsubscribe)).subscribe();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
