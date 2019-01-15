import { TestBed } from '@angular/core/testing';

import { GraphDataStateService } from './graph-data-state.service';

describe('GraphDataStateService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GraphDataStateService = TestBed.get(GraphDataStateService);
    expect(service).toBeTruthy();
  });
});
