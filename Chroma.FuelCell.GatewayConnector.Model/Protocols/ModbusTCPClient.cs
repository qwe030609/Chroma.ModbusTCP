using System.Net.Sockets;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Modbus client proxy
    /// </summary>
    internal class ModbusTCPClient : IProtocol
    {
        internal ModbusTCPClient()
        {
            
        }

        /// <summary>
        /// The reference to the codec to be used
        /// </summary>
        //public IProtocolCodec Codec { get; private set; }

        internal event ModbusCommand.OutgoingData OutgoingData;
        internal event ModbusCommand.IncommingData IncommingData;
        internal void OnOutgoingData(byte[] data)
        {
            if (OutgoingData != null) OutgoingData(data);
        }

        internal void OnIncommingData(byte[] data, int len)
        {
            if (IncommingData != null) IncommingData(data, len);
        }

        /// <summary>
        /// The address of the remote device
        /// </summary>
        internal byte Address { get; set; }



        /// <summary>
        /// Entry-point for submitting any command
        /// </summary>
        /// <param name="port">The client port for the transport</param>
        /// <param name="command">The command to be submitted</param>
        /// <returns>The result of the query</returns>
        internal ResponseWrapper ExecuteGeneric(
            Socket port,
            ModbusCommand command)
        {
            WrapperDataBase data = new WrapperDataBase(this);
            data.UserData = command;
            ModbusTcpCodec.ClientEncode(data);

            IpClient.Port = port;
            ResponseWrapper resVal = IpClient.Query(data);
            if (data.OutgoingData != null)
                OnOutgoingData(data.OutgoingData.ToArray());
            if (data.IncomingData != null)
                OnIncommingData(data.IncomingData.ToArray(), data.IncomingData.Length);
            return resVal;
        }

        //public void ExecuteAsync(
        //    ICommClient port,
        //    ModbusCommand command)
        //{
        //    var data = new ClientCommData(this);
        //    data.UserData = command;
        //    Codec.ClientEncode(data);
        //    port.QueryAsync(data);
        //}

    }
}
