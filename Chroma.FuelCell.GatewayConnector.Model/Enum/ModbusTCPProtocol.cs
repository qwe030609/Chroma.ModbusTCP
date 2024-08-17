using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    public class ModbusTCPProtocol
    {
        public enum FunctionCode : byte
        {
            [Description("讀線圈(Digital Output)")]
            ReadCoils = 0x01,
            [Description("讀離散量輸入(Digital Input)")]
            ReadDiscreteInputs = 0x02,
            [Description("讀保持寄存器(Analog Output)")]
            ReadHoldingRegisters = 0x03,
            [Description("讀輸入寄存器(Analog Input)")]
            ReadInputRegister = 0x04,
            [Description("單一線圈寫入")]
            WriteSingleCoil = 0x05,
            [Description("單一寄存器寫入")]
            WriteSingleRegister = 0x06,
            [Description("多重線圈寫入")]
            WriteMultipleCoils = 0x0F,
            [Description("多重寄存器寫入")]
            WriteMultipleRegisters = 0x10,
        }

        public enum TagDataType : int
        {
            [Description("16-bit unsigned integer. Can be stored in a single Modbus register.")]
            UINT16 = 1,
            [Description("32-bit unsigned integer. Occupies two contiguous registers on the hardware device.")]
            UINT32 = 2,
            [Description("64-bit unsigned integer. Occupies four contiguous registers on the hardware device.")]
            UINT64 = 3,
            [Description("16-bit signed integer.  Can be stored in a single Modbus register.")]
            INT16 = 4,
            [Description("32-bit signed integer. Occupies two contiguous registers on the hardware device.")]
            INT32 = 5,
            [Description("64-bit signed integer. Occupies four contiguous registers on the hardware device.")]
            INT64 = 6,
            [Description("32-bit floating point. Occupies two contiguous registers on the hardware device.")]
            FLOAT = 7,
            [Description("64-bit floating point. Occupies four contiguous registers on the hardware device.")]
            DOUBLE = 8,
            [Description("16-bit BCD integer. Can be stored in a single Modbus register.")]
            BCD16 = 9,
            [Description("32-bit BCD integer. Occupies two contiguous registers on the hardware device.")]
            BCD32 = 10,
            [Description("64-bit BCD integer. Occupies four contiguous registers on the hardware device.")]
            BCD64 = 11,
            [Description("digital data (bool)")]
            BOOL = 12,
        }

        public enum TagType : int
        {
            [Description("線圈型態")]
            COIL = 0,
            [Description("離散輸入型態")]
            DISCRETE_INPUT = 1,
            [Description("保持寄存器型態")]
            HOLDING_REGISTER = 4,
            [Description("輸入寄存器型態")]
            INPUT_REGISTER = 3,
        }

        public enum BitType : int
        {
            [Description("16位元")]
            BIT_16 = 2,
            [Description("32位元")]
            BIT_32 = 4,
            [Description("64位元")]
            BIT_64 = 8,
        }

        public enum ExceptionCode : byte
        {
            [Description("The function code is unknown by the server")]
            ILLEGAL_FUNCTION = 0x01,
            [Description("Dependant on the request")]
            ILLEGAL_DATA_ADDRESS = 0x02,
            [Description("Dependant on the request")]
            ILLEGAL_DATA_VALUE = 0x03,
            [Description("The server failed during the execution")]
            SERVER_DEVICE_FAILURE = 0x04,
            [Description("The server accepted the service invocation but the service requires a relatively long time to execute.The" +
                         "server therefore returns only an acknowledgement of the service invocation receipt.")]
            ACKNOWLEDGE = 0x05,
            [Description("The server was unable to accept the MB Request PDU." + 
                         "The client application has the responsibility of deciding if" + 
                         "and when to re - send the request.")]
            SERVER_DEVICE_BUSY = 0x06,
            [Description("Gateway paths not available.")]
            GATEWAY_PATH_UNAVAILABLE = 0x0A,
            [Description("The targeted device failed to respond. The gateway generates this exception")]
            GATEWAY_TARGET_DEVICE_FAILED_TO_RESPOND = 0x0B,
        }
    }
}
