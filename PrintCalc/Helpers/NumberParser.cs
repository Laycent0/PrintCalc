using System.Globalization;

namespace PrintCalc.Helpers;

public static class NumberParser
{
    // превращает текст в число (понимает и запятую, и точку)
    public static bool TryParse(string text, out double value)
    {
        return double.TryParse(text.Replace(',', '.'),
            NumberStyles.Any, CultureInfo.InvariantCulture, out value);
    }
}
