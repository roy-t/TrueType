using System;
using System.Collections.Generic;
using System.IO;
using RoyT.TrueType.IO;
using RoyT.TrueType.Tables;
using RoyT.TrueType.Tables.Cmap;
using RoyT.TrueType.Tables.Kern;
using RoyT.TrueType.Tables.Name;

namespace RoyT.TrueType
{
    public sealed class TrueTypeFont
    {
        public static TrueTypeFont FromFile(string path)
        {
            using (var reader = new FontReader(File.OpenRead(path)))
            {
                var offsetTable = OffsetTable.FromReader(reader);
                var entries = ReadTableRecords(reader, offsetTable);

                var cmap = ReadCmapTable(path, reader, entries);
                var name = ReadNameTable(path, reader, entries);
                var kern = ReadKernTable(reader, entries);
                var hhea = ReadHheaTable(reader, entries);

                return new TrueTypeFont(path, offsetTable, entries, cmap, name, kern)
                {
                    HheaTable = hhea,
                };
            }
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
            if (entries.TryGetValue("cmap", out var cmapEntry))
            {
                reader.Seek(cmapEntry.Offset);
                return CmapTable.FromReader(reader);
            }

            throw new Exception(
                $"Font {path} does not contain a Character To Glyph Index Mapping Table (cmap)");

        }

        private static NameTable ReadNameTable(string path, FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {            
            if (entries.TryGetValue("name", out var cmapEntry))
            {
                reader.Seek(cmapEntry.Offset);
                return NameTable.FromReader(reader);
            }

            throw new Exception(
                $"Font {path} does not contain a Name Table (name)");

        }

        private static KernTable ReadKernTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue("kern", out var kernEntry))
            {
                reader.Seek(kernEntry.Offset);
                return KernTable.FromReader(reader);
            }

            return KernTable.Empty;            
        }
        
        private static HheaTable ReadHheaTable(FontReader reader, IReadOnlyDictionary<string, TableRecordEntry> entries)
        {
            if (entries.TryGetValue("hhea", out var kernEntry))
            {
                reader.Seek(kernEntry.Offset);
                return HheaTable.FromReader(reader);
            }

            return HheaTable.Empty;
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
        /// Contains adjustment to horizontal/vertical positions between glyphs
        /// </summary>
        public KernTable KernTable { get; }

        /// <summary>
        /// Contains information for horizontal layout
        /// </summary>
        public HheaTable HheaTable { get; init; }

        public override string ToString() => this.Source;
    }
}
