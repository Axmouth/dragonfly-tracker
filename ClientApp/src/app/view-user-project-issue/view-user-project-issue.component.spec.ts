import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewUserProjectIssueComponent } from './view-user-project-issue.component';

describe('ViewUserProjectIssueComponent', () => {
  let component: ViewUserProjectIssueComponent;
  let fixture: ComponentFixture<ViewUserProjectIssueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewUserProjectIssueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewUserProjectIssueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
