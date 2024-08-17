namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Base class for carrying data for any query against the "DataWrapper" layer
    /// (both client and server)
    /// </summary>
    internal class WrapperDataBase
    {
        internal WrapperDataBase(IProtocol protocol)
        {
            OwnerProtocol = protocol;
        }

        internal IProtocol OwnerProtocol { get; set; }

        /// <summary>
        /// Allow to hold extra data around the query roundtrip
        /// </summary>
        internal object UserData { get; set; }

        /// <summary>
        /// Data outgoing from the local application, toward the remote point
        /// </summary>
        internal ByteArrayReader OutgoingData { get; set; }

        /// <summary>
        /// Data incoming from the remote point, toward the local application
        /// </summary>
        internal ByteArrayReader IncomingData { get; set; }

        /// <summary>
        /// Default timeout for the current query
        /// </summary>
        internal int Timeout = 1000;  //ms

        /// <summary>
        /// Number of retries for re-sending the request
        /// </summary>
        internal int Retries = 3;
    }
}
