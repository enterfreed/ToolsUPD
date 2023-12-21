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
   // public static (bool isNum, int lineNum) IsPublishStartsWithNum(string str)
   // {
   //     var trimmedStr = str.Trim('(', ' ');

   //     int index = trimmedStr.IndexOf(':');

   //     if (index != -1)
   //     {
   //         string substringNum = trimmedStr.Substring(0, index);
   //         return int.TryParse(substringNum, out number);
   //     }
   //     
   //     number = -1;
   //     return false;
   // }
    
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
    /// 
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
    
    public static string GetSubstr(string str, char start, char end)
    {
        if (str.Contains(start) && str.Contains(end))
        {
            int startIndex = str.IndexOf(start)+1;
            int endIndex = str.IndexOf(end);
            int length = endIndex - startIndex;
            str = str.Substring(startIndex, length).Trim();
            return str;
        }
        return str;
    }


    public static string GetClassFromString(string str, string searchedVariable = " ")
    {
        if (str.Contains("_logger"))
        {
            return "parse error in string: " + str.Trim();
        }
        
        string subString = "new";
        string cutStr = " ";

        if (str.Contains(subString))
        {
            int subStringIndex = str.IndexOf(subString);
            

            if (str.Contains("new ()"))
            {
                cutStr = str.Substring(0, subStringIndex - 2);
                return cutStr.Trim();
            }
            else
            {
                cutStr = str.Substring(subStringIndex + subString.Length);
            }
       
            return cutStr.Split(' ', StringSplitOptions.TrimEntries)[1];
           
        }
        else
        {
            if (!searchedVariable.Contains('$'))
            {
               //cutStr = DelSubstr(str, '(', ')');
               //var arrStr = cutStr.Split(' ');
               //cutStr = arrStr[arrStr.Length - 1];
               //return cutStr;
               
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
}