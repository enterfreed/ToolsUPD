using System.Xml.Serialization;
using SchemaParser.models;

namespace SchemaParser;

public static class Program
{
    private const string newShemePath = @"C:\Projects\tools\Hobbit\CreateDbSchema\result\result.drawio";
    const string originalShemePath = @"C:\Projects\tools\ToolsUPD\SchemaParser\result\result — копия.drawio";
    const string originalShemePath2 = @"C:\Projects\tools\ToolsUPD\SchemaParser\result\result1.drawio";
  
    public static void Main()
    {
        CheckFileSize(originalShemePath);
        CheckFileSize(newShemePath);
        
        var originalSchema = ConvertXMLToObj(newShemePath);  // Заменить после отладки
        var newSchema = ConvertXMLToObj(originalShemePath);   // Заменить после отладки
        
        var difference = CompareSchemas(originalSchema, newSchema);
       
        if (!difference.Any())
        {
            return;
        }
        
        SetDefaultValues(difference);
        
        originalSchema?.Diagram.MxGraphModel.Root.MxCells.AddRange(difference);

        ConvertObjToXML(originalShemePath2,newSchema);
    }
    /// <summary>
    /// Проверка размера файла
    /// </summary>
    /// <param name="filePath">Путь к файлу</param>
    /// <param name="maxFileSizeInBytes"> Допустимый размер файла, по дефолту 10мб</param>
    /// <exception cref="IOException"> Ну такой файл мы читать не хотим</exception>
    private static void CheckFileSize(string filePath, int maxFileSizeInBytes = 104857600)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        
        if (fileInfo.Length > maxFileSizeInBytes)
        {
            throw new IOException("Слишком большой файл для чтения");
        }
    }

    /// <summary>
    /// Сравнение двух схем происходит по сопоставлению ID MxCells
    /// </summary>
    /// <param name="originalSchema"> Исходная схема зашита в проекте</param>
    /// <param name="newSchema"> Новая схема - результаты работы CreateDbSchema</param>
    /// <returns> Возвращаем добавленные элемкенты</returns>
    private static IEnumerable<MxCell> CompareSchemas(MxFile originalSchema,MxFile newSchema)
    {
        var originalMxCells = originalSchema?.Diagram.MxGraphModel.Root.MxCells; 
        var newMxCells = newSchema?.Diagram.MxGraphModel.Root.MxCells;

        Console.WriteLine("Исходная схема: "  + originalMxCells.Count);
        Console.WriteLine("Новая схема: " + newMxCells.Count);
        
        var commonpart = newMxCells.Intersect(originalMxCells, new Helper()); //пересечение (общие элементы)
        
        var difference = newMxCells.Except(commonpart);

        if (!difference.Any())
        {
            Console.WriteLine("Новые элементы не найдены");
            return new List<MxCell>();
        }
        
        Console.WriteLine("Новые элементы: " + difference.Count());

        return difference;
    }
    
    //TODO Поиграться со стилями

    /// <summary>
    /// Дефолтное позиционирование для новых элементов схемы
    /// </summary>
    /// <param name="items"> Элементы схемы</param>
    /// <param name="x"> Смещение по оси абсцисс</param>
    private static void SetDefaultValues(IEnumerable<MxCell> items, decimal x = -1000)
    {
        foreach (var item in items)
        {
            if (item.Parent == "1") // Позиционирование только родительеского элемента
            {
                item.MxGeometry.X = x; 
            }
        }
    }
    /// <summary>
    /// Разбираем документ XML в объект
    /// </summary>
    /// <param name="path"> Путь к XML файлу </param>
    /// <returns> Объект типа данных, соответствующий структуре XML документа</returns>
    private static MxFile ConvertXMLToObj(string path)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MxFile));
        using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
        MxFile? obj = xmlSerializer.Deserialize(fileStream) as MxFile;
        return obj;
    }
  
    /// <summary>
    /// Преобразуем объект в документ XML
    /// </summary>
    /// <param name="path"> Путь к XML файлу </param>
    /// <param name="type"></param>
    private static void ConvertObjToXML(string path, MxFile type)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(MxFile));
        using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
        xmlSerializer.Serialize(fileStream, type);
    }
}