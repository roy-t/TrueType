namespace RoyT.TrueType.Tables.Cmap
{
    public interface ICmapSubtable
    {
        uint GetGlyphIndex(char c);
    }
}
