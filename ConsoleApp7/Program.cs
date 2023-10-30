using System;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;

public class Figure
{
    public string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Program
{
    private static string filePath; // Переменная для хранения пути к файлу
    private static Figure figure = new Figure(); // Создание объекта класса Figure

    public static void Main(string[] args)
    {
        Console.WriteLine("Текстовый редактор");
        Console.Write("Введите путь к файлу: ");
        filePath = Console.ReadLine(); // Получение пути к файлу от пользователя

        if (!File.Exists(filePath)) // Проверка существования файла
        {
            Console.WriteLine("Файл не существует. Создать новый? (Y/N)");
            var createNew = Console.ReadKey();
            if (createNew.Key == ConsoleKey.Y)
            {
                CreateNewFile(); // Создание нового файла, если он не существует
            }
            else
            {
                Environment.Exit(0); // Выход из программы, если пользователь отказывается создавать новый файл
            }
        }

        LoadFile(); // Загрузка данных из файла
        DisplayMenu(); // Отображение меню для пользователя
    }

    private static void CreateNewFile()
    {
        Console.Write("Введите название фигуры: ");
        figure.Name = Console.ReadLine(); // Получение названия фигуры от пользователя

        Console.Write("Введите ширину: ");
        if (double.TryParse(Console.ReadLine(), out double width))
        {
            figure.Width = width; // Получение ширины фигуры от пользователя
        }
        else
        {
            Console.WriteLine("Ошибка при вводе ширины.");
            Environment.Exit(0); // Завершение программы при ошибке ввода ширины
        }

        Console.Write("Введите высоту: ");
        if (double.TryParse(Console.ReadLine(), out double height))
        {
            figure.Height = height; // Получение высоты фигуры от пользователя
        }
        else
        {
            Console.WriteLine("Ошибка при вводе высоты.");
            Environment.Exit(0); // Завершение программы при ошибке ввода высоты
        }

        SaveFile(); // Сохранение данных в файл
    }

    private static void LoadFile()
    {
        string fileExtension = Path.GetExtension(filePath).ToLower(); // Получение расширения файла

        // Проверка формата файла и загрузка данных из него соответственно
        if (fileExtension == ".txt")
        {
            string[] lines = File.ReadAllLines(filePath); // Чтение строк из файла
            if (lines.Length == 3)
            {
                figure.Name = lines[0]; // Загрузка названия фигуры
                if (double.TryParse(lines[1], out double width))
                {
                    figure.Width = width; // Загрузка ширины фигуры
                }
                if (double.TryParse(lines[2], out double height))
                {
                    figure.Height = height; // Загрузка высоты фигуры
                }
            }
        }
        else if (fileExtension == ".json")
        {
            string json = File.ReadAllText(filePath); // Чтение JSON из файла
            figure = JsonSerializer.Deserialize<Figure>(json); // Десериализация JSON в объект Figure
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure)); // Создание объекта XmlSerializer
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                figure = (Figure)serializer.Deserialize(fileStream); // Десериализация XML в объект Figure
            }
        }
    }

    private static void SaveFile()
    {
        string fileExtension = Path.GetExtension(filePath).ToLower(); // Получение расширения файла

        // Проверка формата файла и сохранение данных в него соответственно
        if (fileExtension == ".txt")
        {
            File.WriteAllText(filePath, $"{figure.Name}\n{figure.Width}\n{figure.Height}"); // Запись данных в текстовый файл
        }
        else if (fileExtension == ".json")
        {
            string json = JsonSerializer.Serialize(figure); // Сериализация объекта Figure в JSON
            File.WriteAllText(filePath, json); // Запись JSON в файл
        }
        else if (fileExtension == ".xml")
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure)); // Создание объекта XmlSerializer
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, figure); // Сериализация объекта Figure в XML и запись в файл
            }
        }
    }

    private static void DisplayMenu()
    {
        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Console.WriteLine("Текстовый редактор");
            Console.WriteLine("Название: " + figure.Name);
            Console.WriteLine("Ширина: " + figure.Width);
            Console.WriteLine("Высота: " + figure.Height);
            Console.WriteLine("F1 - Сохранить, Esc - Выйти");

            key = Console.ReadKey();

            if (key.Key == ConsoleKey.F1)
            {
                SaveFile(); // Сохранение данных при нажатии F1
                Console.WriteLine("Файл сохранен.");
            }
        } while (key.Key != ConsoleKey.Escape); // Выход из программы при нажатии Escape
    }
}