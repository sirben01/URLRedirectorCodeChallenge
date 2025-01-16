namespace RedirectorService
{
    public static class Helpers
    {

        public static double GetDoubleFromString(this string value, double defaultValue)
        {
            double convertedDouble = defaultValue;
            if (!string.IsNullOrEmpty(value)) 
                 convertedDouble = double.TryParse(value, out double converted) ? converted : defaultValue;

            return convertedDouble;
        }
    }
}
