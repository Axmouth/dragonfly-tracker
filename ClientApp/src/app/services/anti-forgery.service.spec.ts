import { TestBed } from '@angular/core/testing';

import { AntiForgeryService } from './anti-forgery.service';

describe('AntiForgeryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AntiForgeryService = TestBed.get(AntiForgeryService);
    expect(service).toBeTruthy();
  });
});
