import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BrowserOnlyComponent } from './browser-only.component';

describe('BrowserOnlyComponent', () => {
  let component: BrowserOnlyComponent;
  let fixture: ComponentFixture<BrowserOnlyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BrowserOnlyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BrowserOnlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
