using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI;

namespace Libraries
{
    public class DynamicColorManager
    {
        public static void WriteAccentColorID()
        {
            List<double> values = new List<double>();
            values.Add(0);
            values.Add(0.083);
            values.Add(0.166);
            values.Add(0.25);
            values.Add(0.333);
            values.Add(0.416);
            values.Add(0.5);
            values.Add(0.583);
            values.Add(0.666);
            values.Add(0.75);
            values.Add(0.833);
            values.Add(0.916);
            values.Add(1);
            values.Add(0.916);
            values.Add(0.833);
            values.Add(0.75);
            values.Add(0.666);
            values.Add(0.583);
            values.Add(0.5);
            values.Add(0.416);
            values.Add(0.333);
            values.Add(0.25);
            values.Add(0.166);
            values.Add(0.083);
            values.Add(0);

            double[] valuesArray = values.ToArray();

            var hour = DateTime.Now.Hour;
            var id = Convert.ToInt16(hour);
            double v = valuesArray[id];
            if ((string)ApplicationData.Current.LocalSettings.Values["DynamicAccent"] != null)
            {
                Color aC = Windows.UI.Color.FromArgb(255, byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicAccentA"]).Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicAccentA"]).Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicAccentA"]).Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                Color bC = Windows.UI.Color.FromArgb(255, byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicAccentB"]).Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicAccentB"]).Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicAccentB"]).Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                byte r = (byte)((aC.R * v) + bC.R * (1 - v));
                byte g = (byte)((aC.G * v) + bC.G * (1 - v));
                byte b = (byte)((aC.B * v) + bC.B * (1 - v));
                Color result = Windows.UI.Color.FromArgb(255, r, g, b);
                ApplicationData.Current.LocalSettings.Values["MixedDynamicAccent"] = result.R.ToString("X2") + result.G.ToString("X2") + result.B.ToString("X2");
            }
            if ((string)ApplicationData.Current.LocalSettings.Values["DynamicColor"] != null)
            {
                Color aC = Windows.UI.Color.FromArgb(255, byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicColorA"]).Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicColorA"]).Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicColorA"]).Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                Color bC = Windows.UI.Color.FromArgb(255, byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicColorB"]).Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicColorB"]).Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(((string)ApplicationData.Current.LocalSettings.Values["DynamicColorB"]).Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                byte r = (byte)((aC.R * v) + bC.R * (1 - v));
                byte g = (byte)((aC.G * v) + bC.G * (1 - v));
                byte b = (byte)((aC.B * v) + bC.B * (1 - v));
                Color result = Windows.UI.Color.FromArgb(255, r, g, b);
                ApplicationData.Current.LocalSettings.Values["MixedDynamicColor"] = result.R.ToString("X2") + result.G.ToString("X2") + result.B.ToString("X2");
            }
        }
    }
}
