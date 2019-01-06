import { TestBed } from '@angular/core/testing';

import { RdfDataService } from './rdf-data.service';

describe('RdfDataService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RdfDataService = TestBed.get(RdfDataService);
    expect(service).toBeTruthy();
  });
});
