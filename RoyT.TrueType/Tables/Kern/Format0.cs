using System.Collections.Generic;
using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables.Kern
{    
    public sealed class Format0
    {        
        public static Format0 FromReader(FontReader reader)
        {
            var pairs = reader.ReadUInt16BigEndian();
            var searchRange = reader.ReadUInt16BigEndian();
            var entrySelector = reader.ReadUInt16BigEndian();
            var rangeShift = reader.ReadUInt16BigEndian();

            var map = new Dictionary<KerningPair, short>();
            for (var i = 0; i < pairs; i++)
            {
                var left = reader.ReadUInt16BigEndian();
                var right = reader.ReadUInt16BigEndian();
                var value = reader.ReadInt16BigEndian();

                map.Add(new KerningPair(left, right), value);
            }

            return new Format0(pairs, searchRange, entrySelector, rangeShift, map);
        }

        private Format0(ushort pairs, ushort searchRange, ushort entrySelector, ushort rangeShift, Dictionary<KerningPair, short> map)
        {
            this.Pairs = pairs;
            this.SearchRange = searchRange;
            this.EntrySelector = entrySelector;
            this.RangeShift = rangeShift;
            this.Map = map;
        }

        public ushort Pairs { get; }
        public ushort SearchRange { get; }
        public ushort EntrySelector { get; }
        public ushort RangeShift { get; }
        public IReadOnlyDictionary<KerningPair, short> Map { get; }
    }
}
