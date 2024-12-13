using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projeto_tv.Models
{
    [Table("Anuncio")]
    public class AnuncioModel
    {
        [Key]
        public int? Id { get; set; }

        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int TempoTransicao  { get; set; }
        public byte[] Image { get; set; }
        public string LinkImage { get; set; }

        [NotMapped]
        public string QrCode { get; set; }

        [NotMapped]
        public string ImageBase4 { get; set; }
    }
}
