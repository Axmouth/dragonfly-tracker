import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewMyProjectsComponent } from './view-my-projects.component';

describe('ViewMyProjectsComponent', () => {
  let component: ViewMyProjectsComponent;
  let fixture: ComponentFixture<ViewMyProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewMyProjectsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewMyProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
