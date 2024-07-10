namespace MigrationRunner;

public static class Program
{
    private const string folderPath = @"C:\Projects\U190001617_rgdob\src\GpnDs.PGA\";
    private const string initMigration = @"C:\Projects\U190001617_rgdob\src\GpnDs.PGA\GpnDs.PGA.Postgres.Migrations";
    private const string searchedStr = "Migrations";
    private const string searchedStrTest = "TestDatas";
    private const string searchedExtension = ".sql";
    private const bool IsAddTestData = true;
    
    private static Dictionary<string, string> rolePwd = new ()
    {
        {"GpnDs.PGA.Postgres.Migrations", "pga:pga123"},
        {"GpnDs.PGA.MDM.Migrations", "mdm:mdm123"},
        {"GpnDs.PGA.ExtAccordance.Migrations", "ext_accordance:ext_accordance123"},
        {"GpnDs.PGA.Facts.Migrations", "facts:facts123"},
        {"GpnDs.PGA.Plans.Migrations", "plans:plans123"},
        {"GpnDs.PGA.ProdVariations.Migrations", "prodvariations:prodvariations123"},
        {"GpnDs.PGA.Forecasts.Migrations", "forecasts:forecasts123"},
        {"GpnDs.PGA.WellEvents.Migrations", "wellevents:wellevents123"},
        {"GpnDs.PGA.SDM.Migrations", "sdm:sdm123"},
        {"GpnDs.PGA.ProdWells.Migrations", "prodwells:prodwells123"},
        {"GpnDs.PGA.IdleFunds.Migrations", "idlefunds:idlefunds123"},
        {"GpnDs.PGA.OilProds.Migrations", "oilprods:oilprods123"},
        {"GpnDs.PGA.OilProdDetails.Migrations", "oilproddetails:oilproddetails123"},
    };
    
    public static void Main()
    {
        //1. зайди в докер и удали нахуй базу
        //2. Создай контейнер. Не благодари за команду
            //docker run --name pg_test -p 5432:5432 -e POSTGRES_PASSWORD=111 -d postgres
        //3. скинь файлы миграций новые в папку GpnDs.PGA (раньше на проекте они лежали в одной куче да)
        //4. запускай тулзу

        DataBaseConnection.RunInitUser();
        
        var initFiles = GetFilesInDirectory(initMigration);
        string initPwd =  GetLoginPasswordString(initMigration);
        DataBaseConnection.ExecuteQuery(initFiles, initPwd, true);
        
        var directoriesList = GetDirectories(folderPath);
        
       foreach (var directory in directoriesList.Where(x => x != initMigration))
       {
            var filesList = GetFilesInDirectory(directory);
            if (filesList.Count == 0)  return;

            string loginPassword = GetLoginPasswordString(directory);
            DataBaseConnection.ExecuteQuery(filesList, loginPassword, false);
            if (IsAddTestData)
            {
                var filesTestList = GetFilesInDirectory(directory + "." + searchedStrTest);
                if (filesTestList.Count == 0)  return;
                DataBaseConnection.ExecuteQuery(filesTestList, loginPassword, false);
            }
       }
       
       Console.WriteLine("Миграции применены");
    }

    private static string GetLoginPasswordString(string dirName)
    {
        var relativePath = Path.GetRelativePath(folderPath, dirName);
        if (rolePwd.TryGetValue(relativePath, out var loginPassword))
        {
          return loginPassword;
        }
        throw new Exception($"Для {dirName} не найден логин и пароль");
    }

    private static List<string> GetFilesInDirectory(string directory)
    {
        var fileEntries = Directory.GetFiles(directory);
        var fileList = new List<string>();
        
        foreach (var scriptFilePath in fileEntries)
        {
            var extension = Path.GetExtension(scriptFilePath);
            if (extension.Equals(searchedExtension))
            {
                fileList.Add(scriptFilePath);
            }
        }
        return fileList;
    }

    private static List<string> GetDirectories(string folderPath)
    {
        var dirList = new List<string>();
        var subDirectories = Directory.GetDirectories(folderPath);
        
        foreach (var subDirectory in subDirectories)
        {
            var lastCharacters = subDirectory[^searchedStr.Length..];
            
            if (lastCharacters.Equals(searchedStr))
            {
                dirList.Add(subDirectory);
            }
        }
        return dirList;
    }
}