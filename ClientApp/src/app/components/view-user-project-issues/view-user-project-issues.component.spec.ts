import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewUserProjectIssuesComponent } from './view-user-project-issues.component';

describe('ViewUserProjectIssuesComponent', () => {
  let component: ViewUserProjectIssuesComponent;
  let fixture: ComponentFixture<ViewUserProjectIssuesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewUserProjectIssuesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewUserProjectIssuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
