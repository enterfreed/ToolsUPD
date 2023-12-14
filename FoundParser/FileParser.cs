using FoundParser.Extensions;

namespace FoundParser;

public class FileParser
{
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
            strArray = Array.Empty<string>();
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
    public static List<FilePublisher> ParseExportFile(string filePath, int SpacesPerLevel, int FirstLevelTabCount)
    {
        var result = new List<FilePublisher>();

        var strArray = GetFileContent(filePath);

        string searchedSubStr = "Publish(";
        string searchedExtension = "GpnDs";

        var dirList = new List<(string path, int level)>();
        string prepearedStr;

        for (int i = 0; i < strArray.Length; i++)
        {
            if (!strArray[i].Take(SpacesPerLevel * FirstLevelTabCount).All(x => x == ' '))
            {
                continue;
            }

            int currentLevel = (strArray[i].Length - strArray[i].TrimStart().Length) / SpacesPerLevel;
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
                    for (int j = 0; j <= dirList.Last().level - currentLevel + 1; j++)
                    {
                        dirList.RemoveAt(dirList.Count - 1);
                    }
                }

                prepearedStr = strArray[i].Replace('<', ' ').Replace('>', ' ');
                prepearedStr = StringHelpers.DelSubstr(prepearedStr, "(", ")");
                dirList.Add((prepearedStr, currentLevel));
            }
            else
            {
                prepearedStr = strArray[i].Replace('<', ' ').Replace('>', ' ');
                prepearedStr = StringHelpers.DelSubstr(prepearedStr, "(", ")");
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
    public static List<FilePublisher> AddTypeByFilePath(List<FilePublisher> result, string rootPath)
    {
        for (int i = 0; i < result.Count; i++)
        {
            var strArray2 = GetFileContent(result[i].GetFullPath(rootPath));

            for (int j = result[i].LineAddress; j < strArray2.Length; j--)
            {
                if (strArray2[j].Contains("public") || strArray2[j].Contains("private"))
                {
                    break;
                }

                if (strArray2[j].Contains("new"))
                {
                    result[i].LinkedClass = strArray2[j];
                    break;
                }
            }
        }

        return result;
    }
}