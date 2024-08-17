using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Threading;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    internal class ModbusTCPHelper
    {
        internal static readonly ModbusTCPHelper modbusTCPHelper;
        private static ModbusTCPClient mbTCPClient;
        static ModbusTCPHelper()
        {
            modbusTCPHelper = new ModbusTCPHelper();
            mbTCPClient.OutgoingData += modbusTCPHelper.ClientOutgoingData;
            mbTCPClient.IncommingData += modbusTCPHelper.ClientIncommingData;
        }

        internal static ModbusTCPHelper GetInstance()
        {
            return modbusTCPHelper;
        }

        internal ModbusTCPHelper()
        {
            //isPolling = isPolling_;
            //pollingRate = pollingRate_;
            mbTCPClient = new ModbusTCPClient() { Address = 1 };
            tcpSynBuffer = new ushort[65600];
        }

        ~ModbusTCPHelper()
        {

        }

        #region properties

        #endregion

        #region properties

        protected internal Socket tcpSynSckt;
        protected readonly internal ushort[] tcpSynBuffer;
        internal byte lastReadCommand = 0;

        #region DispatcherTimer for polling (reading)

        //private DispatcherTimer pollingWriteTimer;
        internal DispatcherTimer pollingReadTimer;

        //是否啟用 polling
        static bool isPolling { get; set; }

        //多久 polling 一次;
        static double pollingRate { get; set; }

        #endregion Timers



        internal static int startAddress;

        internal static int dataLength;

        internal static int transactionId = 0;

        internal static object value;

        #endregion

        #region Methods

        public void CreateDispatcherTimer(bool isPolling_, double pollingRate_)
        {
            isPolling = isPolling_;
            pollingRate = pollingRate_;
            pollingReadTimer = new DispatcherTimer();
            pollingReadTimer.Interval = TimeSpan.FromSeconds(pollingRate);
            pollingReadTimer.Tick += pollingReadTimer_Tick;
            pollingReadTimer.Start();
            pollingReadTimer_UpdateStatus();

        }
        private void pollingReadTimer_Tick(object sender, EventArgs e)
        {
            if (lastReadCommand != 0)
                ExecuteReadCommand(lastReadCommand);
        }

        private void pollingReadTimer_UpdateStatus()
        {
            pollingReadTimer.IsEnabled = isPolling;

            if (!pollingReadTimer.IsEnabled)
                lastReadCommand = 0;
        }

        internal void ExecuteReadCommand(byte function)
        {
            lastReadCommand = function;

            try
            {
                ModbusCommand command = new ModbusCommand(function) { StartingAddress = startAddress, Quantity = dataLength, TransId = transactionId++ };
                ResponseWrapper result = mbTCPClient.ExecuteGeneric(tcpSynSckt, command);
                if (result.Status == ResponseWrapper.Ack)
                {
                    command.Data.CopyTo(tcpSynBuffer, startAddress);
                    //UpdateDataTable();
                    LogExtensions.CreateLog(String.Format("Read succeeded: Function code:{0}.", function));
                    //MessageBox.Show(String.Format("Read succeeded: Function code:{0}.", function), "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //AppendLog(String.Format("Failed to execute Read: Error code:{0}", result.Status));
                    LogExtensions.CreateLog(String.Format("Failed to execute Read: Error code:{0}", result.Status));
                    //MessageBox.Show(String.Format("Failed to execute Read: Error code:{0}", result.Status), "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                //AppendLog(ex.Message);
                LogExtensions.CreateLog(ex.Message);
                //MessageBox.Show(ex.Message, "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void ExecuteWriteCommand(byte function)
        {
            try
            {
                ModbusCommand command = new ModbusCommand(function)
                {
                    StartingAddress = startAddress,
                    Quantity = dataLength,
                    TransId = transactionId++,
                    Data = new ushort[dataLength]
                };
                for (int i = 0; i < dataLength; i++)
                {
                    var index = startAddress + i;
                    if (index > tcpSynBuffer.Length)
                    {
                        break;
                    }
                    command.Data[i] = tcpSynBuffer[index];
                }
                ResponseWrapper result = mbTCPClient.ExecuteGeneric(tcpSynSckt, command);
                //AppendLog(result.Status == ResponseWrapper.Ack
                //              ? String.Format("Write succeeded: Function code:{0}", function)
                //              : String.Format("Failed to execute Write: Error code:{0}", result.Status));
                LogExtensions.CreateLog(result.Status == ResponseWrapper.Ack
                                        ? String.Format("Write succeeded: Function code:{0}.", function)
                                        : String.Format("Failed to execute Write: Error code:{0}", result.Status));
                //MessageBox.Show(result.Status == ResponseWrapper.Ack
                //                ? String.Format("Write succeeded: Function code:{0}.", function)
                //                : String.Format("Failed to execute Write: Error code:{0}", result.Status), 
                //                "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //AppendLog(ex.Message);
                LogExtensions.CreateLog(ex.Message);
                //MessageBox.Show(ex.Message, "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void ExecuteWriteSingleCoilCommand()
        {
            try
            {
                var command = new ModbusCommand((int)ModbusTCPProtocol.FunctionCode.WriteSingleCoil)
                {
                    StartingAddress = startAddress,
                    Quantity = 1,
                    TransId = transactionId++,
                    Data = new ushort[1]
                };
                //command.Data[0] = (ushort)(tcpSynBuffer[startAddress] & 0x0100);
                command.Data[0] = (ushort)(int.Parse(value.ToString()));
                var result = mbTCPClient.ExecuteGeneric(tcpSynSckt, command);
                //AppendLog(result.Status == ResponseWrapper.Ack
                //              ? String.Format("Write succeeded: Function code:{0}", (int)ModbusTCPProtocol.FunctionCode.WriteSingleCoil)
                //              : String.Format("Failed to execute Write: Error code:{0}", result.Status));
                LogExtensions.CreateLog(result.Status == ResponseWrapper.Ack
                                        ? String.Format("Write succeeded: Function code:{0}.", (int)ModbusTCPProtocol.FunctionCode.WriteSingleCoil)
                                        : String.Format("Failed to execute Write: Error code:{0}", result.Status));
                //MessageBox.Show(result.Status == ResponseWrapper.Ack
                //                ? String.Format("Write succeeded: Function code:{0}.", (int)ModbusTCPProtocol.FunctionCode.WriteSingleCoil)
                //                : String.Format("Failed to execute Write: Error code:{0}", result.Status),
                //                "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //AppendLog(ex.Message);
                LogExtensions.CreateLog(ex.Message);
                //MessageBox.Show(ex.Message, "Comm Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected void ClientIncommingData(byte[] data, int len)
        {
            StringBuilder hex = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                hex.AppendFormat("{0:x2} ", data[i]);
            }
            //AppendLog(String.Format("RX: {0}", hex));
            //MessageBox.Show(String.Format("RX: {0}", hex), "Comm log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LogExtensions.CreateLog(String.Format("RX: {0}", hex));
        }

        protected void ClientOutgoingData(byte[] data)
        {
            StringBuilder hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2} ", b);

            //AppendLog(String.Format("TX: {0}", hex));
            //MessageBox.Show(String.Format("TX: {0}", hex), "Comm log", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LogExtensions.CreateLog(String.Format("TX: {0}", hex));
        }

        internal object SwapData(TagData td)
        {
            //float r = 0;
            uint twoWords = 0u;
            ulong fourWords = 0ul;

            if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.INT16 || td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT16)
            {
                // To Int16, Uint16
                return tcpSynBuffer[startAddress];
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.INT32 || td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT32)
            {
                // To Int32, Uint32
                return ((uint)tcpSynBuffer[startAddress] << 16) + tcpSynBuffer[startAddress + 1];
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.INT64 || td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT64)
            {
                // To Int64, Uint64
                return ((ulong)tcpSynBuffer[startAddress] << 16 * 3) + ((ulong)tcpSynBuffer[startAddress + 1] << 16 * 2) + ((ulong)tcpSynBuffer[startAddress + 2] << 16 * 1) + tcpSynBuffer[startAddress + 3];
            }
            else if(td.TagDataType == (int)ModbusTCPProtocol.TagDataType.FLOAT)
            {
                // To IEEE-754 single precision float
                twoWords = ((uint)tcpSynBuffer[startAddress] << 16) + tcpSynBuffer[startAddress + 1];
                return ToSingle_IEEE754(twoWords);
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.DOUBLE)
            {
                // To IEEE-754 double precision float
                fourWords = ((ulong)tcpSynBuffer[startAddress] << 16 * 3) + ((ulong)tcpSynBuffer[startAddress + 1] << 16 * 2) + ((ulong)tcpSynBuffer[startAddress + 2] << 16 * 1) + tcpSynBuffer[startAddress + 3];
                return ToDouble_IEEE754(fourWords);
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.BCD16)
            {
                // To BCD 16
                return HexConvertor.INT16ToHex((short)tcpSynBuffer[startAddress]);
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.BCD32)
            {
                // To BCD 32
                return HexConvertor.INT32ToHex((int)((uint)tcpSynBuffer[startAddress] << 16 * 1) + (int)tcpSynBuffer[startAddress + 1]);
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.BCD64)
            {
                // To BCD 64
                return HexConvertor.INT64ToHex((long)((ulong)tcpSynBuffer[startAddress] << 16 * 3) + (long)((ulong)tcpSynBuffer[startAddress + 1] << 16 * 2) + (long) ((ulong)tcpSynBuffer[startAddress + 2] << 16 * 1) + (long)tcpSynBuffer[startAddress + 3]);
            }
            else
            {
                // To Bool
                return tcpSynBuffer[startAddress];
            }
                

            //return r;
        }

        internal ushort[] GetBufferData()
        {
            return tcpSynBuffer;
        }

        private float ToSingle_IEEE754(uint fb)
        {
            //uint fb = Convert.ToUInt32(f);

            int sign = (int)((fb >> 31) & 1);
            int exponent = (int)((fb >> 23) & 0xFF);
            int mantissa = (int)(fb & 0x7FFFFF);

            float fMantissa;
            float fSign = sign == 0 ? 1.0f : -1.0f;

            if (exponent != 0)
            {
                exponent -= 127;
                fMantissa = 1.0f + (mantissa / (float)0x800000);
            }
            else
            {
                if (mantissa != 0)
                {
                    // denormal
                    exponent -= 126;
                    fMantissa = 1.0f / (float)0x800000;
                }
                else
                {
                    // +0 and -0 cases
                    fMantissa = 0;
                }
            }

            float fExponent = (float)Math.Pow(2.0, exponent);
            float ret = fSign * fMantissa * fExponent;
            return ret;
        }

        private double ToDouble_IEEE754(ulong fb)
        {
            //ulong fb = Convert.ToUInt64(f);

            int sign = (int)((fb >> 63) & 1);
            int exponent = (int)((fb >> 52) & 0x7FF);
            long mantissa = (long)(fb & 0xFFFFFFFFFFFFF);

            double fMantissa;
            double fSign = sign == 0 ? 1.0d : -1.0d;

            if (exponent != 0)
            {
                exponent -= 1023;
                fMantissa = 1.0d + (mantissa / (double)0x10000000000000);
            }
            else
            {
                if (mantissa != 0)
                {
                    // denormal
                    exponent -= 1022;
                    fMantissa = 1.0d / (double)0x10000000000000;
                }
                else
                {
                    // +0 and -0 cases
                    fMantissa = 0;
                }
            }

            double fExponent = (double)Math.Pow(2.0, exponent);
            double ret = fSign * fMantissa * fExponent;
            return ret;
        }

        #endregion
    }
}
