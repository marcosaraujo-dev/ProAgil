import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventoEditComponent } from './eventoEdit.component';

describe('EventoEditComponent', () => {
  let component: EventoEditComponent;
  let fixture: ComponentFixture<EventoEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventoEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventoEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
