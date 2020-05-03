import { AuthIllegalJwtTokenError } from './auth-illegal-jwt-token-error';

describe('AuthIllegalJwtTokenError', () => {
  it('should create an instance', () => {
    expect(new AuthIllegalJwtTokenError()).toBeTruthy();
  });
});
