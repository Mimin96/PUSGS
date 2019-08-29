import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimetableStationComponent } from './timetable-station.component';

describe('TimetableStationComponent', () => {
  let component: TimetableStationComponent;
  let fixture: ComponentFixture<TimetableStationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimetableStationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimetableStationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
