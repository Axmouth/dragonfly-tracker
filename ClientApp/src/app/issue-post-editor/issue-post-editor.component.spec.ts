import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IssuePostEditorComponent } from './issue-post-editor.component';

describe('IssuePostEditorComponent', () => {
  let component: IssuePostEditorComponent;
  let fixture: ComponentFixture<IssuePostEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IssuePostEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IssuePostEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
