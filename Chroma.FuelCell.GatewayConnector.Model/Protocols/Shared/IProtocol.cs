namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Minimum contract required for a protocol implementation
    /// </summary>
    internal abstract class IProtocol
    {
        /// <summary>
        /// Access to the protocol codec
        /// </summary>
        event ModbusCommand.OutgoingData OutgoingData;
        event ModbusCommand.IncommingData IncommingData;

        //void OnOutgoingData(byte[] data);
        //void OnIncommingData(byte[] data, int len);
    }
}
