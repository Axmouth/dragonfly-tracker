import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewWithLoadingStateComponent } from './view-with-loading-state.component';

describe('ViewWithLoadingStateComponent', () => {
  let component: ViewWithLoadingStateComponent;
  let fixture: ComponentFixture<ViewWithLoadingStateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewWithLoadingStateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewWithLoadingStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
