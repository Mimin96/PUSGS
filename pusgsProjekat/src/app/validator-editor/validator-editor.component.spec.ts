import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidatorEditorComponent } from './validator-editor.component';

describe('ValidatorEditorComponent', () => {
  let component: ValidatorEditorComponent;
  let fixture: ComponentFixture<ValidatorEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidatorEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidatorEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
