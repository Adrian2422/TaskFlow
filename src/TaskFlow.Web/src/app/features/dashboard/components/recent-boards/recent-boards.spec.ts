import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecentBoards } from './recent-boards';

describe('RecentBoards', () => {
  let component: RecentBoards;
  let fixture: ComponentFixture<RecentBoards>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RecentBoards],
    }).compileComponents();

    fixture = TestBed.createComponent(RecentBoards);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
