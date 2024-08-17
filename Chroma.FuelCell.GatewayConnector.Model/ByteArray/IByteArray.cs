namespace Chroma.FuelCell.GatewayConnector.Model
{
    internal interface IByteArray
    {
        /// <summary>
        /// Internal facility for the byte array data exchange
        /// </summary>
        /// <remarks>
        /// You should not use this member unless strictly needed
        /// </remarks>
        byte[] Data { get; }
    }
}
