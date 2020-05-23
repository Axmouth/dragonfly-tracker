import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TosDialogComponent } from './tos-dialog.component';

describe('TosDialogComponent', () => {
  let component: TosDialogComponent;
  let fixture: ComponentFixture<TosDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TosDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TosDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
