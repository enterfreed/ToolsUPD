using Dapper;
using Npgsql;

namespace MigrationRunner;

public class DataBaseConnection
{
    private const string ConnectionString = "Host=localhost;Port=5432;Username={0};Password={1};Database={0}";

    private static readonly string[] InitUser = 
    {
        " create role pga with login password 'pga123' superuser",
        @"CREATE DATABASE pga
              WITH OWNER = pga
                   ENCODING = 'UTF8'
                   LC_COLLATE = 'en_US.utf8'
                   LC_CTYPE = 'en_US.utf8'
                   connection limit = -1;",
        "alter database pga set time zone 'Europe/Moscow'",
        "grant all on database pga to pga",
        "revoke all on database pga from public",
        "revoke create on schema public from public",
        "CREATE EXTENSION btree_gist",
        "CREATE EXTENSION tablefunc"
    };

    public static void RunInitUser()
    {
        string connString = string.Format(ConnectionString, "postgres", "111");
        NpgsqlConnection conn = new NpgsqlConnection(connString);
        foreach (var sql in InitUser)
        {
            conn.Execute(sql);
        }
    }

    public static void ExecuteQuery(List<string> filesList, string loginPassword, bool isSplitted)
    {
        char delimiter = ':';
        string[] logPwd = loginPassword.Split(delimiter);
        
        string connString = string.Format(ConnectionString, logPwd[0], logPwd[1]);
        
        NpgsqlConnection conn = new NpgsqlConnection(connString);
        
        try
        { 
            conn.Open();
       
            foreach (var filePath in filesList)
            {
                 string sqlArray = File.ReadAllText(filePath);
                 
                 if (isSplitted)
                 {
                     var commands = sqlArray.Split(';');
                     
                     foreach (var command in commands.Where(x => !string.IsNullOrWhiteSpace(x)))
                     {
                         try
                         {
                             conn.Execute(command);
                         }
                         catch (Exception e)
                         {
                             Console.WriteLine("Команда не сработала " + e.Message + " для " + filePath);
                         }
                     }
                 }
                 else
                 {
                     try
                     {
                         conn.Execute(sqlArray);
                     }
                     catch (Exception e)
                     {
                         Console.WriteLine("Команда не сработала " + e.Message  + " для " + filePath);
                     }
                 }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка соединения с базой данных: " + ex.Message);
        }
        finally
        {
            conn.Close();
        }
    }
}