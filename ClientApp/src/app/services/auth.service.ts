import { Injectable } from '@angular/core';
import { TokenService, AuthToken, AuthCreateJWTToken, AuthJWTToken, AuthIllegalTokenError } from './token.service';
import { map, switchMap, catchError } from 'rxjs/operators';
import { Observable, of as observableOf } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { apiRoute } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private tokenService: TokenService, private http: HttpClient, private route: ActivatedRoute) {}

  /**
   * Authenticates
   * Stores received token in the token storage
   *
   * Example:
   * authenticate('email', {email: 'email@example.com', password: 'test'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  authenticate(data?: any) {
    const result = this.http.request('post', `${apiRoute}/identity/login`, { body: data, observe: 'response' }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res.body,
          true,
          ['Login/Email combination is not correct, please try again.'],
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
  logout() {
    const url = `${apiRoute}/identity/logout`;
    const result = observableOf({}).pipe(
      switchMap((res: any) => {
        if (!url) {
          return observableOf(res);
        }
        return this.http.request('delete', url, { observe: 'response' });
      }),
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          ['Something went wrong, please try again.'],
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
          this.tokenService.clear().pipe(map(() => authResult));
        }
        return observableOf(authResult);
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
  register(data?: any) {
    const url = `${apiRoute}/identity/register`;
    const result = this.http.request('post', url, { body: data, observe: 'response' }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          ['Something went wrong, please try again.'],
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
  isAuthenticated() {
    return this.getToken().pipe(map((token: AuthToken) => token.isValid()));
  }

  /**
   * Returns true if valid auth token is present in the token storage.
   * If not, calls refreshToken, and returns isAuthenticated() if success, false otherwise
   * @returns {Observable<boolean>}
   */
  isAuthenticatedOrRefresh() {
    return this.getToken().pipe(
      switchMap((token) => {
        if (token.getValue() && !token.isValid()) {
          return this.refreshToken(token).pipe(
            switchMap((res) => {
              if (res.isSuccess()) {
                return this.isAuthenticated();
              } else {
                return observableOf(false);
              }
            }),
          );
        } else {
          return observableOf(token.isValid());
        }
      }),
    );
  }

  /**
   * Returns authentication status stream
   * @returns {Observable<boolean>}
   */
  onAuthenticationChange() {
    return this.onTokenChange().pipe(map((token: AuthToken) => token.isValid()));
  }

  /**
   * Sends a refresh token request
   * Stores received token in the token storage
   *
   * Example:
   * refreshToken('email', {token: token})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  refreshToken(data?: any) {
    const url = `${apiRoute}/identity/refresh`;
    return this.http
      .request('post', url, { body: data, observe: 'response' })
      .pipe(
        map((res) => {
          const token = AuthCreateJWTToken(res.body['token'], 'refreshToken');
          return new AuthResult(
            true,
            res,
            true,
            ['Something went wrong re-Authenticating'],
            ['Your token has been successfully refreshed.'],
            token,
          );
        }),
        catchError((res) => {
          return this.handleResponseError(res);
        }),
      )
      .pipe(
        switchMap((result: AuthResult) => {
          return this.processResultToken(result);
        }),
      );
  }

  protected handleResponseError(res: any): Observable<AuthResult> {
    return observableOf(new AuthResult(false, res, false, ''));
  }

  /**
   * Retrieves current authenticated token stored
   * @returns {Observable<any>}
   */
  getToken() {
    return this.tokenService.get();
  }

  /**
   * Returns tokens stream
   * @returns {Observable<AuthSimpleToken>}
   */
  onTokenChange() {
    return this.tokenService.tokenChange();
  }

  /**
   * Sends forgot password request
   *
   * Example:
   * requestPassword('email', {email: 'email@example.com'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  requestPassword(data?: any) {
    const url = `${apiRoute}/identity/request-pass`;
    return this.http.request('post', url, { body: data, observe: 'response' }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          ['Something went wrong, please try again.'],
          ['Reset password instructions have been sent to your email.'],
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
   * resetPassword('email', {newPassword: 'test'})
   *
   * @param data
   * @returns {Observable<AuthResult>}
   */
  resetPassword(data?: any) {
    const url = `${apiRoute}/identity/reset-pass`;
    const tokenKey = 'reset_password_token';
    data[tokenKey] = this.route.snapshot.queryParams[tokenKey];
    return this.http.request('post', url, { body: data, observe: 'response' }).pipe(
      map((res) => {
        return new AuthResult(
          true,
          res,
          true,
          ['Something went wrong, please try again.'],
          ['Your password has been successfully changed.'],
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

    return observableOf(result);
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

export class AuthResult {
  protected token: AuthToken;
  protected errors: string[] = [];
  protected messages: string[] = [];

  // TODO: better pass object
  constructor(
    protected success: boolean,
    protected response?: any,
    protected redirect?: any,
    errors?: any,
    messages?: any,
    token: AuthToken = null,
  ) {
    this.errors = this.errors.concat([errors]);
    if (errors instanceof Array) {
      this.errors = errors;
    }

    this.messages = this.messages.concat([messages]);
    if (messages instanceof Array) {
      this.messages = messages;
    }

    this.token = token;
  }

  getResponse(): any {
    return this.response;
  }

  getToken(): AuthToken {
    return this.token;
  }

  getRedirect(): string {
    return this.redirect;
  }

  getErrors(): string[] {
    return this.errors.filter((val) => !!val);
  }

  getMessages(): string[] {
    return this.messages.filter((val) => !!val);
  }

  isSuccess(): boolean {
    return this.success;
  }

  isFailure(): boolean {
    return !this.success;
  }
}
