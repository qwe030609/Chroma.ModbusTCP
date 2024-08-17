using System;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Helpers for the byte arrays
    /// </summary>
    public static class ByteArrayHelpers
    {
        /// <summary>
        /// Create a copy of the given byte array
        /// </summary>
        /// <param name="source">The source byte-array</param>
        /// <returns>The resulting copy</returns>
        public static byte[] ToArray(this byte[] source)
        {
            return source.ToArray(
                0,
                source.Length);
        }

        /// <summary>
        /// Create a new byte array starting from the defined
        /// segment of the given one
        /// </summary>
        /// <param name="source">The source byte-array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <param name="count">The number of the bytes to be considered</param>
        /// <returns>The resulting byte-array</returns>
        public static byte[] ToArray(
            this byte[] source,
            int offset,
            int count)
        {
            var target = new byte[count];
            Array.Copy(
                source,
                offset,
                target,
                0,
                count);

            return target;
        }


        /// <summary>
        /// Read an <see cref="System.Int16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int16 ReadInt16LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset+2)
                return 0;

            return (Int16)(
                buffer[offset] |
                (buffer[offset + 1] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.Int16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt16LE(
            byte[] buffer,
            int offset,
            Int16 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
        }



        /// <summary>
        /// Read an <see cref="System.Int16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int16 ReadInt16BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset+2)
                return 0;

            return (Int16)(
                buffer[offset + 1] |
                (buffer[offset] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.Int16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt16BE(
            byte[] buffer,
            int offset,
            Int16 value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
        }



        #region NON CLS-Compliant members

#pragma warning disable 3001, 3002

        /// <summary>
        /// Read an <see cref="System.UInt16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static UInt16 ReadUInt16LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 2)
                return 0;
            return (UInt16)(
                buffer[offset] |
                (buffer[offset + 1] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.UInt16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteUInt16LE(
            byte[] buffer,
            int offset,
            UInt16 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
        }



        /// <summary>
        /// Read an <see cref="System.UInt16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static UInt16 ReadUInt16BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 2)
                return 0;
            return (UInt16)(
                buffer[offset + 1] |
                (buffer[offset] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.UInt16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteUInt16BE(
            byte[] buffer,
            int offset,
            UInt16 value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
        }

#pragma warning restore 3001, 3002

        #endregion



        /// <summary>
        /// Read an <see cref="System.Int32"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int32 ReadInt32LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 4)
                return 0;
            return (Int32)(
                buffer[offset] |
                (buffer[offset + 1] << 8) |
                (buffer[offset + 2] << 16) |
                (buffer[offset + 3] << 24));
        }



        /// <summary>
        /// Write an <see cref="System.Int32"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt32LE(
            byte[] buffer,
            int offset,
            Int32 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
        }



        /// <summary>
        /// Read an <see cref="System.Int32"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int32 ReadInt32BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 4)
                return 0;

            return (Int32)(
                buffer[offset + 3] |
                (buffer[offset + 2] << 8) |
                (buffer[offset + 1] << 16) |
                (buffer[offset] << 24));
        }



        /// <summary>
        /// Write an <see cref="System.Int32"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt32BE(
            byte[] buffer,
            int offset,
            Int32 value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }

        /// <summary>
        /// Read an <see cref="System.UInt32"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static UInt32 ReadUInt32LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 4)
                return 0;
            return (UInt32)(
                buffer[offset] |
                (buffer[offset + 1] << 8) |
                (buffer[offset + 2] << 16) |
                (buffer[offset + 3] << 24));
        }



        /// <summary>
        /// Write an <see cref="System.UInt32"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteUInt32LE(
            byte[] buffer,
            int offset,
            UInt32 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
        }



        /// <summary>
        /// Read an <see cref="System.UInt32"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static UInt32 ReadUInt32BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 4)
                return 0;

            return (UInt32)(
                buffer[offset + 3] |
                (buffer[offset + 2] << 8) |
                (buffer[offset + 1] << 16) |
                (buffer[offset] << 24));
        }



        /// <summary>
        /// Write an <see cref="System.UInt32"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteUInt32BE(
            byte[] buffer,
            int offset,
            UInt32 value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }

    }
}
