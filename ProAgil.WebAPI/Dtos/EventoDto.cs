using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        
        [Required (ErrorMessage="Campo Obrigatório")]
        [StringLength (100, MinimumLength=3, ErrorMessage="Local deve conter entre 3 e 100 Caracteres")]
        public string Local { get; set; } 
        public string DataEvento { get; set; }

        [Required (ErrorMessage="O Tema deve ser Preeenchido")]
        public string Tema { get; set; } 

        [Range(2, 1200, ErrorMessage="A quantidade de pessoas deve ser entre 2 e 1200")]
        public int QtdPessoas { get; set; }        
        
        public string ImagemURL { get; set; } 
        
        public string Telefone { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }        

        public List<LoteDto> Lotes { get; set; } 
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}