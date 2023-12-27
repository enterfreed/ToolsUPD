using FoundParser.Extensions;

namespace FoundParser;

public static class Program
{
    private static Dictionary<string, string> excludes = new();
    public const string filePath = @"C:\Users\boris\Desktop\ParserResult\publish.txt"; // путь к файлу выгрузки
    //public const string[] filePath = @"C:\Users\boris\Desktop\ParserResult\publish.txt"; // путь к файлу выгрузки
    public const string rootPath = @"C:\Projects\secretStaff\src\GpnDs.PGA\"; //  корневая директория файлов проекта
    public const string outputFileDir = @"C:\Users\boris\Desktop\ParserResult\"; // куда сохраняем файл
    public const string outputFileName = "publish.csv"; // имя файла для записи
    
    public const bool   isPublish = true; //  флаг для Publish/Subscribe т.к. структура файлов импорта различна
  
    private const int SpacesPerLevel = 4;
    private const int FirstLevelTabCount = 2;

    public static void Main()
    {
        List<FilePublisher> result;
        Init();
        if (isPublish)
        {
            //result add range
            result = FileParser.ParseExportFile(filePath, SpacesPerLevel, FirstLevelTabCount, StringHelpers.IsPublishStartsWithNum);
            
            result = FileParser.AddPublisherTypeByFilePath(result, rootPath, excludes);
            
        }
        else
        {
            result = FileParser.ParseExportFile(filePath, SpacesPerLevel, FirstLevelTabCount, StringHelpers.IsSubscriberStartsWithNum);
            //result = FileParser.AddSubsciberTypeByFilePath(result, rootPath);
            
        }
        //result.Distinct()
        result = FileParser.AddProjectByFilePath(result);
        ImportManager.SaveToCSV(result, outputFileName, outputFileDir);
    }

    private static void Init()
    { 
      excludes.Add("(29: 26)  _destination.Publish(message);", "OnMessageReceived");
      excludes.Add("(93: 30)  commandQueue.Publish(request);", "VspLossRemoveIntent");
      excludes.Add("(308: 27)  _commandQueue.Publish(request);", "FactFundChangeRemoveIntent");
      excludes.Add("(111: 26)  commandQueue.Publish(request);", "FactStartRemoveIntent");
      excludes.Add("(101: 26)  commandQueue.Publish(request);", "FactStopRemoveIntent");
      excludes.Add("(313: 31)  _commandQueue.Publish(request);", "FactStartCrAddIntent");
      excludes.Add("(385: 31)  _commandQueue.Publish(request);", "FactStartCrRemoveIntent");
      excludes.Add("(36: 26)  commandQueue.Publish(request);", "FieldAddIntent");
      excludes.Add("(89: 26)  commandQueue.Publish(request);", "FieldRemoveIntent");
      excludes.Add("(118: 26)  commandQueue.Publish(request);", "FieldAddIntent");
      excludes.Add("(41: 26)  commandQueue.Publish(request);", "SquareObjectAddIntent");
      excludes.Add("(96: 26)  commandQueue.Publish(request);", "SquareObjectRemoveIntent");
      excludes.Add("(130: 26)  commandQueue.Publish(request);", "SquareObjectAddIntent");
      excludes.Add("(54: 26)  commandQueue.Publish(request);", "WellAddIntent");
      excludes.Add("(129: 26)  commandQueue.Publish(request);", "WellRemoveIntent");
      excludes.Add("(168: 26)  commandQueue.Publish(request);", "WellAddIntent");
      excludes.Add("(35: 26)  commandQueue.Publish(request);", "ShopAddIntent");
      excludes.Add("(86: 26)  commandQueue.Publish(request);", "ShopRemoveIntent");
      excludes.Add("(115: 26)  commandQueue.Publish(request);", "ShopAddIntent");
      excludes.Add("(52: 26)  commandQueue.Publish(request);", "WellAddIntent");
      excludes.Add("(106: 26)  commandQueue.Publish(request);", "WellRemoveIntent");
      excludes.Add("(126: 35)  _commandQueue.Publish(removeIntent);", "WellRemoveIntent");
      excludes.Add("(155: 27)  _commandQueue.Publish(addIntent);", "WellAddIntent");
      excludes.Add("(181: 27)  _commandQueue.Publish(intent);", "WellRemoveIntent");
      excludes.Add("(86: 26)  commandQueue.Publish(notificationForAdd);", "InputRateRemoveIntent");
      
      
      
      
      
      
      
      
      
      
      
      
      

      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
      
       
       
      
       
       
       
        
    
    }
}