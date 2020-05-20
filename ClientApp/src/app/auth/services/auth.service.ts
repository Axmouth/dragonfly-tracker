import { Injectable, Inject } from '@angular/core';
import { TokenService } from './token.service';
import { map, switchMap, catchError, mergeMap, concatMap, concatMapTo, switchMapTo, flatMap } from 'rxjs/operators';
import { Observable, of, observable } from 'rxjs';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { AuthSuccessResponse } from '../../models/api/auth-success-response';
import { EmptyResponse } from '../../models/api/empty-response';
import { AuthResult } from '../internal/auth-result';
import { AuthToken } from '../internal/auth-token';
import { AuthJWTToken, AuthCreateJWTToken } from '../internal/auth-jwt-token';
import { AuthIllegalTokenError } from '../internal/auth-illegal-token-error';
import { isPlatformBrowser } from '@angular/common';
import { AX_AUTH_OPTIONS } from '../auth-injection-token';
import { AuthModuleOptionsConfig } from '../auth-module-options-config';
import { User } from 'src/app/models/api/user';
import { IsBrowserService } from '../../helpers/services/is-browser.service';
import { Response } from 'src/app/models/api/response';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  authenticating = false;
  authEndpointPrefix: string;

  constructor(
    private tokenService: TokenService,
    private http: HttpClient,
    private route: ActivatedRoute,
    private isBrowserService: IsBrowserService,
    @Inject(AX_AUTH_OPTIONS) config: AuthModuleOptionsConfig,
  ) {
    this.authEndpointPrefix = config.authEndpointPrefix;
  }

  /**
   * Retrieves the logged in user's username
   * It is assumed it stored under sub inside the token
   *
   * @returns {Observable<string>}
   */
  getUsername(): Observable<string> {
    return this.tokenService.get().pipe(
      map((token) => {
        if (!this.isBrowserService.isInBrowser()) {
          return null;
        }
        const payload = token.getPayload();
        if (payload) {
          return payload.sub;
        }
        return null;
      }),
    );
  }

  /**
   * Retrieves the logged in user's email
   * It is assumed it stored under email inside the token
   *
   * @returns {Observable<string>}
   */
  getEmail(): Observable<string> {
    return this.tokenService.get().pipe(
      map((token) => {
        if (!this.isBrowserService.isInBrowser()) {
          return null;
        }
        const payload = token.getPayload();
        if (payload) {
          return payload.email;
        }
        return null;
      }),
    );
  }

  /**
   * Retrieves the logged in user's email
   * It is assumed it stored under email inside the token
   *
   * @returns {Observable<User>}
   */
  getProfile(): Observable<HttpResponse<Response<User>>> {
    const result = this.http
      .get<Response<User>>(`${this.authEndpointPrefix}profile`, {
        observe: 'response',
        withCredentials: true,
      })
      .pipe(
        map((res) => {
          return res;
        }),
      );
    return result;
  }

  /**
   * Authenticates
   * Stores received token in the token storage
   *
   * Example:
   * authenticate('{email: 'email@example.com', password: 'test'})
   * authenticate( {userName: 'email@example.com', password: 'test'})
   * authenticate( {userName: 'username', password: 'test'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  authenticate(data?: any): Observable<AuthResult> {
    const result = this.http
      .post<AuthSuccessResponse>(`${this.authEndpointPrefix}login`, data, {
        observe: 'response',
        withCredentials: true,
      })
      .pipe(
        map((res) => {
          return new AuthResult(
            true,
            res.body,
            true,
            [], // ['Login/Email combination is not correct, please try again.'],
            ['You have been successfully logged in.'],
            this.createToken(res.body['token'], true),
          );
        }),
        catchError((res) => {
          return this.handleResponseError(res);
        }),
      );
    return result.pipe(
      switchMap((authResult: AuthResult) => {
        return this.processResultToken(authResult);
      }),
    );
  }

  /**
   * Sign outs
   * Removes token from the token storage
   *
   * Example:
   * logout('email')
   *
   * @returns {Observable<AuthResult>}
   */
  logout(): Observable<AuthResult> {
    const url = `${this.authEndpointPrefix}logout`;
    const result = of({}).pipe(
      switchMap((res: any) => {
        if (!url) {
          return of(res);
        }
        return this.http.delete<EmptyResponse>(url, { observe: 'response', withCredentials: true });
      }),
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          [], // ['Something went wrong, please try again.'],
          ['You have been successfully logged out.'],
        );
      }),
      catchError((res) => {
        return this.handleResponseError(res);
      }),
    );
    return result.pipe(
      switchMap((authResult: AuthResult) => {
        if (authResult.isSuccess()) {
        }
        return this.tokenService.clear().pipe(map(() => authResult));
      }),
    );
  }

  /**
   * Registers
   * Stores received token in the token storage
   *
   * Example:
   * register('email', {email: 'email@example.com', name: 'Some Name', password: 'test'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  register(data?: any): Observable<AuthResult> {
    const url = `${this.authEndpointPrefix}register`;
    const result = this.http
      .post<AuthSuccessResponse>(url, data, { observe: 'response', withCredentials: true })
      .pipe(
        map((res) => {
          return new AuthResult(
            true,
            res,
            true,
            [], // ['Something went wrong, please try again.'],
            ['You have been successfully registered.'],
            this.createToken(res.body['token'], true),
          );
        }),
        catchError((res) => {
          return this.handleResponseError(res);
        }),
      );
    return result.pipe(
      switchMap((authResult: AuthResult) => {
        return this.processResultToken(authResult);
      }),
    );
  }

  /**
   * Returns true if auth token is present in the token storage
   * @returns {Observable<boolean>}
   */
  isAuthenticated(): Observable<boolean> {
    if (!this.isBrowserService.isInBrowser()) {
      return of(false);
    }
    return this.getToken().pipe(map((token: AuthToken) => token.isValid()));
  }

  /**
   * Returns true if valid auth token is present in the token storage.
   * If not, calls refreshToken, and returns isAuthenticated() if success, false otherwise
   * @returns {Observable<boolean>}
   */
  isAuthenticatedOrRefresh(callback?: Observable<any>): Observable<boolean> {
    if (!this.isBrowserService.isInBrowser()) {
      return of(false);
    }
    return this.getToken().pipe(
      switchMap((token) => {
        if (token.getValue() && !token.isValid()) {
          return this.refreshToken(token, callback).pipe(
            switchMap((res) => {
              if (res === null) {
                // For the case where there is an auth request in progress. Keep the status Quo
                return of(true);
              }
              if (res.isSuccess()) {
                return this.isAuthenticated();
              } else {
                // this.tokenService.clear();
                return this.logout().pipe(
                  map((result) => {
                    return !result.isSuccess();
                  }),
                );
                // return of(false);
              }
            }),
          );
        } else {
          return of(token.isValid());
        }
      }),
    );
  }

  /**
   * Returns authentication status stream
   * @returns {Observable<boolean>}
   */
  onAuthenticationChange(): Observable<boolean> {
    if (!this.isBrowserService.isInBrowser()) {
      return of(false);
    }
    return this.onTokenChange().pipe(map((token: AuthToken) => token.isValid()));
  }

  /**
   * Sends a refresh token request
   * Stores received token in the token storage
   *
   * Example:
   * refreshToken({token: token})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  refreshToken(data?: any, callback$?: Observable<any>): Observable<AuthResult> {
    if (this.authenticating) {
      // check if auth request is in progress and do nothing then
      return of(null);
    }
    // set the flag that there is an auth request in progress
    this.authenticating = true;

    const url = `${this.authEndpointPrefix}refresh`;
    const refresh$ = this.http
      .post<AuthSuccessResponse>(url, data, { observe: 'response', withCredentials: true })
      .pipe(
        map((res) => {
          const token = AuthCreateJWTToken(res.body['token'], 'refreshToken');
          this.authenticating = false;
          return new AuthResult(
            true,
            res,
            true,
            [], // ['Something went wrong re-Authenticating'],
            ['Your token has been successfully refreshed.'],
            token,
          );
        }),
        catchError((res) => {
          this.authenticating = false;
          return this.handleResponseError(res);
        }),
      )
      .pipe(
        switchMap((result: AuthResult) => {
          this.authenticating = false;
          return this.processResultToken(result);
        }),
      );
    if (callback$ === undefined) {
      callback$ = of(null);
    }
    return callback$.pipe(
      mergeMap(() => {
        return refresh$;
      }),
    );
  }

  protected handleResponseError(res: any): Observable<AuthResult> {
    return of(new AuthResult(false, res, false, ''));
  }

  /**
   * Retrieves current authenticated token stored
   * @returns {Observable<any>}
   */
  getToken(): Observable<any> {
    return this.tokenService.get();
  }

  /**
   * Returns tokens stream
   * @returns {Observable<AuthToken>}
   */
  onTokenChange(): Observable<AuthToken> {
    return this.tokenService.tokenChange();
  }

  /**
   * Sends forgot password request
   *
   * Example:
   * requestPasswordReset({email: 'email@example.com'})
   * requestPasswordReset({userName: 'username'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  requestPasswordReset(data?: any): Observable<AuthResult> {
    const url = `${this.authEndpointPrefix}password-reset-email`;
    return this.http.post(url, data, { observe: 'response', withCredentials: true }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          [], // ['Something went wrong, please try again.'],
          ['Reset password instructions have been sent to your email!'],
        );
      }),
      catchError((res) => {
        return this.handleResponseError(res);
      }),
    );
  }

  /**
   * Tries to reset password
   *
   * Example:
   * passwordReset({newPassword: 'test'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  passwordReset(data?: any): Observable<AuthResult> {
    const url = `${this.authEndpointPrefix}password-reset`;
    const tokenQueryKey = 'reset_password_token';
    const userNameQueryKey = 'user_name';
    const emailQueryKey = 'email';
    const tokenKey = 'token';
    const userNameKey = 'userName';
    const emailKey = 'email';
    data[tokenKey] = this.route.snapshot.queryParams[tokenQueryKey];
    if (this.route.snapshot.queryParams[userNameQueryKey]) {
      data[userNameKey] = this.route.snapshot.queryParams[userNameQueryKey];
    }
    if (this.route.snapshot.queryParams[emailQueryKey]) {
      data[emailKey] = this.route.snapshot.queryParams[emailQueryKey];
    }
    return this.http.post(url, data, { observe: 'response', withCredentials: true }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          [], // ['Something went wrong, please try again.'],
          ['Your password has been successfully changed!'],
        );
      }),
      catchError((res) => {
        return this.handleResponseError(res);
      }),
    );
  }

  /**
   * Uses an email verification token to confirm you own the email address you used
   *
   * Example:
   * verifyEmail()
   *
   * @param
   * @returns {Observable<AuthResult>}
   */
  verifyEmail(): Observable<AuthResult> {
    const data = {};
    const url = `${this.authEndpointPrefix}email-confirm`;
    const tokenQueryKey = 'email_confirm_token';
    const userNameQueryKey = 'user_name';
    const emailQueryKey = 'email';
    const tokenKey = 'token';
    const userNameKey = 'userName';
    const emailKey = 'email';
    data[tokenKey] = this.route.snapshot.queryParams[tokenQueryKey];
    if (this.route.snapshot.queryParams[userNameQueryKey]) {
      data[userNameKey] = this.route.snapshot.queryParams[userNameQueryKey];
    }
    if (this.route.snapshot.queryParams[emailQueryKey]) {
      data[emailKey] = this.route.snapshot.queryParams[emailQueryKey];
    }
    return this.http.post(url, data, { observe: 'response', withCredentials: true }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          [], // ['Something went wrong, please try again.'],
          ['Your Email has been successfully verified!'],
        );
      }),
      catchError((res) => {
        return this.handleResponseError(res);
      }),
    );
  }

  /**
   * Requests an email for email verification
   *
   * Example:
   * verifyEmail({email: 'user@example.com'})
   *
   * @param
   * @returns {Observable<AuthResult>}
   */
  requestVerificationEmail(data?: any): Observable<AuthResult> {
    const url = `${this.authEndpointPrefix}email-confirm-email`;
    return this.http.post(url, data, { observe: 'response', withCredentials: true }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          [], // ['Something went wrong, please try again.'],
          ['Your verification Email has been successfully sent!'],
        );
      }),
      catchError((res) => {
        return this.handleResponseError(res);
      }),
    );
  }

  private processResultToken(result: AuthResult) {
    if (result.isSuccess() && result.getToken()) {
      return this.tokenService.set(result.getToken()).pipe(
        map((token: AuthToken) => {
          return result;
        }),
      );
    }

    return of(result);
  }

  createToken(value: any, failWhenInvalidToken?: boolean): AuthJWTToken {
    const token = AuthCreateJWTToken(value, 'refreshToken');
    // At this point, AuthCreateToken failed with AuthIllegalTokenError which MUST be intercepted
    // Or token is created. It MAY be created even if backend did not return any token, in this case it is !Valid
    if (failWhenInvalidToken && !token.isValid()) {
      // If we require a valid token (i.e. isValid), then we MUST throw AuthIllegalTokenError so that the we
      // intercept it
      throw new AuthIllegalTokenError('Token is empty or invalid.');
    }
    return token;
  }
}
