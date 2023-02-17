using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
        public void ShouldParseTrueTypeFonts()
        {
            List<TrueTypeFont> systemFonts = new();
            DirectoryInfo fontdir = new(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

            var fontFiles = fontdir.EnumerateFiles().
                Where(f => f.Extension.ToLowerInvariant() is ".ttf");
            foreach (var file in fontFiles)
            {
                var font = TrueTypeFont.FromFile(file.FullName);
                systemFonts.Add(font);
            }

            Assert.NotEmpty(systemFonts);
        }

        /// <summary>
        /// Integration tests that parses all fonts installed with Windows        
        /// </summary>
        [Fact]
        public void ShouldParseTrueTypeCollections()
        {
            List<TrueTypeFont> systemFonts = new();
            DirectoryInfo fontdir = new(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));

            var fontCollectionFiles = fontdir.EnumerateFiles().
                Where(f => f.Extension.ToLowerInvariant() is ".ttc");
            foreach (var file in fontCollectionFiles)
            {
                var fonts = TrueTypeFont.FromCollectionFile(file.FullName);
                systemFonts.AddRange(fonts);
            }

            Assert.NotEmpty(systemFonts);
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

            Assert.Equal(1, font.VheaTable.Version);
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
