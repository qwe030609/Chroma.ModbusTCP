namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus codec for commands: reading multiple discrete data
    /// </summary>
    internal class ModbusCodecReadMultipleDiscretes : ModbusCommandCodec
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
            ModbusCodecBase.PopDiscretes(
                command,
                body);
        }

        #endregion
    }
}
