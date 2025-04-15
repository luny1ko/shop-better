using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataTier;
using LogicTier;
using Microsoft.Win32;

namespace PresentationTier;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObservableCollection<string> items = new ObservableCollection<string>();
    public MainWindow()
    {
        Магазин LogicTier = new Магазин();
        this.DataContext = LogicTier;
        DataContext = new LogicTier.Магазин();
        InitializeComponent();
    }
    private void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

        try
        {
            if (openFileDialog.ShowDialog() == true)
            {
                var магазин = (LogicTier.Магазин)DataContext;
                var lines = File.ReadAllLines(openFileDialog.FileName);

                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(p => p.Trim())
                                    .ToArray();

                    if (parts.Length != 5)
                    {
                        MessageBox.Show($"Ошибка в строке: {line}\nНеверный формат данных.");
                        continue;
                    }

                    var товар = new DataTier.Товар()
                    {
                        Наименование = parts[1],
                        Жанр = parts[2],
                        Цена = float.Parse(parts[3]),
                        Количество = int.Parse(parts[4]),
                        Код = parts[0],
                        
                    };

                    магазин.СписокТоваров.Add(new LogicTier.ТоварнаяПозиция(товар));
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка импорта: {ex.Message}");
        }
    }
    private void CalculateButton_Click(object sender, RoutedEventArgs e)
    {
        var магазин = (LogicTier.Магазин)DataContext;
        var товары = магазин.СписокТоваров.Select(tp => tp.Товар).ToList();
        var результаты = LogicTier.АнализТоваров.ПолучитьСреднююЦенуПоЖанрам(товары);

        MessageBox.Show(
            string.Join("\n", результаты.Select(kvp => $"{kvp.Key}: {kvp.Value:C}")),
            "Результаты по Жанрам"
        );
    }

    

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
        var магазин = (LogicTier.Магазин)DataContext;
        var товары = магазин.СписокТоваров.Select(tp => tp.Товар).ToList();
        var результаты = LogicTier.АнализТоваров.ПолучитьСамуюДешевуюКнигуПоЖанрам(товары);

        MessageBox.Show(
            string.Join("\n", результаты.Select(kvp => $"{kvp.Key}: {kvp.Value:C}")),
            "Результаты по Жанрам"
        );
    }

    private void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var товар = new DataTier.Товар
            {
                Код = TxtCode.Text,
                Наименование = TxtName.Text,
                Жанр = TxtGenre.Text,
                Цена = float.Parse(TxtPrice.Text),
                ПроцентСкидки = float.Parse(TxtQuantity.Text),
                Количество = 1
            };

            var позиция = new LogicTier.ТоварнаяПозиция(товар); // оборачиваем

            var магазин = (LogicTier.Магазин)DataContext;
            магазин.СписокТоваров.Add(позиция); // добавляем в список

            TxtCode.Clear();
            TxtName.Clear();
            TxtGenre.Clear();
            TxtPrice.Clear();
            TxtQuantity.Clear();

        }
        catch (Exception ex)
        {
            MessageBox.Show("Ошибка при добавлении товара: " + ex.Message, "Ошибка");
        }
    }

    private void BtnCreateFile_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "Текстовые файлы(*.txt)|*.txt|Скрипты(*.sql)|*.sql|Документы(*.docx)|*.docx";

        if (sfd.ShowDialog() == true)
        {
            string FilePath = sfd.FileName;
            using (StreamWriter swriter = new StreamWriter(FilePath))
            {
                var магазин = (LogicTier.Магазин)DataContext;

                int номер = 1;
                foreach (var товар in магазин.СписокТоваров)
                {
                    //форматируем строку в нужный вид
                    string строка = $"{номер:000} | {товар.НаименованиеТовара} | {товар.Жанр} | {товар.ЦенаТовара:0.00} | {товар.КоличествоТовара}";
                    swriter.WriteLine(строка);
                    номер++;
                }
            }

            MessageBox.Show($"Данные сохранены в файл {FilePath}");
        }
        else
        {
            MessageBox.Show("Пользователь отказался от окна сохранения");
        }
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (MainList.SelectedItems.Count == 0)
        {
            MessageBox.Show("Пожалуйста, выделите элементы для удаления.", "Предупреждение");
            return;
        }
        //получаем объект Магазин из DataContext 
        var магазин = (LogicTier.Магазин)DataContext;
        //преобразуем выбранные элементы в список ТоварнаяПозиация
        var itemsToRemove = MainList.SelectedItems.Cast<LogicTier.ТоварнаяПозиция>().ToList(); //приводим элементы к типу ТоварнаяПозиация

        foreach (var item in itemsToRemove)
        {
            магазин.СписокТоваров.Remove(item); //удаляем из коллекции ObservableCollection
        }
    }
}