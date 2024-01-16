namespace FilesUpdater;

public static class Program
{
    public static void Main()
    {
        const string startFolder = @"C:\Projects\U190001617_rgdob";
        
        var files = Directory.GetFiles(startFolder, "*.cs", SearchOption.AllDirectories);
     
        foreach (var file in files)
        {
            var content = File.ReadAllText(file);
            //content = content.Replace("test123", "test321");
            File.WriteAllText(file, content);
        }
    }
}