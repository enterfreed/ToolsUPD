using FoundParser.Extensions;

namespace FoundParser;

public static class Program
{
    //public const string filePath = @"D:\Projects\publish.txt"; // путь к файлу выгрузки
    //public const string rootPath = @"D:\Projects\GPN\src\GpnDs.PGA\"; //  корневая директория файлов проекта
    //public const string outputFileDir = @"D:\Projects\"; // куда сохраняем файл
    //public const string outputFileName = "publish.csv"; // имя файла для записи
    
    //офис
    
    public const string filePath = @"C:\Users\boris\Desktop\ParserResult\subscriber.txt"; // путь к файлу выгрузки
    public const string rootPath = @"C:\Projects\secretStaff\src\GpnDs.PGA\"; //  корневая директория файлов проекта
    public const string outputFileDir = @"C:\Users\boris\Desktop\ParserResult\"; // куда сохраняем файл
    public const string outputFileName = "subscriber.csv"; // имя файла для записи
  
    private const int SpacesPerLevel = 4;
    private const int FirstLevelTabCount = 2;

    public static void Main()
    {
        var result = FileParser.ParseExportFile(filePath, SpacesPerLevel, FirstLevelTabCount, StringHelpers.IsSubscriberStartsWithNum);
        
        //result = FileParser.AddPublisherTypeByFilePath(result, rootPath);
        
        result = FileParser.AddSubsciberTypeByFilePath(result, rootPath);
        result = FileParser.AddProjectByFilePath(result);
        ImportManager.SaveToCSV(result, outputFileName, outputFileDir);
    }
}