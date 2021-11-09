import { Component, OnInit } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale, ptBrLocale } from 'ngx-bootstrap/chronos';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br',ptBrLocale);
@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  titulo = 'Eventos';
  eventosFiltrados: Evento[] = [];
  eventos: Evento[] = [];
  evento: Evento | any;
  modeloSalvar='post';
  imagemLargura: number = 50;
  imagemMargem: number = 2;
  mostrarImagem: boolean = false;
  registerForm: FormGroup | any;
  bodyDeletarEvento = '';
  file: File | any;

  _filtroLista: string = '';
  fileNameToUpdate: string = '';
  dataAtual: string = '';

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista) : this.eventos;
  }


  constructor(private eventoService: EventoService
    , private modalService: BsModalService
    , private fb: FormBuilder
    , private localeService : BsLocaleService
    , private toastr: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  filtrarEvento(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => {
        return evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1;
      }
    )
  }

  getEventos() {

    this.eventoService.getAllEvento().subscribe(
      (_eventos) => {
        this.eventos = _eventos;
        this.eventosFiltrados = this.eventos;

        this.dataAtual = new Date().getMilliseconds().toString();
      },
      error => {
        this.toastr.error(`Ocorreu erro ao tentar carregar os registros: (${error})`);
       }
    );

  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {

    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      imagemURL: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(1200)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
    // this.registerForm = new FormGroup({
    //   tema: new FormControl('',
    //     [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    //   local: new FormControl('', Validators.required),
    //   dataEvento: new FormControl('', Validators.required),
    //   qtdPessoas: new FormControl('',
    //     [Validators.required, Validators.max(1200)]),
    //   imagemURL: new FormControl('', Validators.required),
    //   telefone: new FormControl('', Validators.required),
    //   email: new FormControl('',
    //     [Validators.required, Validators.email])
    // })
  }


  editarEvento(evento: Evento, template: any){
    this.modeloSalvar = 'put';
    this.openModal(template);
    this.evento = Object.assign({},evento);
    this.fileNameToUpdate = evento.imagemURL.toString();
    this.evento.imagemURL = '';
    this.registerForm.patchValue(this.evento);
  }

  novoEvento(template: any){
    this.modeloSalvar = 'post';
    this.openModal(template);
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Registro deletado com sucesso');
        }, error => {
          this.toastr.error(`Erro ao deletar o registro: (${error})`);

        }
    );
  }

  onFileChange(event: any){
    const reader = new FileReader();
    if (event.target.files && event.target.files.length){
      this.file = event.target.files;
      console.log(this.file);
    }
  }

  uploadImagem() {

    if(this.modeloSalvar === 'post'){
      const nomeArquivo = this.evento.imagemURL.split('\\',3);
      this.evento.imagemURL = nomeArquivo[2];

      this.eventoService.postUpload(this.file, nomeArquivo[2])
      .subscribe(
        () =>{
          this.getEventos();
        }
      );
    }else{
      this.evento.imagemURL = this.fileNameToUpdate;
      this.eventoService.postUpload(this.file, this.fileNameToUpdate)
      .subscribe(
        () =>{
          this.getEventos();
        }
      );
    }
  }

  salvarAlteracao(template: any) {
    if(this.registerForm.valid){
      if(this.modeloSalvar === 'post'){
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem();

        this.eventoService.postEvento(this.evento).subscribe(
          (_novoEvento: Evento) => {
            console.log(_novoEvento);
            template.hide();
            this.getEventos();
            this.toastr.success('Registro gravado com sucesso!');
          }, error => {
            this.toastr.error(`Ocorreu erro ao tentar gravar o registro: (${error})`);

          }

        );
      }else{
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
        this.uploadImagem();
        this.eventoService.putEvento(this.evento).subscribe(
          (_editarEvento: Evento) => {
            console.log(_editarEvento);
            template.hide();
            this.getEventos();
            this.toastr.success('Registro alterado com sucesso!');
          }, error => {
            this.toastr.error(`Ocorreu erro ao tentar editar o registro: (${error})`);
          }

          );
      }
    }

  }

}
