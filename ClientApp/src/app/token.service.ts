import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of as observableOf } from 'rxjs';
import { filter, share } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  protected token$: BehaviorSubject<AuthToken> = new BehaviorSubject(null);

  protected key = 'auth_app_token';

  constructor() {
    this.publishStoredToken();
  }

  /**
   * Returns observable of current token
   * @returns {Observable<AuthToken>}
   */
  get(): Observable<AuthToken> {
    // const token = this.tokenStorage.get();
    const raw = localStorage.getItem(this.key);
    const token = this.unwrap(raw);
    return observableOf(token);
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
    return observableOf(null);
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
    return observableOf(null);
  }

  /**
   * Publishes token when it changes.
   * @returns {Observable<AuthToken>}
   */
  tokenChange(): Observable<AuthToken> {
    return this.token$
      .pipe(
        filter(value => !!value),
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
    } catch (e) { }
    return null;
  }
  
}

export function AuthCreateJWTToken(
  token: any,
  ownerStrategyName: string,
  createdAt?: Date) {
  return new AuthJWTToken(token, ownerStrategyName, createdAt);
}

export interface AuthTokenClass<T = AuthToken> {
  NAME: string;
  new(raw: any, strategyName: string, expDate?: Date): T;
}

export interface TokenPack {
  name: string,
  ownerStrategyName?: string,
  createdAt: Number,
  value: string,
}

export abstract class AuthToken {

  protected payload: any = null;

  abstract getValue(): string;
  abstract isValid(): boolean;
  // the strategy name used to acquire this token (needed for refreshing token)
  abstract getCreatedAt(): Date;
  abstract toString(): string;

  getName(): string {
    return (this.constructor as AuthTokenClass).NAME;
  }

  getPayload(): any {
    return this.payload;
  }
}

/**
 * Wrapper for simple (text) token
 */
export class AuthSimpleToken extends AuthToken {

  static NAME = 'dragonfly:auth:simple:token';

  constructor(protected readonly token: any,
    protected readonly ownerStrategyName: string,
    protected createdAt?: Date) {
    super();
    try {
      this.parsePayload();
    } catch (err) {
      if (!(err instanceof AuthTokenNotFoundError)) {
        // token is present but has got a problem, including illegal
        throw err;
      }
    }
    this.createdAt = this.prepareCreatedAt(createdAt);
  }

  protected parsePayload(): any {
    this.payload = null;
  }

  protected prepareCreatedAt(date: Date) {
    return date ? date : new Date();
  }

  /**
   * Returns the token's creation date
   * @returns {Date}
   */
  getCreatedAt(): Date {
    return this.createdAt;
  }

  /**
   * Returns the token value
   * @returns string
   */
  getValue(): string {
    return this.token;
  }

  getOwnerStrategyName(): string {
    return this.ownerStrategyName;
  }

  /**
   * Is non empty and valid
   * @returns {boolean}
   */
  isValid(): boolean {
    return !!this.getValue();
  }

  /**
   * Validate value and convert to string, if value is not valid return empty string
   * @returns {string}
   */
  toString(): string {
    return !!this.token ? this.token : '';
  }
}

export class AuthIllegalTokenError extends Error {
  constructor(message: string) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
}

export class AuthIllegalJWTTokenError extends AuthIllegalTokenError {
  constructor(message: string) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
}

export class AuthEmptyTokenError extends AuthIllegalTokenError {
  constructor(message: string) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
}



/**
 * Wrapper for JWT token with additional methods.
 */
export class AuthJWTToken extends AuthSimpleToken {

  static NAME = 'dragonfly:auth:jwt:token';

  /**
   * for JWT token, the iat (issued at) field of the token payload contains the creation Date
   */
  protected prepareCreatedAt(date: Date) {
    const decoded = this.getPayload();
    return decoded && decoded.iat ? new Date(Number(decoded.iat) * 1000) : super.prepareCreatedAt(date);
  }

  /**
   * Returns payload object
   * @returns any
   */
  protected parsePayload(): void {
    if (!this.token) {
      throw new AuthTokenNotFoundError('Token not found. ')
    }
    this.payload = decodeJwtPayload(this.token);
  }

  /**
   * Returns expiration date
   * @returns Date
   */
  getTokenExpDate(): Date {
    const decoded = this.getPayload();
    if (decoded && !decoded.hasOwnProperty('exp')) {
      return null;
    }
    const date = new Date(0);
    date.setUTCSeconds(decoded.exp); // 'cause jwt token are set in seconds
    return date;
  }

  /**
   * Is data expired
   * @returns {boolean}
   */
  isValid(): boolean {
    return super.isValid() && (!this.getTokenExpDate() || new Date() < this.getTokenExpDate());
  }
}


export class AuthTokenNotFoundError extends Error {
  constructor(message: string) {
    super(message);
    Object.setPrototypeOf(this, new.target.prototype);
  }
}

export function decodeJwtPayload(payload: string): any {

  if (payload.length === 0) {
    throw new AuthEmptyTokenError('Cannot extract from an empty payload.');
  }

  const parts = payload.split('.');

  if (parts.length !== 3) {
    throw new AuthIllegalJWTTokenError(
      `The payload ${payload} is not valid JWT payload and must consist of three parts.`);
  }

  let decoded;
  try {
    decoded = urlBase64Decode(parts[1]);
  } catch (e) {
    throw new AuthIllegalJWTTokenError(
      `The payload ${payload} is not valid JWT payload and cannot be parsed.`);
  }

  if (!decoded) {
    throw new AuthIllegalJWTTokenError(
      `The payload ${payload} is not valid JWT payload and cannot be decoded.`);
  }
  return JSON.parse(decoded);
}

export function urlBase64Decode(str: string): string {
  let output = str.replace(/-/g, '+').replace(/_/g, '/');
  switch (output.length % 4) {
    case 0: { break; }
    case 2: { output += '=='; break; }
    case 3: { output += '='; break; }
    default: {
      throw new Error('Illegal base64url string!');
    }
  }
  return b64DecodeUnicode(output);
}

export function b64decode(str: string): string {
  const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';
  let output: string = '';

  str = String(str).replace(/=+$/, '');

  if (str.length % 4 === 1) {
    throw new Error(`'atob' failed: The string to be decoded is not correctly encoded.`);
  }

  for (
    // initialize result and counters
    let bc: number = 0, bs: any, buffer: any, idx: number = 0;
    // get next character
    buffer = str.charAt(idx++);
    // character found in table? initialize bit storage and add its ascii value;
    ~buffer && (bs = bc % 4 ? bs * 64 + buffer : buffer,
      // and if not first of each 4 characters,
      // convert the first 8 bits to one ascii character
      bc++ % 4) ? output += String.fromCharCode(255 & bs >> (-2 * bc & 6)) : 0
  ) {
    // try to find character in table (0-63, not found => -1)
    buffer = chars.indexOf(buffer);
  }
  return output;
}

// https://developer.mozilla.org/en/docs/Web/API/WindowBase64/Base64_encoding_and_decoding#The_Unicode_Problem
export function b64DecodeUnicode(str: any) {
  return decodeURIComponent(Array.prototype.map.call(b64decode(str), (c: any) => {
    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));
}
