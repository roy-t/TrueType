using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables.Hmtx
{
    /// <summary>
    /// Horizontal layout metrics for a glyph
    /// </summary>
    public class LongHorMetric
    {
        public static LongHorMetric FromReader(FontReader reader)
        {
            return new()
            {
                AdvanceHeight = reader.ReadUInt16BigEndian(),
                Lsb = reader.ReadInt16BigEndian(),
            };
        }

        /// <summary>
        /// Advance width, in font design units.
        /// </summary>
        public ushort AdvanceHeight { get; init; }

        /// <summary>
        /// Glyph left side bearing, in font design units.
        /// </summary>
        public short Lsb { get; init; }
    }
}
