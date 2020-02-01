import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOrgProjectIssueComponent } from './view-org-project-issue.component';

describe('ViewOrgProjectIssueComponent', () => {
  let component: ViewOrgProjectIssueComponent;
  let fixture: ComponentFixture<ViewOrgProjectIssueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOrgProjectIssueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOrgProjectIssueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
