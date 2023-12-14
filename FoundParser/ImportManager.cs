using FoundParser.Extensions;

namespace FoundParser;

public class ImportManager
{
    public static void SaveToCSV(List<FilePublisher> result, string fileName, string fileDir)
    {
        var resultToFile = new List<string>(); // лист для импорта с шапкой
        var title = new List<string>();
        title.Add("Path");

        result.Insert(0, new FilePublisher() { PathElems = title, LinkedClass = "Class" });

        foreach (var res in result)
        {
            var linkedClass = res.LinkedClass ?? " ";

            resultToFile.Add($"{res.GetFullPath()} ; {StringHelpers.GetClassFromString(linkedClass)}");
        }

        using StreamWriter sw = new StreamWriter(fileDir + fileName);

        foreach (var res in resultToFile)
        {
            sw.WriteLine(string.Join(",", res));
        }
    }
}