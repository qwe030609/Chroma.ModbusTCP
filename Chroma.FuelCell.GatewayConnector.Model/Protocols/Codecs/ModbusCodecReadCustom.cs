using System;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    internal class ModbusCodecReadCustom : ModbusCommandCodec
    {
        #region Client codec

        internal override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            ModbusCodecBase.PushRequestHeader(
                command,
                body);
        }


        internal override void ClientDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
            int count = body.ReadByte() / 2;
            command.Data = new ushort[count];
            for (int i = 0; i < count; i++)
                command.Data[i] = body.ReadUInt16BE();
        }

        #endregion
    }
}