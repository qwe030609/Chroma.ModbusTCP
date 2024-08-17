namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus codec for commands: write multiple discrete data
    /// </summary>
    internal class ModbusCodecWriteMultipleDescretes : ModbusCommandCodec
    {
        #region Client codec

        internal override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            ModbusCodecBase.PushRequestHeader(
                command,
                body);

            ModbusCodecBase.PushDiscretes(
                command,
                body);
        }


        internal override void ClientDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
            ModbusCodecBase.PopRequestHeader(
                command,
                body);
        }

        #endregion
    }
}
