import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueReactionAreaComponent } from './issue-reaction-area.component';

describe('IssueReactionAreaComponent', () => {
  let component: IssueReactionAreaComponent;
  let fixture: ComponentFixture<IssueReactionAreaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IssueReactionAreaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IssueReactionAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
