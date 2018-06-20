using System;

namespace RoyT.TrueType
{
    public enum Platform : ushort
    {
        Unicode = 0,
        Macintosh = 1,

        [Obsolete]
        ISO = 2,

        Windows = 3,
        Custom = 4
    }
}
