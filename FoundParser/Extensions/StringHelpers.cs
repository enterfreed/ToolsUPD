namespace FoundParser.Extensions;

public static class StringHelpers
{
    /// <summary>
    ///  Метод принимает строку, определяет начинается ли она с числа и возвращает bool и само число
    /// </summary>
    /// <param name="str"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
   public static (bool isNum, int lineNum) IsPublishStartsWithNum(string str)
   {
      (bool, int) result = (false, 0);
      var trimmedStr = str.Trim('(', ' ');
      int index = trimmedStr.IndexOf(':');
      
      if (index != -1)
      {
          string substringNum = trimmedStr.Substring(0, index);
          result = (int.TryParse(substringNum, out int number), number);
      }
       return result;
   }
    
    /// <summary>
    ///  Метод принимает строку, определяет начинается ли она с числа и возвращает bool и само число
    /// </summary>
    /// <param name="str"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static (bool isNum, int lineNum) IsSubscriberStartsWithNum(string str)
    {
        var cutStr = str.Trim().Split(' ')[0];
        var result = (int.TryParse(cutStr, out int number), number);
        return result;
    }

    /// <summary>
    /// Метод удаляет подстроку между символами, включая эти символы
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static string DelSubstr(string str, char start, char end)
    {
        if (str.Contains(start) && str.Contains(end))
        {
            int startIndex = str.IndexOf(start);
            int endIndex = str.IndexOf(end);
            int length = endIndex - startIndex + 1;
            str = str.Remove(startIndex, length).Trim();
        }

        return str;
    }
    
    /// <summary>
    /// Метод получает подстроку между символами
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    
    public static string GetSubstr(string str, char start, char end)
    {
        if (str.Contains(start) && str.Contains(end))
        {
            int startIndex = str.IndexOf(start)+1;
            int endIndex = str.IndexOf(end);
            int length = endIndex - startIndex;

            if (length < 0)
            {
                return str;
            }
            
            str = str.Substring(startIndex, length).Trim();
            return str;
        }
        return str;
    }

    /// <summary>
    /// Метод получает строку, в которой содержится искомый тип и пытается обрабатывает строку по условиям
    /// </summary>
    /// <param name="str"></param>
    /// <param name="searchedVariable"></param>
    /// <returns></returns>
    public static string GetClassFromString(string str, string searchedVariable = " ")
    {
        string subString = "new";
        string cutStr = " ";

        if (str.Contains(subString))
        {
            int subStringIndex = str.IndexOf(subString);

            if (str.Contains("new ()"))
            {
                cutStr = str.Substring(0, subStringIndex - 2);
                
            }else
            {
                cutStr = str.Substring(subStringIndex + subString.Length);
            }

            cutStr = DelSubstr(cutStr, '(', ')');
            
            return cutStr.Trim().Split(' ', StringSplitOptions.TrimEntries)[0];
        }
        else
        {
            if (!searchedVariable.Contains('$'))
            {
               var arrStr = str.Split(' ');
               str = arrStr[arrStr.Length - 2];
               char start = '(';
               int startIndex = str.IndexOf(start) +1;
               cutStr = str.Substring(startIndex).Trim();
               return cutStr;
            }
        }
        
        return cutStr;
    }

    public static string GetStringWithoutBrackets(string sourse)
    {
        string str = sourse.Replace("<", "").Replace(">", "");
        str = DelSubstr(str, '(', ')');

        return str;
    }
    
}