using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// Font Header Table
    /// </summary>
    public record HeadTable
    {
        public static HeadTable FromReader(FontReader reader, TableRecordEntry entry)
        {
            reader.Seek(entry.Offset);

            var major = reader.ReadUInt16BigEndian();
            var minor = reader.ReadUInt16BigEndian();

            return new()
            {
                MajorVersion = major,
                MinorVersion = minor,
                FontRevision = reader.ReadFixedBigEndian(),
                ChecksumAdjustment = reader.ReadUInt32BigEndian(),
                MagicNumber = reader.ReadUInt32BigEndian(),
                Flags = reader.ReadUInt16BigEndian(),
                UnitsPerEm = reader.ReadUInt16BigEndian(),
                Created = reader.ReadInt64BigEndian(),
                Modified = reader.ReadInt64BigEndian(),
                XMin = reader.ReadInt16BigEndian(),
                XMax = reader.ReadInt16BigEndian(),
                YMin = reader.ReadInt16BigEndian(),
                YMax = reader.ReadInt16BigEndian(),
                MacStyle = reader.ReadUInt16BigEndian(),
                LowestRecPPEM = reader.ReadUInt16BigEndian(),
                FontDirectionHint = reader.ReadInt16BigEndian(),
                IndexToLocFormat = reader.ReadInt16BigEndian(),
                GlyphDataFormat = reader.ReadInt16BigEndian(),
            };
        }

        // Major version number of the font header table — set to 1.
        public ushort MajorVersion { get; init; }

        // Minor version number of the font header table — set to 0.
        public ushort MinorVersion { get; init; }

        // Set by font manufacturer.
        public float FontRevision { get; init; }

        public uint ChecksumAdjustment { get; init; }

        /// <summary>
        /// Set to 0x5F0F3CF5.
        /// </summary>
        public uint MagicNumber { get; init; }

        public ushort Flags { get; init; }

        /// <summary>
        /// Set to a value from 16 to 16384.Any value in this range is valid. In fonts that have TrueType outlines, a power of 2 is recommended as this allows performance optimizations in some rasterizers.
        /// </summary>
        public ushort UnitsPerEm { get; init; }

        /// <summary>
        /// Number of seconds since 12:00 midnight that started January 1st 1904 in GMT / UTC time zone.
        /// </summary>
        public long Created { get; init; }

        /// <summary>
        /// Number of seconds since 12:00 midnight that started January 1st 1904 in GMT / UTC time zone.
        /// </summary>
        public long Modified { get; init; }

        /// <summary>
        /// Minimum x coordinate across all glyph bounding boxes.
        /// </summary>
        public short XMin { get; init; }

        /// <summary>
        /// Minimum y coordinate across all glyph bounding boxes.
        /// </summary>
        public short YMin { get; init; }

        /// <summary>
        /// Maximum x coordinate across all glyph bounding boxes.
        /// </summary>
        public short XMax { get; init; }

        /// <summary>
        /// Maximum y coordinate across all glyph bounding boxes.
        /// </summary>
        public short YMax { get; init; }

        public ushort MacStyle { get; init; }

        /// <summary>
        /// Smallest readable size in pixels.
        /// </summary>
        public ushort LowestRecPPEM { get; init; }

        public short FontDirectionHint { get; init; }
        public short IndexToLocFormat { get; init; }
        public short GlyphDataFormat { get; init; }
    }
}
