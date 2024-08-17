namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus codec for commands: reading multiple register data
    /// </summary>
    internal class ModbusCodecReadMultipleRegisters : ModbusCommandCodec
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

            if (GetCurrTagDataInfo().IsLittleEndian == true && GetCurrTagDataInfo().IsReverse == false)
            {
                for (int i = count - 1; i >= 0; i--)
                    command.Data[i] = body.ReadUInt16LE();
            }
            else if (GetCurrTagDataInfo().IsLittleEndian == false && GetCurrTagDataInfo().IsReverse == true)
            {
                for (int i = count - 1; i >= 0; i--)
                    command.Data[i] = body.ReadUInt16BE();
            }
            else if (GetCurrTagDataInfo().IsLittleEndian == true && GetCurrTagDataInfo().IsReverse == true)
            {
                for (int i = 0; i < count; i++)
                    command.Data[i] = body.ReadUInt16LE();
            }
            else
            {
                for (int i = 0; i < count; i++)
                    command.Data[i] = body.ReadUInt16BE();
            }
        }

        #endregion
    }
}
