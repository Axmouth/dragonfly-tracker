import { TestBed } from '@angular/core/testing';

import { PrepareTokensService } from './prepare-tokens.service';

describe('PrepareTokensService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PrepareTokensService = TestBed.get(PrepareTokensService);
    expect(service).toBeTruthy();
  });
});
