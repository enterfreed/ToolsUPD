namespace SchemaParser.models;

public class TextProperties
{
    public string Text { get; set; }
    public int Html { get; set; }
    public int Spacing { get; set; }
    public string Align { get; set; }
    public string FontFamily { get; set; }
    public int FontSize { get; set; }
    public int FontStyle { get; set; }
    public string FontColor { get; set; }

    public TextProperties(string input)
    {
        string[] parts = input.Split(';');
        foreach (var part in parts)
        {
            string[] keyValue = part.Split('=');
            string key = keyValue[0];
            string value = keyValue[1];

            switch (key)
            {
                case "text":
                    Text = value;
                    break;
                case "html":
                    Html = int.Parse(value);
                    break;
                case "spacing":
                    Spacing = int.Parse(value);
                    break;
                case "align":
                    Align = value;
                    break;
                case "fontFamily":
                    FontFamily = value;
                    break;
                case "fontSize":
                    FontSize = int.Parse(value);
                    break;
                case "fontStyle":
                    FontStyle = int.Parse(value);
                    break;
                case "fontColor":
                    FontColor = value;
                    break;
                default:
                    break;
            }
        }
    }
}