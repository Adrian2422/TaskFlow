import { TestBed } from '@angular/core/testing';

import { BreadcrumbStore } from './breadcrumb-store';

describe('BreadcrumbStore', () => {
  let store: BreadcrumbStore;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    store = TestBed.inject(BreadcrumbStore);
  });

  it('should be created', () => {
    expect(store).toBeTruthy();
  });
});
