using System;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    internal class ModbusTcpCodec : ModbusCodecBase
    {
        #region IProtocolCodec members

        internal static void ClientEncode(WrapperDataBase data)
        {
            ModbusTCPClient client = (ModbusTCPClient) data.OwnerProtocol;
            ModbusCommand command = (ModbusCommand)data.UserData;
            byte fncode = command.FunctionCode;

            //encode the command body, if applies
            ByteArrayWriter body = new ByteArrayWriter();
            ModbusCommandCodec codec = CommandCodecs[fncode];
            if (codec != null)
                codec.ClientEncode(command, body);

            //calculate length field
            var length = 2 + body.Length;

            //create a writer for the outgoing data
            ByteArrayWriter writer = new ByteArrayWriter();

            //transaction-id 
            writer.WriteUInt16BE((ushort)command.TransId);

            //protocol-identifier (always zero)
            writer.WriteInt16BE(0);

            //message length
            writer.WriteInt16BE((short)length);

            //unit identifier (address)
            writer.WriteByte(client.Address);

            //function code
            writer.WriteByte(fncode);

            //body data
            writer.WriteBytes(body);

            data.OutgoingData = writer.ToReader();
        }



        internal static ResponseWrapper ClientDecode(WrapperDataBase data)
        {
            ModbusTCPClient client = (ModbusTCPClient) data.OwnerProtocol;
            ModbusCommand command = (ModbusCommand)data.UserData;
            ByteArrayReader incoming = data.IncomingData;

            //validate header first
            if (incoming.Length >= 6 &&
                incoming.ReadUInt16BE() == (ushort)command.TransId &&
                incoming.ReadInt16BE() == 0     //protocol-identifier
                )
            {
                //message length
                var length = incoming.ReadInt16BE();

                //validate address
                if (incoming.Length >= (length + 6) &&
                    incoming.ReadByte() == client.Address
                    )
                {
                    //validate function code
                    var fncode = incoming.ReadByte();

                    if ((fncode & 0x7F) == command.FunctionCode)
                    {
                        if (fncode <= 0x7F)
                        {
                            //
                            ByteArrayReader body = new ByteArrayReader(incoming.ReadToEnd());
                            
                            //encode the command body, if applies
                            ModbusCommandCodec codec = CommandCodecs[fncode];
                            if (codec != null)
                                codec.ClientDecode(command, body);

                            return new ResponseWrapper(
                                data,
                                ResponseWrapper.Ack);
                        }
                        else
                        {
                            //exception
                            command.ExceptionCode = incoming.ReadByte();

                            return new ResponseWrapper(
                                data,
                                ResponseWrapper.Critical);
                        }
                    }
                }
            }

            return new ResponseWrapper(
                data,
                ResponseWrapper.Unknown);
        }

        #endregion

    }
}
