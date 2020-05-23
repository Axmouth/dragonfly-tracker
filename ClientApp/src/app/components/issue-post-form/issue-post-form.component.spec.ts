import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IssuePostFormComponent } from './issue-post-form.component';

describe('IssuePostFormComponent', () => {
  let component: IssuePostFormComponent;
  let fixture: ComponentFixture<IssuePostFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IssuePostFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IssuePostFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
