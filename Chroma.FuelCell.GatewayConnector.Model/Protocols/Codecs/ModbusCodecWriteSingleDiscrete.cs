namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus codec for commands: writing single discrete datum
    /// </summary>
    internal class ModbusCodecWriteSingleDiscrete : ModbusCommandCodec
    {
        #region Client codec
        internal override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            body.WriteUInt16BE((ushort)command.StartingAddress);

            int value = command.Data[0] != 0
                ? 0xFF00
                : 0;

            body.WriteInt16BE((short)value);
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
