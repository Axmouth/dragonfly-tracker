import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOwnProjectComponent } from './create-own-project.component';

describe('CreateOwnProjectComponent', () => {
  let component: CreateOwnProjectComponent;
  let fixture: ComponentFixture<CreateOwnProjectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateOwnProjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOwnProjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
