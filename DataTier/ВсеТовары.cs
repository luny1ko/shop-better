using DataTier;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class ВсеТовары
{
    //public static List<Товар> ПолучитьВсеТоварыИзФайла()
    //{
    //    List<Товар> товары = new List<Товар>();
    //    string[] lines = File.ReadAllLines("товары.txt"); // Путь к файлу

    //    foreach (var line in lines)
    //    {
    //        if (string.IsNullOrWhiteSpace(line)) continue;

    //        var parts = line.Split('\\');
    //        if (parts.Length < 5)
    //        {
    //            continue; // Пропустить некорректные строки
    //        }

    //        товары.Add(new Товар
    //        {
    //            Наименование = parts[0].Trim(),
    //            Группа = parts[1].Trim(),
    //            Цена = float.Parse(parts[2].Trim()),
    //            ПроцентСкидки = float.Parse(parts[3].Trim()),
    //            Код = parts[0].Trim(),
    //            Количество = 1
    //        });
    //    }

    //    return товары;
    //}

   
}