using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Chroma.FuelCell.GatewayConnector
{
    public enum WriteTagType
    {
        // Write types for IO tag in Modbus Devices   
        [Description("單一線圈寫入")]
        WRITE_SINGLE_COIL = 1,
        [Description("多重線圈寫入")]
        WRITE_MULTIPLE_COILS = 2,
        [Description("單一寄存器寫入")]
        WRITE_SINGLE_REGISTER = 3,
        [Description("多重寄存器寫入")]
        WRITE_MULTIPLE_REGISTER = 4,
    }
}
