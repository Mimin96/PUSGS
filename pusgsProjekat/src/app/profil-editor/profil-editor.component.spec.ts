import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfilEditorComponent } from './profil-editor.component';

describe('ProfilEditorComponent', () => {
  let component: ProfilEditorComponent;
  let fixture: ComponentFixture<ProfilEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProfilEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfilEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
