using System.Text.Json.Serialization;

namespace PasteleriaMaui.Models
{
    public class CatCategoria
    {
        public int Id { get; set; }
        public string categoria { get; set; } = string.Empty;

        [JsonIgnore]
        public string Categoria
        {
            get => categoria;
            set => categoria = value;
        }

        [JsonIgnore]
        public string Nombre
        {
            get => categoria;
            set => categoria = value;
        }

        public override string ToString() => !string.IsNullOrEmpty(categoria) ? categoria : (Nombre ?? string.Empty);
    }
}
