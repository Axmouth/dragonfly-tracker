import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerOnlyComponent } from './server-only.component';

describe('ServerOnlyComponent', () => {
  let component: ServerOnlyComponent;
  let fixture: ComponentFixture<ServerOnlyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerOnlyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerOnlyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
