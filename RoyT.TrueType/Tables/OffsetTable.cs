using System;
using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables
{
    /// <summary>
    /// Contains the offset in the file to the major tables (like CMAP, NAME, KERN, etc...) in this font
    /// </summary>
    public sealed class OffsetTable
    {
        private static uint WindowsAdobe = 0x00010000u;
        private static uint OTTO = 0x4F54544Fu;

        // The Apple specification for TrueType fonts allows for 'true' and 'typ1' for sfnt version. These version tags should not be used for OpenType fonts.
        private static uint Mac = 0x74727565u;
        private static uint Typ1 = 0x74797031u;
        
        public static OffsetTable FromReader(FontReader reader)
        {
            var scalarType = reader.ReadUInt32BigEndian(); //reader.ReadFixedBigEndian(out short major, out short minor);

            if (scalarType == WindowsAdobe || scalarType == OTTO || scalarType == Typ1 || scalarType == Mac)
            {
                var tables        = reader.ReadUInt16BigEndian();
                var searchRange   = reader.ReadUInt16BigEndian();
                var entrySelector = reader.ReadUInt16BigEndian();
                var rangeShift    = reader.ReadUInt16BigEndian();

                return new OffsetTable(scalarType, tables, searchRange, entrySelector, rangeShift);
            }

            throw new Exception($"Unexpected OffsetTable scalar type. Expected: {WindowsAdobe} or {OTTO}, actual: {scalarType}");
        }

        private OffsetTable(uint scalarType, ushort tables, ushort searchRange, ushort entrySelector, ushort rangeShift)
        {
            this.ScalarType = scalarType;
            this.Tables = tables;
            this.SearchRange = searchRange;
            this.EntrySelector = entrySelector;
            this.RangeShift = rangeShift;
        }

        public uint ScalarType { get; }
        public ushort Tables { get; }
        public ushort SearchRange { get; }
        public ushort EntrySelector { get; }
        public ushort RangeShift { get; }

        public override string ToString() => $"Tables: {this.Tables}";        
    }
}
