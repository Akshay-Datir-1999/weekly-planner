import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { leadGuard } from './lead-guard';

describe('leadGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => leadGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
