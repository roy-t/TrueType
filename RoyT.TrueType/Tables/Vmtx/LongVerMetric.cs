using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables.Vmtx
{
    /// <summary>
    /// Horizontal layout metrics for a glyph
    /// </summary>
    public class LongVerMetric
    {
        public static LongVerMetric FromReader(FontReader reader)
        {
            return new()
            {
                AdvanceWidth = reader.ReadUInt16BigEndian(),
                TopSideBearing = reader.ReadInt16BigEndian(),
            };
        }

        /// <summary>
        /// Advance width, in font design units.
        /// </summary>
        public ushort AdvanceWidth { get; init; }

        /// <summary>
        /// Glyph left side bearing, in font design units.
        /// </summary>
        public short TopSideBearing { get; init; }
    }
}
