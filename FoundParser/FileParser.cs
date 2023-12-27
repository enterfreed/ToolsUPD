using FoundParser.Extensions;

namespace FoundParser;

public class FileParser
{
    private static readonly string[] stopWords = { "public", "internal", "private", "logger"};
    
    private static string[] GetFileContent(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл '{filePath}' не найден");
        }

        CheckFileSize(filePath, 104857600); // 10mb
        
        return File.ReadAllLines(filePath);
    }
    
    private static void CheckFileSize(string filePath, int maxFileSizeInBytes)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        
        if (fileInfo.Length > maxFileSizeInBytes)
        {
            throw new IOException("Слишком большой файл для чтения");
        }
    }

    /// <summary>
    /// Метод парсит файл и возвращает лист с сыллками на файлы, где ипользуется Publish. Возвращает номера строк
    /// в этих файлах 
    /// </summary>
    /// <param name="filePath"> Путь к файлу экспорта</param>
    /// <param name="SpacesPerLevel"> Количество пустых символов на уровень</param>
    /// <param name="FirstLevelTabCount"> Количество уровней для начального уровня работы скрипта</param>
    /// <returns></returns>
    public static List<FilePublisher> ParseExportFile(string filePath, int SpacesPerLevel, int FirstLevelTabCount, Func<string, (bool isNum, int lineNum)> lineChecker)
    {
        var result = new List<FilePublisher>();
        var dirList = new List<(string path, int level)>();
       
        var strArray = GetFileContent(filePath);
        for (int i = 0; i < strArray.Length; i++)
        {
            if (!strArray[i].Take(SpacesPerLevel * FirstLevelTabCount).All(x => x == ' '))
            {
                continue;
            }

            int currentLevel = (strArray[i].Length - strArray[i].TrimStart().Length) / SpacesPerLevel;

            var checker = lineChecker(strArray[i]);
           
            if (checker.isNum)
            {
                
                result.Add(new FilePublisher()
                {
                    LineAddress = checker.lineNum,
                    PathElems = dirList.Select(x => x.path).ToList(),
                    DefaultSting = strArray[i].Any(x => x == ':') ? strArray[i].Trim(): null,
                    //LinkedClass = StringHelpers.GetSubstr(strArray[i], '<', '>') , // Для subscribe у нас уже на этом этапе есть связанный класс
                    
                });
            }
            else if (dirList.Where(x => true).Any() && dirList.Last().level >= currentLevel)
            {
                if (currentLevel == 2)
                {
                    dirList.Clear();
                }
                else
                {
                    var linesToRemove = dirList.Last().level - currentLevel;
                    for (int j = 0; j < linesToRemove + 1; j++)
                    {
                        dirList.RemoveAt(dirList.Count - 1);
                    }
                }
                
                dirList.Add((StringHelpers.GetStringWithoutBrackets(strArray[i]), currentLevel));
            }
            else
            {
                dirList.Add((StringHelpers.GetStringWithoutBrackets(strArray[i]), currentLevel));
            }
        }
        return result;
    }

    /// <summary>
    /// Метод ищет в файле тип и добавляет его в лист с ссылками
    /// </summary>
    /// <param name="result"></param>
    /// <param name="rootPath"> Путь к папке с файлами</param>
    /// <param name="excludes">исключения</param>
    /// <returns></returns>
    public static List<FilePublisher> AddPublisherTypeByFilePath(List<FilePublisher> result, string rootPath, Dictionary<string, string> excludes)
    {
        int counter = 0;
        
        for (int i = 0; i < result.Count; i++)
        {
            
            // массив строк кода для каждого файла
            var fullPath = result[i].GetFullPath(rootPath);

           //Console.WriteLine(result[i].DefaultSting.Trim());
          
            if (excludes.ContainsKey(result[i].DefaultSting))
            {
                result[i].LinkedClass = excludes[result[i].DefaultSting];
                continue;
            }
            var strArray = GetFileContent(fullPath);
            
            for (int j = result[i].LineAddress; j < strArray.Length; j--)
            {
                if (strArray[j].Contains("_logger"))
                {
                    continue;
                }

                var searchedVariable = StringHelpers.GetSubstr(strArray[j], '(', ')');
            
                if (strArray[j].Contains("new") && strArray[j].Contains(searchedVariable))
                {
                    result[i].LinkedClass = StringHelpers.GetClassFromString(strArray[j]);
                    counter++;
                    break;
                    
                }else if(stopWords.Any(x => strArray[j].Contains(x)) && strArray[j].Contains(searchedVariable)) 
                {
                    result[i].LinkedClass = StringHelpers.GetClassFromString(strArray[j], searchedVariable);
                    counter++; 
                    break;
                }
            }
        }

        var parsePercent = Math.Round((double)(counter*100/result.Count));
        Console.WriteLine($"Найдено типов: {counter} - {parsePercent} %");
        
        return result;
    }
    
    public static List<FilePublisher> AddProjectByFilePath(List<FilePublisher> result)
    {
        for (int i = 0; i < result.Count; i++)
        {
            string line =result[i].PathElems.First();
            line = line.Substring(line.LastIndexOf(@"PGA.") + 4);
            result[i].Project = line;
        }
        return result;
    }

    public static List<FilePublisher> AddSubsciberTypeByFilePath(List<FilePublisher> result, string rootPath)
    {
        int counter = 0;
     
        for (int i = 0; i < result.Count; i++)
        {
            var strArray2 = GetFileContent(result[i].GetFullPath(rootPath));
            var searchedVariable = result[i].PathElems.Last().Trim().Substring(0, result[i].PathElems.Last().Length -3);
     
            for (int j = 0 ; j < strArray2.Length; j++)
            {
                 if (strArray2[j].Contains(searchedVariable))
                 {
                     if (strArray2[j].Contains('<') && strArray2[j].Contains('>'))
                     {
                         result[i].LinkedClass = StringHelpers.GetSubstr(strArray2[j], '<','>');
                     } else
                     {
                         result[i].LinkedClass = strArray2[j];
                     }
                     counter++;
                     break;
                 }
            }
        }

       var parsePercent = Math.Round((double)(counter*100/result.Count));
       Console.WriteLine($"Найдено типов: {counter} - {parsePercent} %");
        
        return result;
    }
}