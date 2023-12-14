namespace FoundParser.Extensions;

public static class StringHelpers
{
    /// <summary>
    ///  Метод принимает строку, определяет начинается ли она с числа и возвращает bool
    /// </summary>
    /// <param name="str"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static bool IsStartsWithNum(string str, out int number)
    {
        var trimmedStr = str.Trim('(', ' ');
        
        int index = trimmedStr.IndexOf(':');
        
        if (index != -1)
        {
            string substringNum = trimmedStr.Substring(0, index);

            if (!int.TryParse(substringNum, out number))
            {
                throw new Exception("Ваши строки не числа");
            }

            return true;
        }

        number = -1;
        return false;
    }

    public static string GetClearedString(string str)
    {
        string cutString = str;
        
        if (str.Contains('('))
        {
            int endIndex = str.IndexOf('(');
            cutString = str.Substring(0, endIndex).Replace('<', ' ').Replace('>', ' ').Trim();
        }
        return cutString;
    }

    public static string GetClassFromString(string str)
    {
        string subString = "new";
        
        if (!str.Contains(subString))
        {
            return str;
        }
        
        int startIndex = str.IndexOf(subString);
        var result = "";

        //тут я конечно ловко придумал, но если добавить лишний пробел после new то  все схлопнется, переписать...

        for (int i = startIndex+subString.Length+1; i < str.Length; i++)
        {
            if (str[i] == ' ')
            {
                break;
            }
            result += str[i];
        }
      
        return result;
    }
}