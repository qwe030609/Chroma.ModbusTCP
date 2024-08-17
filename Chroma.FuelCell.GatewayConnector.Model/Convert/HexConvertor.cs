using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    public class HexConvertor
    {
        #region Hex to other types
        public static Int16 HexToINT16(Int16 hexdec)
        {
            return Convert.ToInt16(hexdec.ToString(), 16);
        }

        public static Int32 HexToINT32(Int32 hexdec)
        {
            return Convert.ToInt32(hexdec.ToString(), 16);
        }

        public static Int64 HexToINT64(Int64 hexdec)
        {
            return Convert.ToInt64(hexdec.ToString(), 16);
        }
        #endregion

        #region Other types to Hex
        public static Int16 INT16ToHex(Int16 dec)
        {
            Int16 hexdec;

            //// Convert the hex string back to the number
            //hexdec = Int16.Parse(dec.ToString("X"), System.Globalization.NumberStyles.HexNumber);
            //return hexdec;

            if (Int16.TryParse(dec.ToString("X"), out hexdec))
                //if (Int16.TryParse(Convert.ToString(short.Parse(dec.ToString()), 16), out hexdec))
                return hexdec;
            else
                return 0;
        }

        public static Int32 INT32ToHex(Int32 dec)
        {
            Int32 hexdec;
            if (Int32.TryParse(dec.ToString("X"), out hexdec))
                return hexdec;
            else
                return 0;
        }

        public static Int64 INT64ToHex(Int64 dec)
        {
            Int64 hexdec;
            if (Int64.TryParse(dec.ToString("X"), out hexdec))
                return hexdec;
            else
                return 0;
        }
        #endregion
    }
}
