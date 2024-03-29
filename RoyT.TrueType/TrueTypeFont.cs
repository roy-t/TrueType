﻿using RoyT.TrueType.IO;
using RoyT.TrueType.Tables;
using RoyT.TrueType.Tables.Cmap;
using RoyT.TrueType.Tables.Hmtx;
using RoyT.TrueType.Tables.Kern;
using RoyT.TrueType.Tables.Name;
using RoyT.TrueType.Tables.Vmtx;
using System;
using System.Collections.Generic;
using System.IO;

namespace RoyT.TrueType
{
    public sealed class TrueTypeFont
    {
        public static TrueTypeFont FromFile(string path)
        {
            using var fstream = File.OpenRead(path);
            using var reader = new FontReader(fstream);

            return FromStream(reader, path);
        }

        public static IReadOnlyList<TrueTypeFont> FromCollectionFile(string path)
        {
            using var fstream = File.OpenRead(path);
            using var reader = new FontReader(fstream);

            var header = TtcHeader.Parse(reader);

            List<TrueTypeFont> fonts = new();
            foreach (var offset in header.TableDirectoryOffsets)
            {
                reader.Seek(offset);
                fonts.Add(FromStream(reader, path));
            }

            return fonts;
        }

        public static TrueTypeFont FromStream(FontReader reader, string source)
        {
            var offsetTable = OffsetTable.FromReader(reader);
            var entries = ReadTableRecords(reader, offsetTable);

            var cmap = ReadCmapTable(source, reader, entries);
            var name = ReadNameTable(source, reader, entries);
            var head = ReadHeadTable(source, reader, entries);
            var maxp = ReadMaxpTable(source, reader, entries);
            var os2 = ReadOs2Table(reader, entries);
            var kern = ReadKernTable(reader, entries);
            var hhea = ReadHheaTable(reader, entries);
            var vhea = ReadVheaTable(reader, entries);
            var hmtx = ReadHmtxTable(reader, entries, hhea.NumberOfHMetrics, maxp.NumGlyphs);
            var vmtx = ReadVmtxTable(reader, entries, vhea.NumberOfVMetrics, maxp.NumGlyphs);

            return new TrueTypeFont(source, offsetTable, entries, cmap, name, kern)
            {
                Os2Table = os2,
                HeadTable = head,
                MaxpTable = maxp,
                HheaTable = hhea,
                HmtxTable = hmtx,
                VheaTable = vhea,
                VmtxTable = vmtx,
            };
        }

        public static bool TryGetTablePosition(FontReader reader, string tableName, out long offset)
        {
            reader.Seek(0);
            var offsetTable = OffsetTable.FromReader(reader);
            var entries = ReadTableRecords(reader, offsetTable);

            if (entries.TryGetValue(tableName, out var cmapEntry))
            {
                offset = cmapEntry.Offset;
                return true;
            }

            offset = 0;
            return false;
        }

        private static IReadOnlyDictionary<string, TableRecordEntry> ReadTableRecords(FontReader reader, OffsetTable offsetTable)
        {
            var entries = new Dictionary<string, TableRecordEntry>(offsetTable.Tables);
            for (var i = 0; i < offsetTable.Tables; i++)
            {
                var entry = TableRecordEntry.FromReader(reader);
                entries.Add(entry.Tag, entry);
            }

            return entries;
        }

        private static CmapTable ReadCmapTable(string path, FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.cmap, out var cmapEntry))
            {
                reader.Seek(cmapEntry.Offset);
                return CmapTable.FromReader(reader);
            }

