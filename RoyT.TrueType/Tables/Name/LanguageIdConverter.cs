using System;
using System.Collections.Generic;
using System.Globalization;

namespace RoyT.TrueType.Tables.Name
{
    public static class LanguageIdConverter
    {
        private static readonly Dictionary<int, string> LCIDMap = ReadMapFromStringResource(Properties.Resources.LCIDMap);
        private static readonly Dictionary<int, string> MacLanguageCodeMap = ReadMapFromStringResource(Properties.Resources.MacLanguageCodeMap);


        public static CultureInfo ToCulture(Platform platform, ushort languageId)
        {
            if (platform == Platform.Windows)
            {
                if (LCIDMap.TryGetValue(languageId, out string culture))
                {                    
                    return new CultureInfo(culture);
                }                
            }
            
            if (platform == Platform.Macintosh)
            {
                if (MacLanguageCodeMap.TryGetValue(languageId, out string culture))
                {
                    return new CultureInfo(culture);
                }
            }

            return CultureInfo.InvariantCulture;            
        }

        private static Dictionary<int, string> ReadMapFromStringResource(string resouce)
        {
            var map = new Dictionary<int, string>();

            var lines = resouce.Split(new []{ "\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                int code;

                if (parts[0].StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    code = Convert.ToInt32(parts[0].Substring(2), 16);
                }
                else
                {
                    code = int.Parse(parts[0]);
                }                

                map.Add(code, parts[1]);
            }

            return map;
        }        
    }
}
