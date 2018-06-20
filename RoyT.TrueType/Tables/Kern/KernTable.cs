using System.Collections.Generic;
using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables.Kern
{
    /// <summary>
    /// Contains adjustment to horizontal/vertical positions between glyphs
    /// </summary>
    public sealed class KernTable
    {        
        public static KernTable FromReader(FontReader reader)
        {
            var version = reader.ReadUInt16BigEndian();
            var subtableCount = reader.ReadUInt16BigEndian();

            var subtables = new List<KernSubtable>();
            for (var i = 0; i < subtableCount; i++)
            {
                var subTable = KernSubtable.FromReader(reader);
                subtables.Add(subTable);
            }

            return new KernTable(version, subtableCount, subtables);
        }        

        public static KernTable Empty => new KernTable(0, 0, new List<KernSubtable>());

        private KernTable(ushort version, ushort subtableCount, IReadOnlyList<KernSubtable> subtables)
        {
            this.Version = version;
            this.SubtableCount = subtableCount;
            this.Subtables = subtables;
        }

        public ushort Version { get; }
        public ushort SubtableCount { get; }
        public IReadOnlyList<KernSubtable> Subtables { get; }       
    }
}