            throw new Exception(
                $"Font {path} does not contain a Character To Glyph Index Mapping Table (cmap)");

        }

        private static NameTable ReadNameTable(string path, FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {            
            if (entries.TryGetValue(TrueTypeTableNames.name, out var cmapEntry))
            {
                reader.Seek(cmapEntry.Offset);
                return NameTable.FromReader(reader);
            }

            throw new Exception(
                $"Font {path} does not contain a Name Table (name)");
        }

        private static HeadTable ReadHeadTable(string path, FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.head, out var entry))
            {
                return HeadTable.FromReader(reader, entry);
            }

            throw new Exception(
                $"Font {path} does not contain a Font Header Table (head)");
        }

        private static MaxpTable ReadMaxpTable(string path, FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.maxp, out var entry))
            {
                return MaxpTable.FromReader(reader, entry);
            }

            throw new Exception(
                $"Font {path} does not contain a Maximum Profile Table (maxp)");
        }

        private static Os2Table ReadOs2Table(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.os2, out var entry))
            {
                return Os2Table.FromReader(reader, entry);
            }

            return default;
        }

        private static KernTable ReadKernTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.kern, out var kernEntry))
            {
                reader.Seek(kernEntry.Offset);
                return KernTable.FromReader(reader);
            }

            return KernTable.Empty;            
        }

        private static HheaTable ReadHheaTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.hhea, out var entry))
            {
                return HheaTable.FromReader(reader, entry);
            }

            return HheaTable.Empty;
        }
        
        private static HmtxTable ReadHmtxTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries, ushort metricsCount, ushort glyphCount)
        {
            if (entries.TryGetValue(TrueTypeTableNames.hmtx, out var entry))
            {
                return HmtxTable.FromReader(reader, entry, metricsCount, glyphCount);
            }

            return HmtxTable.Empty;
        }

        private static VheaTable ReadVheaTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue(TrueTypeTableNames.vhea, out var entry))
            {
                return VheaTable.FromReader(reader, entry);
            }

            return VheaTable.Empty;
        }

        private static VmtxTable ReadVmtxTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries, ushort metricsCount, int glyphCount)
        {
            if (entries.TryGetValue(TrueTypeTableNames.vmtx, out var entry))
            {
                return VmtxTable.FromReader(reader, entry, metricsCount, glyphCount);
            }

            return VmtxTable.Empty;
        }

        private TrueTypeFont(string source, OffsetTable offsetTable, IReadOnlyDictionary<string, TableRecordEntry> entries, CmapTable cmapTable, NameTable nameTable, KernTable kernTable)
        {
            this.Source = source;
            this.OffsetTable = offsetTable;
            this.TableRecordEntries = entries;
            this.CmapTable = cmapTable;
            this.NameTable = nameTable;
            this.KernTable = kernTable;
        }

        public string Source { get; }
        public OffsetTable OffsetTable { get; }
        public IReadOnlyDictionary<string, TableRecordEntry> TableRecordEntries { get; }
        
        /// <summary>
        /// Contains information to get the glyph that corresponds to each supported character
        /// </summary>
        public CmapTable CmapTable { get; }

        /// <summary>
        /// Contains the (translated) name of this font, copyright notices, etc...
        /// </summary>
        public NameTable NameTable { get; }

        /// <summary>
        /// Consists of a set of metrics and other data that are required in OpenType fonts
        /// </summary>
        public Os2Table Os2Table { get; init; }

        /// <summary>
        /// Consists of global information about the font
        /// </summary>
        public HeadTable HeadTable { get; init; }

        /// <summary>
        /// Contains the memory requirements for this font.
        /// </summary>
        public MaxpTable MaxpTable { get; init; }

        /// <summary>
        /// Contains adjustment to horizontal/vertical positions between glyphs
        /// </summary>
        public KernTable KernTable { get; }

        /// <summary>
        /// Contains information for horizontal layout
        /// </summary>
        public HheaTable HheaTable { get; init; }

        /// <summary>
        /// Horizontal Metrics Table
        /// </summary>
        public HmtxTable HmtxTable { get; init; }
        
        /// <summary>
        /// Contains information for vertical layout
        /// </summary>
        public VheaTable VheaTable { get; init; }

        /// <summary>
        /// Vertical Metrics Table
        /// </summary>
        public VmtxTable VmtxTable { get; init; }

        public override string ToString() => this.Source;
    }
}
