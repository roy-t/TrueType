using RoyT.TrueType.IO;
using System.Collections.Generic;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// OS/2 and Windows Metrics Table
    /// </summary>
    public record Os2Table
    {
        public static Os2Table FromReader(FontReader reader, TableRecordEntry tableEntry)
        {
            reader.Seek(tableEntry.Offset);
            Os2Table table = new()
            {
                Version = reader.ReadUInt16BigEndian(),
                XAvgCharWidth = reader.ReadInt16BigEndian(),
                UsWeightClass = reader.ReadUInt16BigEndian(),
                UsWidthClass = reader.ReadUInt16BigEndian(),
                FsType = reader.ReadUInt16BigEndian(),
                YSubscriptXSize = reader.ReadInt16BigEndian(),
                YSubscriptYSize = reader.ReadInt16BigEndian(),
                YSubscriptXOffset = reader.ReadInt16BigEndian(),
                YSubscriptYOffset = reader.ReadInt16BigEndian(),
                YSuperscriptXSize = reader.ReadInt16BigEndian(),
                YSuperscriptYSize = reader.ReadInt16BigEndian(),
                YSuperscriptXOffset = reader.ReadInt16BigEndian(),
                YSuperscriptYOffset = reader.ReadInt16BigEndian(),
                YStrikeoutSize = reader.ReadInt16BigEndian(),
                YStrikeoutPosition = reader.ReadInt16BigEndian(),
                SFamilyClass = reader.ReadInt16BigEndian(),
                Panose = reader.ReadBytes(10),
                UlUnicodeRange1 = reader.ReadUInt32BigEndian(),
                UlUnicodeRange2 = reader.ReadUInt32BigEndian(),
                UlUnicodeRange3 = reader.ReadUInt32BigEndian(),
                UlUnicodeRange4 = reader.ReadUInt32BigEndian(),
                AchVendID = reader.ReadAscii(4),
                FsSelection = reader.ReadUInt16BigEndian(),
                UsFirstCharIndex = reader.ReadUInt16BigEndian(),
                UsLastCharIndex = reader.ReadUInt16BigEndian(),
            };

            if (table.Version == 0 && tableEntry.Length == reader.Position - tableEntry.Offset)
            {
                return table;
            }

            // Some version 0 fonts on Windows include the following extra fields
            table = table with
            {
                STypoAscender = reader.ReadInt16BigEndian(),
                STypoDescender = reader.ReadInt16BigEndian(),
                STypoLineGap = reader.ReadInt16BigEndian(),
                UsWinAscent = reader.ReadUInt16BigEndian(),
                UsWinDescent = reader.ReadUInt16BigEndian(),
            };

            if (table.Version == 0)
            {
                return table;
            }

            table = table with
            {
                UlCodePageRange1 = reader.ReadUInt32BigEndian(),
                UlCodePageRange2 = reader.ReadUInt32BigEndian(),
            };

            if (table.Version == 1)
            {
                return table;
            }

            table = table with
            {
                SxHeight = reader.ReadInt16BigEndian(),
                SCapHeight = reader.ReadInt16BigEndian(),
                UsDefaultChar = reader.ReadUInt16BigEndian(),
                UsBreakChar = reader.ReadUInt16BigEndian(),
                UsMaxContext = reader.ReadUInt16BigEndian(),
            };

            if (table.Version < 5)
            {
                return table;
            }

            table = table with
            {
                UsLowerOpticalPointSize = reader.ReadUInt16BigEndian(),
                UsUpperOpticalPointSize = reader.ReadUInt16BigEndian(),
            };

            return table;
        }

        public ushort Version { get; init; }
        public short XAvgCharWidth { get; init; }
        public ushort UsWeightClass { get; init; }
        public ushort UsWidthClass { get; init; }
        public ushort FsType { get; init; }
        public short YSubscriptXSize { get; init; }
        public short YSubscriptYSize { get; init; }
        public short YSubscriptXOffset { get; init; }
        public short YSubscriptYOffset { get; init; }
        public short YSuperscriptXSize { get; init; }
        public short YSuperscriptYSize { get; init; }
        public short YSuperscriptXOffset { get; init; }
        public short YSuperscriptYOffset { get; init; }
        public short YStrikeoutSize { get; init; }
        public short YStrikeoutPosition { get; init; }
        public short SFamilyClass { get; init; }
        public IReadOnlyList<byte> Panose { get; init; }
        public uint UlUnicodeRange1 { get; init; }
        public uint UlUnicodeRange2 { get; init; }
        public uint UlUnicodeRange3 { get; init; }
        public uint UlUnicodeRange4 { get; init; }
        public string AchVendID { get; init; }
        public ushort FsSelection { get; init; }
        public ushort UsFirstCharIndex { get; init; }
        public ushort UsLastCharIndex { get; init; }
        public short STypoAscender { get; init; }
        public short STypoDescender { get; init; }
        public short STypoLineGap { get; init; }
        public ushort UsWinAscent { get; init; }
        public ushort UsWinDescent { get; init; }
        public uint UlCodePageRange1 { get; init; }
        public uint UlCodePageRange2 { get; init; }
        public short SxHeight { get; init; }
        public short SCapHeight { get; init; }
        public ushort UsDefaultChar { get; init; }
        public ushort UsBreakChar { get; init; }
        public ushort UsMaxContext { get; init; }
        public ushort UsLowerOpticalPointSize { get; init; }
        public ushort UsUpperOpticalPointSize { get; init; }
    }
}