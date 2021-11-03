import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[] = [];
  eventos: Evento[] = [];
  imagemLargura: number = 50;
  imagemMargem: number = 2;
  mostrarImagem: boolean = false;
  modalRef: BsModalRef | any;

  _filtroLista: string = "";

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string){
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista): this.eventos;
  }


  constructor(private eventoService: EventoService
            , private modalService: BsModalService
    ) { }


  //Metodos para criar a modal
  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    this.getEventos();
  }

  filtrarEvento(filtrarPor: string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento : any) => {
        return evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1;
      }
    )
  }

  getEventos(){
    this.eventoService.getAllEvento().subscribe(
      (_eventos) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;
      },
      error => {console.log(error)}
    );

  }

  alternarImagem(){
    this.mostrarImagem = ! this.mostrarImagem;
  }

}
