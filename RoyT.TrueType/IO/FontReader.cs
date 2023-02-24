using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;

namespace RoyT.TrueType.IO
{
    /// <summary>
    /// Extended BinaryReader with helper methods for reading number and text formats
    /// common in TrueType files
    /// </summary>
    public sealed class FontReader : BinaryReader
    {
        public FontReader(Stream stream)
            : base(stream) { }

        public long Position => this.BaseStream.Position;

        public short ReadInt16BigEndian()
        {
            var bytes = ReadBigEndian(2);
            return BitConverter.ToInt16(bytes, 0);
        }               

        public ushort ReadUInt16BigEndian()
        {
            var bytes = ReadBigEndian(2);
            return BitConverter.ToUInt16(bytes, 0);
        }       

        public uint ReadUInt32BigEndian()
        {
            var bytes = ReadBigEndian(4);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public long ReadInt64BigEndian()
        {
            var bytes = ReadBigEndian(8);
            return BitConverter.ToInt64(bytes, 0);
        }

        public string ReadAscii(int length)
        {            
            var bytes = this.ReadBytes(length);
            return Encoding.ASCII.GetString(bytes);
        }

        public string ReadBigEndianUnicode(int length)
        {
            var bytes = ReadBytes(length);
            return Encoding.BigEndianUnicode.GetString(bytes).Replace("\0", string.Empty);
        }
     
        public BitArray ReadBitsBigEndian(int numberOfBytes)
        {
            var bytes = ReadBigEndian(numberOfBytes);
            return new BitArray(bytes);
        }

        /// <summary>
        /// Reads a 32 bit signed fixed point number (16.16)        
        /// </summary>
        public float ReadFixedBigEndian()
        {
            var dec = this.ReadInt16BigEndian();
            var frac = this.ReadUInt16BigEndian();
            return dec + frac / 65535f;
        }

        private byte[] ReadBigEndian(int count)
        {
            var data = base.ReadBytes(count);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }

            return data;
        }

        public void Seek(long offset, SeekOrigin seekOrigin = SeekOrigin.Begin)
        {            
            this.BaseStream.Seek(offset, seekOrigin);
        }        
    }
}
