import { TestBed } from '@angular/core/testing';

import { WeekCycle } from './week-cycle';

describe('WeekCycle', () => {
  let service: WeekCycle;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WeekCycle);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
