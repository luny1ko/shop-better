
namespace DataTier
{
    public class Товар
    {
        public string Код { get; set; }
        public string Наименование { get; set; }
        public float Цена { get; set; }
        public float МинимальнаяЦенаВЖанре { get; set; } 
        public int Количество { get; set; }
        public string Жанр { get; set; }
    }

}