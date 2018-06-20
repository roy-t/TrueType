# Roy-T.TrueType

*Current version: 0.1.0*

A TrueType parser for reading, glyphIds, names, descriptions, and kerning information from TrueType fonts. Supports `.NetStandard 1.6`, `.Net 4.6.1` and higher.


You can directly add this library to your project using [NuGet](https://www.nuget.org/packages/RoyT.TrueType/):

```
Install-Package RoyT.TrueType
```



For more information please visit my blog at http://roy-t.nl.

To learn more about the TrueType font format and terminology used here see the [Open Type specification](https://docs.microsoft.com/en-us/typography/opentype/spec/).


## Why choose this library?
- It can read all formats of the Cmap, Kern, and Name tables that exist in the fonts that come with a standard Windows 10 installation. This means you can use this library to read
  - The glyph ids of all characters supported by the font
  - (Localized) font names, font families, descriptions, and more
- It exposes both an easy to use API to directly get useful information from these tables, but also exposes the tables themselves so you add your own interpretation of the data
- It works on both `.Net Core 1.0` and `.Net 4.6.1` and higher by targetting `.Net standard 1.6`

## Limitations
- This library aims to read meta-information from .ttf files, it does not give you enough information to render glyphs yourself. For that you need OpenType and HarfBuzz.



## Supported Cmap formats
[Reference](https://docs.microsoft.com/en-us/typography/opentype/spec/cmap)

| Subtable format | supported/unsupported | remarks |
|--- | --- | --- |
| Segmented mapping to delta values | supported | Most common |
| Trimmed table mapping | supported | |
| Segmented coverage | supported | |
| Byte encoding table | supported | Least common |
| High-byte mapping through table | unsupported | I have not seeen a font that uses this yet, samples welcome |
| Mixed 16-bit and 32-bit coverage | unsupported | I have not seeen a font that uses this yet, samples welcome |
| Trimmed array | unsupported | I have not seeen a font that uses this yet, samples welcome |
| Many-to-one range mappings | unsupported | I have not seeen a font that uses this yet, samples welcome |
| Unicode Variation Sequences | unsupported | Specifies variations of a single glyph |


## Usage example

```csharp
var font = TrueTypeFont.FromFile(@"C:\Windows\Fonts\arial.ttf");

// Using the helper functions
var glyphIndex = GlyphHelper.GetGlyphIndex('A', font);  // 36
var horizontalKerning = KerningHelper.GetHorizontalKerning('A', 'W', 1.0f, font);   // -76
var name = NameHelper.GetName(NameId.FontSubfamilyName, new CultureInfo("nl-NL"), font); // Standaard

// Diving in deep yourself to get some specific information is also possible
if (font.KernTable.SubtableCount > 0)
{
    var leftCode = GlyphHelper.GetGlyphIndex(left, font);
    var rightCode = GlyphHelper.GetGlyphIndex(right, font);

    foreach (var subTable in font.KernTable.Subtables)
    {
        if (subTable.Format0 != null && subTable.Direction == Direction.Vertical
            && subTable.Values == Values.Kerning)
        {
            var pair = new KerningPair((ushort)leftCode, (ushort)rightCode);

            if (subTable.Format0.Map.TryGetValue(pair, out var value))
            {
                // Do something
            }
        }
    }
}
```