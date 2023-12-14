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
        
        if (File.Exists(fileName)) {
            strArray = File.ReadAllLines(fileName);
        }
        else
        {
            strArray = Array.Empty<string>();
        }

        return strArray;
    }
}