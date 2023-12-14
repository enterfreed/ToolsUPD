namespace FoundParser;

public class FilePublisher
{
    public List<string> PathElems = new ();
    public int LineAddress;
    public string LinkedClass;
    
    /// <summary>
    /// Метод принимает строку с адресом корневой директории и отдает абсолютный путь к файлу в виде строки
    /// </summary>
    /// <param name="rootPath"></param>
    /// <returns></returns>
    /// 
   public string GetFullPath(string rootPath = " ")
    {
        return rootPath+Path.Combine(PathElems.ToArray());
    }
}