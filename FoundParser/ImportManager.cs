using FoundParser.Extensions;

namespace FoundParser;

public class ImportManager
{
    public static void SaveToCSV(List<FilePublisher> result, string fileName, string fileDir)
    {
        
        var resultToFile = new List<string>(); // лист для импорта с шапкой
        var title = new List<string>();
        title.Add("Path");

        result.Insert(0, new FilePublisher() {Project = "Project", PathElems = title, LinkedClass = "Class" });

        foreach (var res in result)
        {
            var linkedClass = res.LinkedClass ?? string.Empty;
            resultToFile.Add(string.Join(';', 
              res.Project,
              res.GetFullPath(),
              res.LinkedClass));
        }

        using StreamWriter sw = new StreamWriter(fileDir + fileName);
        resultToFile.ForEach(x => sw.WriteLine(string.Join(",", x)));
    }
}