using System;

namespace RoyT.TrueType.Tables.Kern
{
    public struct KerningPair : IEquatable<KerningPair>
    {
        public ushort LeftGlyphCode { get; }
        public ushort RightGlyphCode { get; }

        public KerningPair(ushort leftGlyphCode, ushort rightGlyphCode)
        {
            this.LeftGlyphCode = leftGlyphCode;
            this.RightGlyphCode = rightGlyphCode;
        }

        public bool Equals(KerningPair other)
        {           
            return this.LeftGlyphCode == other.LeftGlyphCode && this.RightGlyphCode == other.RightGlyphCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is KerningPair pair)
            {
                return Equals(pair);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.LeftGlyphCode.GetHashCode() * 397) ^ this.RightGlyphCode.GetHashCode();
            }
        }        
    }
}
