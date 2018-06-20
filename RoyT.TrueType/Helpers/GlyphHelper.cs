using System.Linq;

namespace RoyT.TrueType.Helpers
{
    public static class GlyphHelper
    {
        /// <summary>
        /// Returns the glyph index for the given character, or 0 if the character is not supported by this font
        /// </summary>        
        public static uint GetGlyphIndex(char c, TrueTypeFont font)
        {
            uint glyphIndex = 0;

            // Prefer Windows platform UCS2 glyphs as they are the recommended default on the Windows platform
            var preferred = font.CmapTable.EncodingRecords.FirstOrDefault(
                e => e.PlatformId == Platform.Windows
                     && e.WindowsEncodingId == WindowsEncoding.UnicodeUCS2);


            if (preferred != null)
            {
                glyphIndex = preferred.Subtable.GetGlyphIndex(c);
            }

            if (glyphIndex != 0)
            {
                return glyphIndex;
            }

            // Fall back to using any table to find the match
            foreach (var record in font.CmapTable.EncodingRecords)
            {
                glyphIndex = record.Subtable.GetGlyphIndex(c);
                if (glyphIndex != 0)
                {
                    return glyphIndex;
                }
            }

            return 0;
        }
    }

}
