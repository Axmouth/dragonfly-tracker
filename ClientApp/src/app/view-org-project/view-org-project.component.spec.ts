import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOrgProjectComponent } from './view-org-project.component';

describe('ViewOrgProjectComponent', () => {
  let component: ViewOrgProjectComponent;
  let fixture: ComponentFixture<ViewOrgProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOrgProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOrgProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
