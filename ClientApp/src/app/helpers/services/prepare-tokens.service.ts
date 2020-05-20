import { Injectable } from '@angular/core';
import { AntiForgeryService } from '../../services/anti-forgery.service';
import { AuthService } from '../../auth/services/auth.service';
import { mergeMap, switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PrepareTokensService {
  constructor(private antiForgeryService: AntiForgeryService, private authService: AuthService) {}

  applyTokenChain$(obs$: Observable<any>) {
    const antiFoorgeryToken$ = this.antiForgeryService.getAntiForgeryToken$();
    const isAuthenticatedOrRefresh$ = this.authService.isAuthenticatedOrRefresh();

    return antiFoorgeryToken$
      .pipe(
        switchMap(() => {
          return isAuthenticatedOrRefresh$;
        }),
      )
      .pipe(
        switchMap(() => {
          return obs$;
        }),
      );
  }

  async getTokens() {
    await this.antiForgeryService.getAntiForgeryToken();
    await this.authService.isAuthenticatedOrRefresh().toPromise();
  }
}
