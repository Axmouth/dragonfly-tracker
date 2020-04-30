import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewUserProjectComponent } from './view-user-project.component';

describe('ViewUserProjectComponent', () => {
  let component: ViewUserProjectComponent;
  let fixture: ComponentFixture<ViewUserProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewUserProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewUserProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
