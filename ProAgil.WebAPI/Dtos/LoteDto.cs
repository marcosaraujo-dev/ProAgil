using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage="O nome deve ser preeenchido")]
        public string Nome { get; set; }
        
        [Required (ErrorMessage="o preço deve ser Preeenchido")]
        public decimal Preco { get; set; }

        [DataType(DataType.Date)]
        [Required (ErrorMessage="A data inicial deve ser preenchida")]
        public string DataInicio { get; set; }
        public string DataFim { get; set; }

        [Range(2, 1200, ErrorMessage ="A quantidade deve ser entre 2 e 1200 pessoas")]
        public int quantidade { get; set; }
    }
}