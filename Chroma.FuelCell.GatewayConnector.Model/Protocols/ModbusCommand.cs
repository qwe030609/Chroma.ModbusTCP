namespace Chroma.FuelCell.GatewayConnector.Model
{
    /// <summary>
    /// Provide the base for any Modbus command
    /// </summary>
    internal class ModbusCommand
    {
        ///**
        // * Class 0 functions
        // **/
        //public const byte FuncReadMultipleRegisters = 3;
        //public const byte FuncWriteMultipleRegisters = 16;



        ///**
        // * Class 1 functions
        // **/
        //public const byte FuncReadCoils = 1;
        //public const byte FuncReadInputDiscretes = 2;
        //public const byte FuncReadInputRegisters = 4;
        //public const byte FuncWriteCoil = 5;
        //public const byte FuncWriteSingleRegister = 6;
        //public const byte FuncReadExceptionStatus = 7;

        ///// <summary>Constant for exception connection lost.</summary>
        //public const byte ExceptionConnectionLost = 254;
        ///// <summary>Constant for exception send fail.</summary>
        //public const byte SendFail = 100;
        //public const byte ExceptionOffset = 128;

        ///**
        // * Class 2 functions
        // **/
        //public const byte FuncForceMultipleCoils = 15;

        //public const byte FuncReadCustom = 104;

        ///**
        // * Exceptions
        // **/
        //public const byte ErrorIllegalFunction = 1;
        //public const byte ErrorIllegalDataAddress = 2;
        //public const byte ErrorIllegalDataValue = 3;
        //public const byte ErrorIllegalResponseLength = 4;
        //public const byte ErrorAcknowledge = 5;
        //public const byte ErrorSlaveDeviceBusy = 6;
        //public const byte ErrorNegativeAcknowledge = 7;
        //public const byte ErrorMemoryParity = 8;

        internal delegate void OutgoingData(byte[] data);
        internal delegate void IncommingData(byte[] data, int len);

        internal ModbusCommand(byte fc)
        {
            FunctionCode = fc;
        }



        /// <summary>
        /// The function code of the command
        /// </summary>
        internal readonly byte FunctionCode;

        /// <summary>
        /// The transaction-id of the request (often is zero)
        /// </summary>
        internal int TransId;

        /// <summary>
        /// The starting address for the involved resources
        /// </summary>
        internal int StartingAddress { get; set; }

        /// <summary>
        /// The number of involved resources
        /// </summary>
        internal int Quantity { get; set; }

        /// <summary>
        /// The data submitted/received
        /// </summary>
        /// <remarks>
        /// As a simplification, the data type is only the <see cref="System.UInt16"/>,
        /// which should cover most of the commands.
        /// For discrete (i.e. bool) data, consider one data per cell,
        /// using zero/non-zero as boolean criteria
        /// </remarks>
        internal ushort[] Data { get; set; }

        /// <summary>
        /// If non-zero, indicates the exception raised by the server
        /// </summary>
        internal byte ExceptionCode { get; set; }

        /// <summary>
        /// Estimated total length (bytes) of the received command
        /// </summary>
        internal int QueryTotalLength;

    }
}
