using System.Globalization;
using System.Linq;
using RoyT.TrueType.Tables.Name;

namespace RoyT.TrueType.Helpers
{
    public static class NameHelper
    {
        public static string GetName(NameId nameId, CultureInfo culture, TrueTypeFont font)
        {
            var candidates = font.NameTable.NameRecords.Where(r => 
                                r.NameId == nameId &&
                                r.Language.Equals(culture))
                            .ToList();

            if (candidates.Any())
            {

                // Prefer the Windows platform as the text has a rich encoding (UTF-16)
                if (candidates.Any(x => x.PlatformId == Platform.Windows))
                {
                    return candidates.First(x => x.PlatformId == Platform.Windows).Text;
                }

                // Fallback to any platform
                return candidates.First().Text;

            }
            
            return string.Empty;            
        }
    }
}
