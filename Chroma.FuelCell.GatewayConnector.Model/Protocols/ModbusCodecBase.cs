using System.Collections.Generic;
using System.Data.Odbc;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    internal class ModbusCodecBase
    {
        static ModbusCodecBase()
        {
            //fill the local array with the curretly supported commands
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.ReadCoils] = new ModbusCodecReadMultipleDiscretes();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.ReadDiscreteInputs] = new ModbusCodecReadMultipleDiscretes();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.ReadHoldingRegisters] = new ModbusCodecReadMultipleRegisters();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.ReadInputRegister] = new ModbusCodecReadMultipleRegisters();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.WriteSingleCoil] = new ModbusCodecWriteSingleDiscrete();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.WriteSingleRegister] = new ModbusCodecWriteSingleRegister();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.WriteMultipleCoils] = new ModbusCodecWriteMultipleDescretes();
            CommandCodecs[(byte)ModbusTCPProtocol.FunctionCode.WriteMultipleRegisters] = new ModbusCodecWriteMultipleRegisters();
        }


        internal static readonly Dictionary<byte, ModbusCommandCodec> CommandCodecs = new Dictionary<byte, ModbusCommandCodec>(9);
        //public static readonly ModbusCommandCodec[] CommandCodecs = new ModbusCommandCodec[36];



        /// <summary>
        /// Append the typical header for a request command (master-side)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PushRequestHeader(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            body.WriteUInt16BE((ushort)command.StartingAddress);
            if (command.FunctionCode == 05 || command.FunctionCode == 06)
            {
                body.WriteInt16BE((short)command.Data[0]);
            }
            else
            {
                body.WriteInt16BE((short)command.Quantity);
            }
        }



        /// <summary>
        /// Extract the typical header for a request command (server-side)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PopRequestHeader(
            ModbusCommand command,
            ByteArrayReader body)
        {
            command.StartingAddress = body.ReadUInt16BE();
            command.Quantity = body.ReadInt16BE();
        }



        /// <summary>
        /// Helper for packing the discrete data outgoing as a bit-array
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PushDiscretes(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            var count = ((byte)((command.Quantity + 7) / 8));
            var wholeWords = command.Quantity / 16;
            var remainingBits = command.Quantity % 16;
            body.WriteByte(count);
            int k;
            for (k = 0; k < wholeWords; k++)
            {
                var hb = (byte)(command.Data[k] >> 8);
                var lb = (byte)(command.Data[k] & 0x00FF);
                body.WriteByte(hb);
                body.WriteByte(lb);
            }
            if (remainingBits > 0)
            {
                byte bitMask = 1;
                byte cell = 0;
                byte currentByte = (byte)(command.Data[k] >> 8);
                for (int j = 0; j < remainingBits; j++)
                {
                    if (j == 8)
                    {
                        body.WriteByte(cell);
                        currentByte = (byte)(command.Data[k] & 0x00FF);
                        bitMask = 1;
                        cell = 0;
                    }
                    cell |= (byte)(currentByte & bitMask);
                    bitMask = (byte)(bitMask << 1);
                }
                body.WriteByte(cell);
            }
        }

        /// <summary>
        /// Helper for unpacking discrete data incoming as a bit-array
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PopDiscretes(
            ModbusCommand command,
            ByteArrayReader body)
        {
            var byteCount = body.ReadByte();

            var count = command.Quantity;
            command.Data = new ushort[count];
            command.QueryTotalLength += (byteCount + 1);

            int k = 0;
            while (body.EndOfBuffer == false)
            {
                if (command.Quantity <= k)
                    break;
                byte hb = body.CanRead(1) ? body.ReadByte() : (byte)0;
                byte lb = body.CanRead(1) ? body.ReadByte() : (byte)0;
                //command.Data[k++] = (ushort)((hb << 8) | lb);
                command.Data[k++] = (ushort)(hb | lb);
                //int n = count <= 8 ? count : 8;
                //count -= n;
                //for (int i = 0; i < n; i++)
                //    command.Data[k++] = (ushort)(cell & (1 << i));
            }
        }

    }
}
