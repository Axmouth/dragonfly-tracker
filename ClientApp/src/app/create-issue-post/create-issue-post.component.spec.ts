import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateIssuePostComponent } from './create-issue-post.component';

describe('CreateIssuePostComponent', () => {
  let component: CreateIssuePostComponent;
  let fixture: ComponentFixture<CreateIssuePostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateIssuePostComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateIssuePostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
