using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// Maximum Profile Table
    /// </summary>
    public sealed class MaxpTable
    {
        public static MaxpTable FromReader(FontReader reader)
        {
            reader.ReadFixedBigEndian(out short major, out short minor);
            ushort numGlyphcs = reader.ReadUInt16BigEndian();

            if (major == 0 && minor == 5)
            {
                return new()
                {
                    MajorVersion = major,
                    MinorVersion = minor,
                    NumGlyphs = numGlyphcs,
                };
            }

            return new()
            {
                MajorVersion = major,
                MinorVersion = minor,
                NumGlyphs = numGlyphcs,
                MaxPoints = reader.ReadUInt16BigEndian(),
                MaxContours = reader.ReadUInt16BigEndian(),
                MaxCompositePoints = reader.ReadUInt16BigEndian(),
                MaxCompositeContours = reader.ReadUInt16BigEndian(),
                MaxZones = reader.ReadUInt16BigEndian(),
                MaxTwilightPoints = reader.ReadUInt16BigEndian(),
                MaxStorage = reader.ReadUInt16BigEndian(),
                MaxFunctionDefs = reader.ReadUInt16BigEndian(),
                MaxInstructionDefs = reader.ReadUInt16BigEndian(),
                MaxStackElements = reader.ReadUInt16BigEndian(),
                MaxSizeOfInstructions = reader.ReadUInt16BigEndian(),
                MaxComponentElements = reader.ReadUInt16BigEndian(),
                MaxComponentDepth = reader.ReadUInt16BigEndian(),
            };
        }

        public short MajorVersion { get; init; }
        public short MinorVersion { get; init; }

        /// <summary>
        /// The number of glyphs in the font.
        /// </summary>
        public ushort NumGlyphs { get; init; }

        /// <summary>
        /// Maximum points in a non-composite glyph.
        /// </summary>
        public ushort MaxPoints { get; init; }

        /// <summary>
        /// Maximum contours in a non-composite glyph.
        /// </summary>
        public ushort MaxContours { get; init; }

        /// <summary>
        /// Maximum points in a composite glyph.
        /// </summary>
        public ushort MaxCompositePoints { get; init; }

        /// <summary>
        /// Maximum contours in a composite glyph.
        /// </summary>
        public ushort MaxCompositeContours { get; init; }

        /// <summary>
        /// 1 if instructions do not use the twilight zone (Z0), or 2 if instructions do use Z0; should be set to 2 in most cases.
        /// </summary>
        public ushort MaxZones { get; init; }

        /// <summary>
        /// Maximum points used in Z0.
        /// </summary>
        public ushort MaxTwilightPoints { get; init; }

        /// <summary>
        /// Number of Storage Area locations.
        /// </summary>
        public ushort MaxStorage { get; init; }

        /// <summary>
        /// Number of FDEFs, equal to the highest function number + 1.
        /// </summary>
        public ushort MaxFunctionDefs { get; init; }

        /// <summary>
        /// Number of IDEFs.
        /// </summary>
        public ushort MaxInstructionDefs { get; init; }

        /// <summary>
        /// Maximum stack depth across Font Program ('fpgm' table), CVT Program('prep' table) and all glyph instructions(in the 'glyf' table).
        /// </summary>
        public ushort MaxStackElements { get; init; }

        /// <summary>
        /// Maximum byte count for glyph instructions.
        /// </summary>
        public ushort MaxSizeOfInstructions { get; init; }

        /// <summary>
        /// Maximum number of components referenced at “top level” for any composite glyph.
        /// </summary>
        public ushort MaxComponentElements { get; init; }

        /// <summary>
        /// Maximum levels of recursion; 1 for simple components.
        /// </summary>
        public ushort MaxComponentDepth { get; init; }
    }
}