import { AuthTokenNotFoundError } from './auth-token-not-found-error';
import { AuthToken } from './auth-token';

/**
 * Wrapper for simple (text) token
 */
export class AuthSimpleToken extends AuthToken {
  static NAME = 'dragonfly:auth:simple:token';

  constructor(protected readonly token: any, protected readonly ownerStrategyName: string, protected createdAt?: Date) {
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
