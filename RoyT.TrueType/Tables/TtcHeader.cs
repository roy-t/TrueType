using RoyT.TrueType.IO;
using System.Collections.Generic;

namespace RoyT.TrueType
{
    /// <summary>
    /// Header for TrueType Collection files
    /// </summary>
    public record TtcHeader()
    {
        /// <summary>
        /// Font Collection ID 
        /// </summary>
        public string Tag { get; init; }

        /// <summary>
        /// Major version of the TTC Header
        /// </summary>
        public ushort MajorVersion { get; init; }

        /// <summary>
        /// Minor version of the TTC Header, = 0.
        /// </summary>
        public ushort MinorVersion { get; init; }

        /// <summary>
        /// Number of fonts in TTC
        /// </summary>
        public uint NumFonts { get; init; }

        /// <summary>
        /// Array of offsets to the TableDirectory for each font from the beginning of the file
        /// </summary>
        public IReadOnlyList<uint> TableDirectoryOffsets { get; init; }

        /// <summary>
        /// Tag indicating that a DSIG table exists, 0x44534947 (‘DSIG’) (null if no signature)
        /// </summary>
        public uint DsigTag { get; init; }

        /// <summary>
        /// The length(in bytes) of the DSIG table(null if no signature)
        /// </summary>
        public uint DsigLength { get; init; }

        /// <summary>
        ///  The offset(in bytes) of the DSIG table from the beginning of the TTC file (null if no signature)
        /// </summary>
        public uint DsigOffset { get; init; }

        public static TtcHeader Parse(FontReader reader)
        {
            TtcHeader result = new()
            {
                Tag = reader.ReadAscii(4),
                MajorVersion = reader.ReadUInt16BigEndian(),
                MinorVersion = reader.ReadUInt16BigEndian(),
                NumFonts = reader.ReadUInt32BigEndian(),
            };

            List<uint> offsets = new();
            for (int i = 0; i < result.NumFonts; i++)
            {
                offsets.Add(reader.ReadUInt32BigEndian());
            }

            if (result.MajorVersion == 1)
            {
                return result with { TableDirectoryOffsets = offsets };
            }

            return result with
            {
                TableDirectoryOffsets = offsets,
                DsigTag = reader.ReadUInt32BigEndian(),
                DsigLength = reader.ReadUInt32BigEndian(),
                DsigOffset = reader.ReadUInt32BigEndian(),
            };
        }
    }
}
