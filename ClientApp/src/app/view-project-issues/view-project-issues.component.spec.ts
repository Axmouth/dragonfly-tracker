import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProjectIssuesComponent } from './view-project-issues.component';

describe('ViewProjectIssuesComponent', () => {
  let component: ViewProjectIssuesComponent;
  let fixture: ComponentFixture<ViewProjectIssuesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewProjectIssuesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewProjectIssuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
