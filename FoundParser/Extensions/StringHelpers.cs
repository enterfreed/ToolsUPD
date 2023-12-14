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

    public static string DelSubstr(string str, string start, string end)
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


    public static string GetClassFromString(string str)
    {
        string subString = "new";

        if (!str.Contains(subString))
        {
            return str;
        }

        if (str.Contains("_logger"))
        {
            return "parse error in string: " + str.Trim();
        }


        int subStringIndex = str.IndexOf(subString);
        string cutStr;

        if (str.Contains("new ()"))
        {
            cutStr = str.Substring(0, subStringIndex - 2);
            return cutStr.Trim();
        }
        else
        {
            cutStr = str.Substring(subStringIndex + subString.Length);
        }

        var result = "";

        for (int i = 0; i < cutStr.Length; i++)
        {
            if (cutStr[0] != ' ')
            {
                break;
            }

            if (i > 0)
            {
                if (cutStr[i] == ' ')
                {
                    break;
                }
            }

            result += cutStr[i];
        }

        return result;
    }
}