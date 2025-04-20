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
    private string currentFilePath = null;

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
                currentFilePath = openFileDialog.FileName;  // Сохраняем путь к файлу

                var магазин = (LogicTier.Магазин)DataContext;
                var lines = File.ReadAllLines(currentFilePath);

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

                MessageBox.Show($"Файл успешно импортирован: {currentFilePath}");
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
        // Получаем значения
        string код = TxtCode.Text.Trim();
        string название = TxtName.Text.Trim();
        string ценаStr = TxtPrice.Text.Trim();
        string колвоStr = TxtQuantity.Text.Trim();

        // Проверка жанра
        var selectedItem = (ComboBoxItem)GenreComboBox.SelectedItem;
        if (selectedItem == null || !selectedItem.IsEnabled)
        {
            MessageBox.Show("Пожалуйста, выберите жанр.");
            return;
        }
        string жанр = selectedItem.Content.ToString();

        // Проверка пустых полей
        if (string.IsNullOrWhiteSpace(код) ||
            string.IsNullOrWhiteSpace(название) ||
            string.IsNullOrWhiteSpace(ценаStr) ||
            string.IsNullOrWhiteSpace(колвоStr))
        {
            MessageBox.Show("Пожалуйста, заполните все поля.");
            return;
        }

        // Проверка корректности цены
        if (!float.TryParse(ценаStr, out float цена) || цена < 0)
        {
            MessageBox.Show("Цена должна быть положительным числом.");
            return;
        }

        // Проверка корректности количества
        if (!int.TryParse(колвоStr, out int количество) || количество < 0)
        {
            MessageBox.Show("Количество должно быть положительным целым числом.");
            return;
        }

        // Создание товара
        var товар = new DataTier.Товар()
        {
            Код = код,
            Наименование = название,
            Жанр = жанр,
            Цена = цена,
            Количество = количество
        };

        // Добавление в список
        var магазин = (LogicTier.Магазин)DataContext;
        магазин.СписокТоваров.Add(new LogicTier.ТоварнаяПозиция(товар));

        // Очистка
        TxtCode.Clear();
        TxtName.Clear();
        GenreComboBox.SelectedIndex = 0;
        TxtPrice.Clear();
        TxtQuantity.Clear();
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

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(currentFilePath))
        {
            MessageBox.Show("Файл не выбран. Сначала используйте 'Импорт' для выбора файла.");
            return;
        }

        try
        {
            var магазин = (LogicTier.Магазин)DataContext;

            using (StreamWriter sw = new StreamWriter(currentFilePath))
            {
                foreach (var товарнаяПозиция in магазин.СписокТоваров)
                {
                    var товар = товарнаяПозиция.Товар;
                    sw.WriteLine($"{товар.Код} | {товар.Наименование} | {товар.Жанр} | {товар.Цена} | {товар.Количество}");
                }
            }

            MessageBox.Show($"Файл успешно сохранён: {currentFilePath}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении файла:\n{ex.Message}");
        }
    }
}