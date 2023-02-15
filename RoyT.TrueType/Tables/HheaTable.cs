using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// The Horizontal Header Table contains information for horizontal layout
    /// </summary>
    public sealed class HheaTable
    {
        public static HheaTable FromReader(FontReader reader, TableRecordEntry entry)
        {
            reader.Seek(entry.Offset);

            var majorVersion = reader.ReadUInt16BigEndian();
            var minorVersion = reader.ReadUInt16BigEndian();
            var ascender = reader.ReadInt16BigEndian();
            var descender = reader.ReadInt16BigEndian();
            var lineGap = reader.ReadInt16BigEndian();
            var advanceWidthMax = reader.ReadUInt16BigEndian();
            var minLeftSideBearing = reader.ReadInt16BigEndian();
            var minRightSideBearing = reader.ReadInt16BigEndian();
            var xMaxExtent = reader.ReadInt16BigEndian();
            var caretSlopeRise = reader.ReadInt16BigEndian();
            var caretSlopeRun = reader.ReadInt16BigEndian();
            var caretOffset = reader.ReadInt16BigEndian();

            // Seek over reserved bytes
            reader.Seek(4);

            var metricDataFormat = reader.ReadInt16BigEndian();
            var numberOfHMetrics = reader.ReadUInt16BigEndian();

            return new HheaTable()
            {
                MajorVersion = majorVersion,
                MinorVersion = minorVersion,
                Ascender = ascender,
                Descender = descender,
                LineGap = lineGap,
                AdvanceWidthMax = advanceWidthMax,
                MinLeftSideBearing = minLeftSideBearing,
                MinRightSideBearing = minRightSideBearing,
                XMaxExtent = xMaxExtent,
                CaretSlopeRise = caretSlopeRise,
                CaretSlopeRun = caretSlopeRun,
                CaretOffset = caretOffset,
                MetricDataFormat = metricDataFormat,
                NumberOfHMetrics = numberOfHMetrics,
            };
        }

        public static HheaTable Empty => new HheaTable();

        /// <summary>
        /// Major version number of the horizontal header table
        /// </summary>
        public ushort MajorVersion { get; init; }

        /// <summary>
        /// Minor version number of the horizontal header table
        /// </summary>
        public ushort MinorVersion { get; init; }

        /// <summary>
        /// Typographic ascent—see note below.
        /// </summary>
        public short Ascender { get; init; }

        /// <summary>
        /// Typographic descent—see note below.
        /// </summary>
        public short Descender { get; init; }

        /// <summary>
        /// Typographic line gap.
        /// </summary>
        public short LineGap { get; init; }

        /// <summary>
        /// Maximum advance width value in 'hmtx' table.
        /// </summary>
        public ushort AdvanceWidthMax { get; init; }

        /// <summary>
        /// Minimum left sidebearing value in 'hmtx' table for glyphs with contours (empty glyphs should be ignored).
        /// </summary>
        public short MinLeftSideBearing { get; init; }

        /// <summary>
        /// Minimum right sidebearing value; calculated as min(aw - (lsb + xMax - xMin)) for glyphs with contours(empty glyphs should be ignored).
        /// </summary>
        public short MinRightSideBearing { get; init; }

        /// <summary>
        /// Max(lsb + (xMax - xMin)).
        /// </summary>
        public short XMaxExtent { get; init; }

        /// <summary>
        /// Used to calculate the slope of the cursor(rise/run); 1 for vertical.
        /// </summary>
        public short CaretSlopeRise { get; init; }

        /// <summary>
        /// 0 for vertical.
        /// </summary>
        public short CaretSlopeRun { get; init; }

        /// <summary>
        /// The amount by which a slanted highlight on a glyph needs to be shifted to produce the best appearance.Set to 0 for non-slanted fonts
        /// </summary>
        public short CaretOffset { get; init; }

        /// <summary>
        /// 0 for current format.
        /// </summary>
        public short MetricDataFormat { get; init; }

        /// <summary>
        /// Number of hMetric entries in 'hmtx' table
        /// </summary>
        public ushort NumberOfHMetrics { get; init; }
    }
}
