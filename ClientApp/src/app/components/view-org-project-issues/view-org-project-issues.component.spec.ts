import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOrgProjectIssuesComponent } from './view-org-project-issues.component';

describe('ViewOrgProjectIssuesComponent', () => {
  let component: ViewOrgProjectIssuesComponent;
  let fixture: ComponentFixture<ViewOrgProjectIssuesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOrgProjectIssuesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOrgProjectIssuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
