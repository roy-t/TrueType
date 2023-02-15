using RoyT.TrueType.IO;
using System;
using System.Collections.Generic;

namespace RoyT.TrueType.Tables.Vmtx
{
    /// <summary>
    /// Vertical Metrics Table
    /// </summary>
    public sealed class VmtxTable
    {
        public static VmtxTable FromReader(FontReader reader, TableRecordEntry entry, int metricsCount, int glyphCount)
        {
            reader.Seek(entry.Offset);

            List<LongVerMetric> vmetrics = new(metricsCount);
            for (int i = 0; i < metricsCount; i++)
            {
                vmetrics.Add(LongVerMetric.FromReader(reader));
            }

            int topSideBearingCount = glyphCount - metricsCount;
            topSideBearingCount = Math.Max(0, topSideBearingCount);

            List<short> topSideBearings = new(topSideBearingCount);
            for (int i = 0; i < topSideBearingCount; i++)
            {
                topSideBearings.Add(reader.ReadInt16BigEndian());
            }

            return new()
            {
                VMetrics = vmetrics,
                TopSideBearings = topSideBearings,
            };
        }

        public static VmtxTable Empty => new()
        {
            VMetrics = new List<LongVerMetric>()
        };

        /// <summary>
        /// Paired advance width and left side bearing values for each glyph. Records are indexed by glyph ID.
        /// </summary>
        public IReadOnlyList<LongVerMetric> VMetrics { get; init; }

        /// <summary>
        /// Optional array for glyphs not in VMetrics. Their TopSideBearing is equal to the TopSideBearing
        /// of the last entry in VMetrics
        /// </summary>
        public IReadOnlyList<short> TopSideBearings { get; init; }
    }
}
