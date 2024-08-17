namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus codec for commands: writing single register datum
    /// </summary>
    internal class ModbusCodecWriteSingleRegister : ModbusCommandCodec
    {
        #region Client codec
        internal override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            body.WriteUInt16BE((ushort)command.StartingAddress);
            body.WriteUInt16BE(command.Data[0]);
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
