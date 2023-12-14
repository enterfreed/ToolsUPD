using FoundParser.Extensions;

namespace FoundParser;

public static class Program
{
    public const string filePath = @"D:\Projects\xdfgdfg.txt";   // путь к файлу выгрузки
    public const string rootPath = @"D:\Projects\"; //  корневая директория файлов проекта
    public const string outputFile = "output.csv"; // файл для записи
    
    
    // C:\Users\boris\Desktop\xdfgdfg.txt
    
    private const int SpacesPerLevel = 4;
    private const int FirstLevelTabCount = 2;
    public static void Main()
    {
        var strArray = FileParser.GetFileContent(filePath);
        var result = new List<FilePublisher>();
        
        string searchedSubStr = "Publish("; 
        string searchedExtension = "GpnDs";
        
        var dirList = new List<(string path, int level)>();
        string className;

        for (int i = 0; i < strArray.Length; i++)
        {
            if (!strArray[i].Take(SpacesPerLevel * FirstLevelTabCount).All(x => x == ' '))
            {
                continue;
            }
            
            int currentLevel = (strArray[i].Length - strArray[i].TrimStart().Length)/SpacesPerLevel;
            if (StringHelpers.IsStartsWithNum(strArray[i], out var lineNum))
            {
                result.Add(new FilePublisher()
                {
                    LineAddress = lineNum,
                    PathElems = dirList.Select(x => x.path).ToList(),
                    
                });
            }
           
            else if (dirList.Any() && dirList.Last().level >= currentLevel)
            {
                if (currentLevel == 2)
                {
                    dirList.Clear();
                }
                else
                {
                    for (int j = 0; j <= dirList.Last().level - currentLevel+1; j++)
                    {
                        dirList.RemoveAt(dirList.Count - 1); 
                    }
                }

                dirList.Add((StringHelpers.GetClearedString(strArray[i]), currentLevel));
            }
            else
            {
                dirList.Add((StringHelpers.GetClearedString(strArray[i]), currentLevel));
            }
        }
        
        
        //new start
        
        for (int i = 0; i < result.Count; i++)
        {
            var strArray2 = FileParser.GetFileContent(result[i].GetFullPath(rootPath));

            for (int j = result[i].LineAddress; j < strArray2.Length; j--)
            {
                if (strArray2[j].Contains("new"))
                {
                    result[i].LinkedClass = strArray2[j];
                    break;
                }
            }
        }
        
        // здесь я начинаю готовить лист для записи в файл, вынести в отдельный метод
        
        var resultToFile = new List<string>();
        var title = new List<string>();
        title.Add("Пути господни");
        
        result.Insert(0, new FilePublisher() { PathElems = title, LinkedClass = "Связанный класс" });
        
        foreach (var res in result)
        {
            var linkedClass = res.LinkedClass ?? " "; 
            
            resultToFile. Add($"{res.GetFullPath()} ; {StringHelpers.GetClassFromString(linkedClass)}");
            
        }
        
        
        //  тут записываю
        
        using StreamWriter sw = new StreamWriter(rootPath+outputFile);
           
        foreach (var res in resultToFile)
        {
            sw.WriteLine(string.Join(",", res));
        }
        
    }
}