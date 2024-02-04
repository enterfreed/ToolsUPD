using System.Diagnostics;

namespace CRLFconverter;

public static class Program
{
    private const string searchedExtension = ".cs";
    
    static void Main()
    {
        string startFolder = @"D:\Projects\U190001617_rgdob\src\GpnDs.PGA\";
        ProcessDirectory(startFolder);
    }
    /// <summary>
    /// Обходим все файлы в проекте и исправляем LF на CRLF
    /// </summary>
    /// <param name="targetDirectory"></param>
    static void ProcessDirectory(string targetDirectory)
    {
        // Обработка файлов в текущей директории
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
        {
            ProcessFile(fileName);
        }
        // Рекурсивный обход подпапок
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            ProcessDirectory(subdirectory);
        }
    }
    static void ProcessFile(string path)
    {
        var extension = Path.GetExtension(path);
        if (extension.Equals(searchedExtension))
        {
            ConverFile(path);
        }
    }
    static void ConverFile(string sourse)
    {
        Process process = new Process();
        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/c type " + sourse + " | find /v \"\" > " + sourse + ".tmp && move /Y " + sourse + ".tmp " + sourse;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
        Console.WriteLine("Обрабатываем файл " + sourse);
    }
}