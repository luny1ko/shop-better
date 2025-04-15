using DataTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTier
{
    public static class АнализТоваров
    {
        
        public static Dictionary<string, float> ПолучитьМинимальныеЦеныПоЖанрам(List<Товар> товары)
        {
            return товары
                .GroupBy(t => t.Жанр)
                .ToDictionary(
                    g => g.Key,
                    g => g.Min(t => t.Цена)
                );
        }

        public static Dictionary<string, float> ПолучитьСреднююЦенуПоЖанрам(List<Товар> товары)
        {
            return товары
                .GroupBy(t => t.Жанр)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(t => t.Цена)
                );
        }

        public static Dictionary<string, string> ПолучитьСамуюДешевуюКнигуПоЖанрам(List<Товар> товары)
        {
            return товары
                .GroupBy(t => t.Жанр)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        var cheapestBook = g.MinBy(t => t.Цена);
                        return cheapestBook != null
                            ? $"{cheapestBook.Наименование} (Цена: {cheapestBook.Цена})"
                            : "Нет товаров";
                    }
                );
        }
    }
}