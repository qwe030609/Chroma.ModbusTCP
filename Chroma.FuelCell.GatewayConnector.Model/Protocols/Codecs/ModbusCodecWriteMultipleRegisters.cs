namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus codec for commands: writing multiple register data
    /// </summary>
    internal class ModbusCodecWriteMultipleRegisters : ModbusCommandCodec
    {
        #region Client codec
        internal override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            ModbusCodecBase.PushRequestHeader(
                command,
                body);

            int count = command.Quantity;
            body.WriteByte((byte)(count * 2));

            //for (int i = 0; i < count; i++)
            //    body.WriteUInt16BE(command.Data[i]);

            if (GetCurrTagDataInfo().IsLittleEndian == true && GetCurrTagDataInfo().IsReverse == false)
            {
                for (int i = count - 1; i >= 0; i--)
                    body.WriteUInt16LE(command.Data[i]);
            }
            else if (GetCurrTagDataInfo().IsLittleEndian == false && GetCurrTagDataInfo().IsReverse == true)
            {
                for (int i = count - 1; i >= 0; i--)
                    body.WriteUInt16BE(command.Data[i]);
            }
            else if (GetCurrTagDataInfo().IsLittleEndian == true && GetCurrTagDataInfo().IsReverse == true)
            {
                for (int i = 0; i < count; i++)
                    body.WriteUInt16LE(command.Data[i]);
            }
            else
            {
                for (int i = 0; i < count; i++)
                    body.WriteUInt16BE(command.Data[i]);
            }
        }


        internal override void ClientDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
            //not used
        }

        #endregion
    }
}
