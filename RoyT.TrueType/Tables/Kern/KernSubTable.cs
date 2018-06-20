using RoyT.TrueType.IO;

namespace RoyT.TrueType.Tables.Kern
{
    public sealed class KernSubtable
    {        
        public static KernSubtable FromReader(FontReader reader)
        {
            var version = reader.ReadUInt16BigEndian();
            var length = reader.ReadUInt16BigEndian();
            var format = reader.ReadByte();
            var coverage = reader.ReadBitsBigEndian(1);
                     

            var direction = coverage.Get(0) ? Direction.Horizontal : Direction.Vertical;
            var values = coverage.Get(1) ? Values.Minimum : Values.Kerning;
            var isCrossStream = coverage.Get(2);
            var isOverride = coverage.Get(3);

            // The only format that is properly interpreted by Windows
            Format0 format0 = null;
            if (format == 0)
            {
                format0 = Format0.FromReader(reader);
            }

            return new KernSubtable(version, length, format, direction, values, isCrossStream, isOverride, format0);
        }

        private KernSubtable(ushort version, ushort length, byte format, Direction direction, Values values, bool isCrossStream, bool isOverride, Format0 format0)
        {
            this.Version = version;
            this.Length = length;
            this.Format = format;
            this.Direction = direction;
            this.Values = values;
            this.IsCrossStream = isCrossStream;
            this.IsOverride = isOverride;
            this.Format0 = format0;
        }

        public ushort Version { get; }
        public ushort Length { get; }
        public byte Format { get; }
        public Direction Direction { get; }
        public Values Values { get; }
        public bool IsCrossStream { get; }
        public bool IsOverride { get; }
        public Format0 Format0 { get; }

        public short GetKerning(KerningPair pair)
        {
            if (this.Format0 != null)
            {
                if (this.Format0.Map.TryGetValue(pair, out var value))
                {
                    return value;
                }
            }

            return 0;
        }
    }
}
