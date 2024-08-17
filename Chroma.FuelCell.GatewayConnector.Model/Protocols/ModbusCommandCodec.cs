using System.Collections.Generic;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Provide the abstraction for any Modbus command codec
    /// </summary>
    internal class ModbusCommandCodec
    {
        #region Client codec

        internal TagData GetCurrTagDataInfo()
        {
            return TagStaticDatas.currTagData;
        }

        /// <summary>
        /// Encode the client-side command toward the remote slave device
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal virtual void ClientEncode(ModbusCommand command, ByteArrayWriter body) {}

        /// <summary>
        /// Decode the incoming data from the remote slave device 
        /// to a client-side command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal virtual void ClientDecode(ModbusCommand command, ByteArrayReader body) {}

        #endregion
    }
}
