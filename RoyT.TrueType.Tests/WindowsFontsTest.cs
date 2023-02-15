using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using RoyT.TrueType.Helpers;
using RoyT.TrueType.Tables.Name;
using Xunit;

namespace RoyT.TrueType.Tests
{    
    public class WindowsFontsTest
    {
        /// <summary>
        /// Integration tests that parses all fonts installed with Windows        
        /// </summary>
        [Fact]
        public void ShouldParseWindowsFonts()
        {
            var fonts = new List<TrueTypeFont>();
            foreach (var file in Directory.EnumerateFiles(@"C:\Windows\Fonts"))
            {
                if (file.EndsWith(".ttf") && !file.EndsWith("mstmc.ttf")) // mstmc.ttf is not a regular font file
                {
                    var font = TrueTypeFont.FromFile(file);
                    fonts.Add(font);
                }
            }

            Assert.NotEmpty(fonts);
        }


        /// <summary>
        /// Smoke test for checking glyph indices     
        /// </summary>
        [Fact]
        public void ShouldGetGlyph()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\arial.ttf");

            var glyphIndex = GlyphHelper.GetGlyphIndex('A', font);
            Assert.NotEqual((uint)0, glyphIndex);
        }

        /// <summary>
        /// Smoke test for checking kerning info
        /// </summary>
        [Fact]
        public void ShouldGetKerning()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\arial.ttf");            

            var horizontalKerning = KerningHelper.GetHorizontalKerning('A', 'W', font);
            Assert.True(horizontalKerning < 0);

            horizontalKerning = KerningHelper.GetHorizontalKerning('T', 'T', font);
            Assert.Equal(0, horizontalKerning);
        }

        /// <summary>
        /// Smoke test for checking name info
        /// </summary>
        [Fact]
        public void ShouldGetName()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\arial.ttf");

            var name = NameHelper.GetName(NameId.FontSubfamilyName, new CultureInfo("nl-NL"), font);

            Assert.Equal("Standaard", name);
        }

        [Fact]
        public void ShouldGetHhea()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\arial.ttf");

            Assert.Equal(1, font.HheaTable.MajorVersion);
            Assert.Equal(0, font.HheaTable.MinorVersion);
        }

        [Fact]
        public void ShouldGetHmtx()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\arial.ttf");

            Assert.Equal(font.HheaTable.NumberOfHMetrics, font.HmtxTable.HMetrics.Count);
        }

        [Fact]
        public void ShouldGetVhea()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\malgun.ttf");

            Assert.Equal(1, font.VheaTable.MajorVersion);
            Assert.Equal(0, font.VheaTable.MinorVersion);
        }

        [Fact]
        public void ShouldGetVmtx()
        {
            var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\malgun.ttf");

            Assert.Equal(font.VheaTable.NumberOfVMetrics, font.VmtxTable.VMetrics.Count);
            Assert.Equal(font.MaxpTable.NumGlyphs - font.VheaTable.NumberOfVMetrics, font.VmtxTable.TopSideBearings.Count);
        }
    }
}
