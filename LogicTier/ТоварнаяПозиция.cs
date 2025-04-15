
using DataTier;


namespace LogicTier
{
    public class ТоварнаяПозиция
    {
        private Товар _товар;
        public Товар Товар => _товар;
        public ТоварнаяПозиция(Товар p)
        {
            _товар = p;
        }
        public string ПредставлениеТовара
        {
            get
            {
                return _товар.Код + ":" + _товар.Наименование
                    + " (" + _товар.Цена.ToString("C") + ")"
                    + " [Группа: " + _товар.Жанр + "]"; 
            }
        }
        public string КодТовара
        {
            get { return _товар.Код; }
            set { _товар.Наименование = value; }
        }
        public string НаименованиеТовара
        {
            get { return _товар.Наименование; }
            set { _товар.Наименование = value; }
        }
        public string Жанр
        {
            get { return _товар.Жанр; }
            set { _товар.Жанр = value; }
        }
        public float ЦенаТовара
        {
            get { return _товар.Цена; }
            set { _товар.Цена = value; }
        }
        public int КоличествоТовара
        {
            get { return _товар.Количество; }
            set { _товар.Количество = value; }
        }
        public float СуммарнаяСтоимостьПозиции
        {
            get { return _товар.Цена * _товар.Количество; }
        }
        
        
    }
}