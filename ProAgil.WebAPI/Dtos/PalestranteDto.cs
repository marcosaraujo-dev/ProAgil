using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class PalestranteDto
    {
        public int Id { get; set; }
        
        [Required (ErrorMessage="O Campo {0} é Obrigatório")]
        public string Nome { get; set; }
        public string miniCurriculo { get; set; }
        
        public string ImagemURL { get; set; }
        
        [Phone]
        public string Telefone { get; set; }

        [Required (ErrorMessage="O Campo {0} é Obrigatório")]
        [EmailAddress (ErrorMessage ="O email preenchido é inválido")]
        public string Email { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<EventoDto> Eventos { get; set; }
    }
}