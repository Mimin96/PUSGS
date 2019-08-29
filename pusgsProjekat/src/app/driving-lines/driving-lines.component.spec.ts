import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrivingLinesComponent } from './driving-lines.component';

describe('DrivingLinesComponent', () => {
  let component: DrivingLinesComponent;
  let fixture: ComponentFixture<DrivingLinesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DrivingLinesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrivingLinesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
