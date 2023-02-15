using RoyT.TrueType.IO;
using System;
using System.Collections.Generic;

namespace RoyT.TrueType.Tables.Hmtx
{
    /// <summary>
    /// Horizontal Metrics Table
    /// </summary>
    public sealed class HmtxTable
    {
        public static HmtxTable FromReader(FontReader reader, TableRecordEntry entry, int metricsCount, int glyphCount)
        {
            reader.Seek(entry.Offset);

            List<LongHorMetric> hmetrics = new(metricsCount);
            for (int i = 0; i < metricsCount; i++)
            {
                hmetrics.Add(LongHorMetric.FromReader(reader));
            }

            int leftSideBearingCount = glyphCount - metricsCount;
            leftSideBearingCount = Math.Max(0, leftSideBearingCount);

            List<short> leftSideBearings = new(leftSideBearingCount);
            for (int i = 0; i < leftSideBearingCount; i++)
            {
                leftSideBearings.Add(reader.ReadInt16BigEndian());
            }

            return new()
            {
                HMetrics = hmetrics,
                LeftSideBearings = leftSideBearings,
            };
        }

        public static HmtxTable Empty => new()
        {
            HMetrics = new List<LongHorMetric>()
        };

        /// <summary>
        /// Paired advance width and left side bearing values for each glyph. Records are indexed by glyph ID.
        /// </summary>
        public IReadOnlyList<LongHorMetric> HMetrics { get; init; }

        /// <summary>
        /// Optional array for glyphs not in HMetrics. Their Lsb is equal to the Lsb
        /// of the last entry in HMetrics
        /// </summary>
        public IReadOnlyList<short> LeftSideBearings { get; init; }
    }
}
