using FoundParser.Extensions;

namespace FoundParser;

public static class Program
{
    public const string filePath = @"C:\Users\boris\Desktop\ParserResult\publish.txt"; // путь к файлу выгрузки
    public const string rootPath = @"C:\Projects\secretStaff\src\GpnDs.PGA\"; //  корневая директория файлов проекта
    public const string outputFileDir = @"C:\Users\boris\Desktop\ParserResult\"; // куда сохраняем файл
    public const string outputFileName = "publish.csv"; // имя файла для записи
    public const bool   isPublish = true; //  флаг для Publish/Subscribe т.к. структура файлов импорта различна
  
    private const int SpacesPerLevel = 4;
    private const int FirstLevelTabCount = 2;

    public static void Main()
    {
        List<FilePublisher> result;
        
        if (isPublish)
        {
            result = FileParser.ParseExportFile(filePath, SpacesPerLevel, FirstLevelTabCount, StringHelpers.IsPublishStartsWithNum);
            result = FileParser.AddPublisherTypeByFilePath(result, rootPath);
            
        }
        else
        {
            result = FileParser.ParseExportFile(filePath, SpacesPerLevel, FirstLevelTabCount, StringHelpers.IsSubscriberStartsWithNum);
            result = FileParser.AddSubsciberTypeByFilePath(result, rootPath);
            
        }
        result = FileParser.AddProjectByFilePath(result);
        ImportManager.SaveToCSV(result, outputFileName, outputFileDir);
    }
}