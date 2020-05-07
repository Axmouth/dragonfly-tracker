import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateProjectWizardComponent } from './update-project-wizard.component';

describe('UpdateProjectWizardComponent', () => {
  let component: UpdateProjectWizardComponent;
  let fixture: ComponentFixture<UpdateProjectWizardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateProjectWizardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateProjectWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
