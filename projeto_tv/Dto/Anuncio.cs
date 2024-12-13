namespace projeto_tv.Dto
{
    public class Anuncio
    {
        public int? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int TempoTransicao { get; set; }
        public byte[] Image { get; set; }
        public string LinkImage { get; set; }
        public string QrCode { get; set; }
        public string ImageBase4 { get; set; }
    }
}
