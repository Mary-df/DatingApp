import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TESTDROPDOWNComponent } from './testdropdown.component';

describe('TESTDROPDOWNComponent', () => {
  let component: TESTDROPDOWNComponent;
  let fixture: ComponentFixture<TESTDROPDOWNComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TESTDROPDOWNComponent]
    });
    fixture = TestBed.createComponent(TESTDROPDOWNComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
