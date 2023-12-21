using FoundParser.Extensions;

namespace FoundParser;

public class FileParser
{
    private static readonly string[] stopWords = { "public", "internal", "private", "logger"};
  
    /// <summary>
    /// Метод принимает путь к файлу и отдает содержимое в виде массива строк
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string[] GetFileContent(string fileName)
    {
        string[] strArray;
        
        if (File.Exists(fileName))
        {
            strArray = File.ReadAllLines(fileName);
        }
        else
        {
            string[] errorMessage = { "Массив не найден"};
            strArray = errorMessage;
        }
        return strArray;
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
        string prepearedStr;
        
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
                
                prepearedStr = strArray[i].Replace('<', ' ').Replace('>', ' ');
                prepearedStr = StringHelpers.DelSubstr(prepearedStr, '(', ')');
                dirList.Add((prepearedStr, currentLevel));
            }
            else
            {
                prepearedStr = strArray[i].Replace('<', ' ').Replace('>', ' ');
                prepearedStr = StringHelpers.DelSubstr(prepearedStr, '(', ')');
                dirList.Add((prepearedStr, currentLevel));
            }
        }
        return result;
    }

    /// <summary>
    /// Метод ищет в файле тип и добавляет его в лист с ссылками
    /// </summary>
    /// <param name="result"></param>
    /// <param name="rootPath"> Путь к папке с файлами</param>
    /// <returns></returns>
    public static List<FilePublisher> AddPublisherTypeByFilePath(List<FilePublisher> result, string rootPath)
    {
        int counter = 0;
        
        for (int i = 0; i < result.Count; i++)
        {
            // массив строк кода для каждого файла
            var strArray2 = GetFileContent(result[i].GetFullPath(rootPath));
            
            for (int j = result[i].LineAddress; j < strArray2.Length; j--)
            {
                //Здесь записывается наша переменная, которая передается в метод

                var searchedVariable = StringHelpers.GetSubstr(strArray2[j], '(', ')');
            
                if (strArray2[j].Contains("new") && strArray2[j].Contains(searchedVariable))
                {
                    //Publish(new Type intent)
                    //Type intent = new
                    //var intent = new Type( - done
                    //Type intent = invoc()
                    //var intent = invoc() // manual?
                    //case  private-public void Method(Type intent) check importance
                    result[i].LinkedClass = StringHelpers.GetClassFromString(strArray2[j]);
                    //Console.WriteLine( result[i].LinkedClass);
                    counter++;
                    break;
                    
                }else if(stopWords.Any(x => strArray2[j].Contains(x)) && strArray2[j].Contains(searchedVariable)) 
                {
                    result[i].LinkedClass = StringHelpers.GetClassFromString(strArray2[j], searchedVariable);
                    //Console.WriteLine(result[i].LinkedClass);
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
            var searchedVariable = result[i].PathElems.Last().Substring(0, result[i].PathElems.Last().Length -3);
     
            for (int j = 0 ; j < strArray2.Length; j++)
            {
                 if (strArray2[j].Contains(searchedVariable))
                 {
                     result[i].LinkedClass = StringHelpers.GetSubstr(strArray2[j], '<','>');
                     Console.WriteLine( result[i].LinkedClass);
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