using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTier;

using System.Collections.ObjectModel;
using System.Linq;

namespace LogicTier
{
    public class Магазин
    {
        private ObservableCollection<ТоварнаяПозиция> _товары = new ObservableCollection<ТоварнаяПозиция>();

        public void ДобавитьТоварыИзФайла(List<Товар> товары)
        {
            foreach (var товар in товары)
            {
                _товары.Add(new ТоварнаяПозиция(товар));
            }
        }
        public ObservableCollection<ТоварнаяПозиция> СписокТоваров => _товары;

        public string НаименованиеМагазина => "Наш магазин";

        public float СуммарнаяСтоимость => _товары.Sum(p => p.СуммарнаяСтоимостьПозиции);

        public float СуммарноеКоличество => _товары.Sum(p => p.КоличествоТовара);
    }
}