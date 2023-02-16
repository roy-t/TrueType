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
                AdvanceHeight = reader.ReadUInt16BigEndian(),
                TopSideBearing = reader.ReadInt16BigEndian(),
            };
        }

        /// <summary>
        /// Advance height, in font design units.
        /// </summary>
        public ushort AdvanceHeight { get; init; }

        /// <summary>
        /// Glyph left side bearing, in font design units.
        /// </summary>
        public short TopSideBearing { get; init; }
    }
}
