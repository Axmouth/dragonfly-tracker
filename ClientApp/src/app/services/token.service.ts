import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { filter, share } from 'rxjs/operators';
import { AuthEmptyTokenError } from '../models/internal/auth-empty-token-error';
import { AuthIllegalJWTTokenError } from '../models/internal/auth-illegal-jwt-token-error';
import { AuthJWTToken, AuthCreateJWTToken } from '../models/internal/auth-jwt-token';
import { TokenPack } from '../models/internal/token-pack';
import { AuthToken } from '../models/internal/auth-token';
import { isPlatformBrowser, isPlatformServer } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  protected token$: BehaviorSubject<AuthToken> = new BehaviorSubject(null);

  protected key = 'auth_app_token';

  constructor(@Inject(PLATFORM_ID) private platform: Object) {
    if (isPlatformBrowser(platform)) {
      this.publishStoredToken();
    }
  }

  /**
   * Returns observable of current token
   * @returns {Observable<AuthToken>}
   */
  get(): Observable<AuthToken> {
    // const token = this.tokenStorage.get();
    if (isPlatformServer(this.platform)) {
      return of(this.unwrap(''));
    }
    const raw = localStorage.getItem(this.key);
    const token = this.unwrap(raw);
    return of(token);
  }

  /**
   * Sets a token into the storage. This method is used by the AuthService automatically.
   *
   * @param {AuthToken} token
   * @returns {Observable<any>}
   */
  set(token: AuthToken): Observable<null> {
    const raw = this.wrap(token);
    localStorage.setItem(this.key, raw);
    this.publishStoredToken();
    return of(null);
  }

  /**
   * Removes the token and published token value
   *
   * @returns {Observable<any>}
   */
  clear(): Observable<null> {
    // this.tokenStorage.clear();
    localStorage.removeItem(this.key);
    this.publishStoredToken();
    return of(null);
  }

  /**
   * Publishes token when it changes.
   * @returns {Observable<AuthToken>}
   */
  tokenChange(): Observable<AuthToken> {
    return this.token$.pipe(
      filter((value) => !!value),
      share(),
    );
  }

  protected publishStoredToken() {
    const raw = localStorage.getItem(this.key);
    const token = this.unwrap(raw);
    this.token$.next(token);
  }

  protected wrap(token: AuthToken): string {
    return JSON.stringify({
      name: token.getName(),
      createdAt: token.getCreatedAt().getTime(),
      value: token.toString(),
    });
  }

  protected unwrap(value: string): AuthToken {
    // let tokenClass: AuthTokenClass = this.fallbackClass;
    let tokenValue = '';
    let tokenOwnerStrategyName = '';
    let tokenCreatedAt: Date = null;

    const tokenPack: TokenPack = this.parseTokenPack(value);
    if (tokenPack) {
      // tokenClass = this.getClassByName(tokenPack.name) || this.fallbackClass;
      tokenValue = tokenPack.value;
      tokenOwnerStrategyName = tokenPack.ownerStrategyName;
      tokenCreatedAt = new Date(Number(tokenPack.createdAt));
    }

    return AuthCreateJWTToken(tokenValue, tokenOwnerStrategyName, tokenCreatedAt);
  }

  protected parseTokenPack(value): TokenPack {
    try {
      return JSON.parse(value);
    } catch (e) {}
    return null;
  }
}
