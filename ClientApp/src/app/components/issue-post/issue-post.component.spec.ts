import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IssuePostComponent } from './issue-post.component';

describe('IssuePostComponent', () => {
  let component: IssuePostComponent;
  let fixture: ComponentFixture<IssuePostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IssuePostComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IssuePostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
