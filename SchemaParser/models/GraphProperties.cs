namespace SchemaParser.models;

public class GraphProperties
{
    public string GraphMlID { get; set; }
    public string GradientDirection { get; set; }
    public string Shape { get; set; }
    public int StartSize { get; set; }
    public bool Rounded { get; set; }
    public int ArcSize { get; set; }
    public int Collapsible { get; set; }
    public string FillColor { get; set; }
    public string StrokeColor { get; set; }
    public double StrokeWidth { get; set; }
    public string SwimlaneFillColor { get; set; }

    public GraphProperties(string data)
    {
        string[] parts = data.Split(';');

        foreach (string part in parts)
        {
            string[] keyValue = part.Split('=');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0];
                string value = keyValue[1];

                switch (key)
                {
                    case "graphMlID":
                        GraphMlID = value;
                        break;
                    case "gradientDirection":
                        GradientDirection = value;
                        break;
                    case "shape":
                        Shape = value;
                        break;
                    case "startSize":
                        StartSize = int.Parse(value);
                        break;
                    case "rounded":
                        Rounded = value == "1";
                        break;
                    case "arcSize":
                        ArcSize = int.Parse(value);
                        break;
                    case "collapsible":
                        Collapsible = int.Parse(value);
                        break;
                    case "fillColor":
                        FillColor = value;
                        break;
                    case "strokeColor":
                        StrokeColor = value;
                        break;
                    case "strokeWidth":
                        StrokeWidth = double.Parse(value);
                        break;
                    case "swimlaneFillColor":
                        SwimlaneFillColor = value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}