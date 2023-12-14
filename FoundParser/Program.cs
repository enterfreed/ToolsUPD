namespace FoundParser;

public static class Program
{
    public const string filePath = @"D:\Projects\publish.txt"; // путь к файлу выгрузки
    public const string rootPath = @"D:\Projects\GPN\src\GpnDs.PGA\"; //  корневая директория файлов проекта
    public const string outputFileDir = @"D:\Projects\"; // куда сохраняем файл
    public const string outputFileName = "publish.csv"; // имя файла для записи


    // C:\Users\boris\Desktop\xdfgdfg.txt

    private const int SpacesPerLevel = 4;
    private const int FirstLevelTabCount = 2;

    public static void Main()
    {
        var result = FileParser.ParseExportFile(filePath, SpacesPerLevel, FirstLevelTabCount);
        result = FileParser.AddTypeByFilePath(result, rootPath);
        ImportManager.SaveToCSV(result, outputFileName, outputFileDir);
    }
}