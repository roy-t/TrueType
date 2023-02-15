using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// The Vertical Header Table contains information for vertical layout
    /// </summary>
    public sealed class VheaTable
    {
        public static VheaTable FromReader(FontReader reader, TableRecordEntry entry)
        {
            reader.Seek(entry.Offset);

            reader.ReadFixedBigEndian(out short major, out short minor);

            var ascender = reader.ReadInt16BigEndian();
            var descender = reader.ReadInt16BigEndian();
            var lineGap = reader.ReadInt16BigEndian();
            var advanceHeightMax = reader.ReadUInt16BigEndian();
            var minTopSideBearing = reader.ReadInt16BigEndian();
            var minBottomSideBearing = reader.ReadInt16BigEndian();
            var yMaxExtent = reader.ReadInt16BigEndian();
            var caretSlopeRise = reader.ReadInt16BigEndian();
            var caretSlopeRun = reader.ReadInt16BigEndian();
            var caretOffset = reader.ReadInt16BigEndian();

            // Seek over 4 reserved int16 fields
            reader.Seek(8, System.IO.SeekOrigin.Current);

            var metricDataFormat = reader.ReadInt16BigEndian();
            var numberOfVMetrics = reader.ReadUInt16BigEndian();

            return new VheaTable()
            {
                MajorVersion = major,
                MinorVersion = minor,
                Ascender = ascender,
                Descender = descender,
                LineGap = lineGap,
                AdvanceHeightMax = advanceHeightMax,
                MinTopSideBearing = minTopSideBearing,
                MinBottomSideBearing = minBottomSideBearing,
                YMaxExtent = yMaxExtent,
                CaretSlopeRise = caretSlopeRise,
                CaretSlopeRun = caretSlopeRun,
                CaretOffset = caretOffset,
                MetricDataFormat = metricDataFormat,
                NumberOfVMetrics = numberOfVMetrics,
            };
        }

        public static VheaTable Empty => new VheaTable();

        /// <summary>
        /// Major version number of the vertical header table
        /// </summary>
        public short MajorVersion { get; init; }

        /// <summary>
        /// Minor version number of the vertical header table
        /// </summary>
        public short MinorVersion { get; init; }

        /// <summary>
        /// The vertical typographic ascender for this font. It is the distance in FUnits from the vertical
        /// center baseline to the right edge of the design space for CJK / ideographic glyphs (or “ideographic em box”).
        /// 
        /// It is usually set to(head.unitsPerEm)/2. For example, a font with an em of 1000 FUnits will set this field
        /// to 500. See the Baseline tags section of the OpenType Layout Tag Registry for a description of the ideographic em-box.
        /// </summary>
        /// <remarks>
        /// Named vertTypoAscender in table version 1.1
        /// </remarks>
        public short Ascender { get; init; }

        /// <summary>
        /// The vertical typographic descender for this font. It is the distance in FUnits from the vertical center
        /// baseline to the left edge of the design space for CJK / ideographic glyphs.
        /// 
        /// It is usually set to -(head.unitsPerEm/2). For example, a font with an em of 1000 FUnits will set this field to -500.
        /// </summary>
        /// <remarks>
        /// Named vertTypoDescender in table version 1.1
        /// </remarks>
        public short Descender { get; init; }

        /// <summary>
        /// The vertical typographic gap for this font. An application can determine the recommended line spacing for single
        /// spaced vertical text for an OpenType font by the following expression: ideo embox width + vhea.vertTypoLineGap
        /// </summary>
        /// <remarks>
        /// Named vertTypoLineGap in table version 1.1
        /// </remarks>
        public short LineGap { get; init; }

        /// <summary>
        /// Maximum advance height measurement in FUnits.
        /// </summary>
        public ushort AdvanceHeightMax { get; init; }

        /// <summary>
        /// Minimum top sidebearing value in FUnits.
        /// </summary>
        public short MinTopSideBearing { get; init; }

        /// <summary>
        /// Minimum bottom sidebearing value in FUnits.
        /// </summary>
        public short MinBottomSideBearing { get; init; }

        /// <summary>
        /// Defined as yMaxExtent = max(tsb + (yMax - yMin)).
        /// </summary>
        public short YMaxExtent { get; init; }

        /// <summary>
        /// The value of the caretSlopeRise field divided by the value of the caretSlopeRun Field 
        /// determines the slope of the caret. A value of 0 for the rise and a value of 1 for the
        /// run specifies a horizontal caret. A value of 1 for the rise and a value of 0 for the
        /// run specifies a vertical caret. Intermediate values are desirable for fonts whose glyphs
        /// are oblique or italic. For a vertical font, a horizontal caret is best.
        /// </summary>
        public short CaretSlopeRise { get; init; }

        /// <summary>
        ///  See the caretSlopeRise field. Value=1 for nonslanted vertical fonts.
        /// </summary>
        public short CaretSlopeRun { get; init; }

        /// <summary>
        /// The amount by which the highlight on a slanted glyph needs to be shifted away from the 
        /// glyph in order to produce the best appearance. Set value equal to 0 for nonslanted fonts.
        /// </summary>
        public short CaretOffset { get; init; }

        /// <summary>
        /// 0 for current format.
        /// </summary>
        public short MetricDataFormat { get; init; }

        /// <summary>
        /// Number of advance heights in the 'vmtx' table
        /// </summary>
        public ushort NumberOfVMetrics { get; init; }
    }
}
