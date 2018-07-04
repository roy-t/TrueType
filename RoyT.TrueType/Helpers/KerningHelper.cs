using RoyT.TrueType.Tables.Kern;

namespace RoyT.TrueType.Helpers
{
    public static class KerningHelper
    {
        /// <summary>
        /// Returns the horizontal kerning between the left and right character scaled by the scale parameter
        /// or 0 if no kerning information exists for this pair of characters
        /// </summary>        
        public static float GetHorizontalKerning(char left, char right, TrueTypeFont font)
        {
            if (font.KernTable.SubtableCount > 0)
            {
                var leftCode = GlyphHelper.GetGlyphIndex(left, font);
                var rightCode = GlyphHelper.GetGlyphIndex(right, font);

                foreach (var subTable in font.KernTable.Subtables)
                {
                    if (subTable.Format0 != null && subTable.Direction == Direction.Horizontal
                        && subTable.Values == Values.Kerning)
                    {
                        var pair = new KerningPair((ushort)leftCode, (ushort)rightCode);

                        if (subTable.Format0.Map.TryGetValue(pair, out var value))
                        {
                            return value;
                        }
                    }
                }
            }

            return 0.0f;
        }
    }
}
