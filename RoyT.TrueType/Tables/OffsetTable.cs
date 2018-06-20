using System;
using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// Contains the offset in the file to the major tables (like CMAP, NAME, KERN, etc...) in this font
    /// </summary>
    public sealed class OffsetTable
    {        
        public static OffsetTable FromReader(FontReader reader)
        {
            reader.ReadFixedBigEndian(out short major, out short minor);

            if (major == 1 && minor == 0)
            {
                var tables        = reader.ReadUInt16BigEndian();
                var searchRange   = reader.ReadUInt16BigEndian();
                var entrySelector = reader.ReadUInt16BigEndian();
                var rangeShift    = reader.ReadUInt16BigEndian();

                return new OffsetTable(major, minor, tables, searchRange, entrySelector, rangeShift);
            }

            throw new Exception($"Unexpected OffsetTable version. Expected: 1.0, actual: {major}.{minor}");
        }

        private OffsetTable(short major, short minor, ushort tables, ushort searchRange, ushort entrySelector, ushort rangeShift)
        {
            this.Major = major;
            this.Minor = minor;
            this.Tables = tables;
            this.SearchRange = searchRange;
            this.EntrySelector = entrySelector;
            this.RangeShift = rangeShift;
        }

        public short Major { get; }
        public short Minor { get; }
        public ushort Tables { get; }
        public ushort SearchRange { get; }
        public ushort EntrySelector { get; }
        public ushort RangeShift { get; }

        public override string ToString() => $"Tables: {this.Tables}";        
    }
}
