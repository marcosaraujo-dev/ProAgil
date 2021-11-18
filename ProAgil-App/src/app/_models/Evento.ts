import { Lote } from "./Lote";
import { Palestrante } from "./Palestrante";
import { RedeSocial } from "./RedeSocial";

export class Evento {
  /**
   *
   */
  constructor() {
  }
  id : number = 0;
  local : string = '';
  dataEvento : Date = new Date();
  tema : string = '';
  qtdPessoas : number = 0;
  imagemURL : string = '';
  telefone : string = '';
  email : string = '';
  lotes : Lote [] = [];
  redesSociais : RedeSocial [] = [];
  palestranteEventos : Palestrante [] = [];

}

