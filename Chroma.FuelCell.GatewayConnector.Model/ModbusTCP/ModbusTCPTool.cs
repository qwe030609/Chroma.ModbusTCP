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
using Chroma.UI.Wpf.Common;
using System.Runtime.InteropServices;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    public class ModbusTCPTool
    {
        ModbusTCPHelper modbusTCPHelper;
        CSVHelper csvHelper;

        # region Ctor
        public ModbusTCPTool(string ipAddr, int tcpPort)
        {
            //IPAddress.TryParse(ipAddr, out IPAddr);
            IPAddr = ipAddr;
            TCPPort = tcpPort;
            modbusTCPHelper = ModbusTCPHelper.GetInstance();
            csvHelper = CSVHelper.GetInstance();
            TagStaticDatas.tagDataList = new List<TagData>();
            TagStaticDatas.currTagData = new TagData();
            tcpSynBuffer_ = modbusTCPHelper.tcpSynBuffer;

            //modbusTCPHelper.CreateDispatcherTimer();
        }
        #endregion

        #region properties

        private string IPAddr;

        private int TCPPort;

        private static bool isConnected = false;

        // ------------------------------------------------------------------------
        /// <summary>Shows if a connection is active.</summary>
        public bool IsConnected
        {
            get { return isConnected; }
        }

        //儲存所有 tag 的資料;
        public List<TagData> tagDataList;

        //儲存要寫入資料的 tags ;
        public List<TagData> tagsWriteDataList;

        protected internal ushort[] tcpSynBuffer_;

        #endregion

        ///// <summary>
        ///// Polling之計時器委派函式
        ///// </summary>
        ///// <param name="sender">事件傳遞物件</param>
        ///// <param name="args">事件參數</param>
        //public delegate void PollingHandler(bool isPolling_, double pollingRate_);

        ///// <summary>
        ///// 資料讀取後事件
        ///// </summary>
        //public event PollingHandler PollingReadTimerEvent;

        //public void OnPollingReadTimerEvent(bool isPolling_, double pollingRate_)
        //{
        //    OnPollingReadTimerEvent(isPolling_, pollingRate_);
        //}

        #region Methods
        public void Connect()
        {
            try
            {
                modbusTCPHelper.tcpSynSckt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //modbusTCPHelper.tcpSynSckt.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.KeepAlive, true);
               

                modbusTCPHelper.tcpSynSckt.SendTimeout = 2000;
                modbusTCPHelper.tcpSynSckt.ReceiveTimeout = 2000;
                modbusTCPHelper.tcpSynSckt.Connect(new IPEndPoint(IPAddress.Parse(IPAddr), TCPPort));

                // 啓用keep-alive
                modbusTCPHelper.tcpSynSckt.IOControl(IOControlCode.KeepAliveValues, GetKeepAliveData(), null);
                modbusTCPHelper.tcpSynSckt.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);


                //AppendLog(String.Format("Connected using TCP to {0}", modbusTCPHelper.tcpSynSckt.RemoteEndPoint));
                //MessageBox.Show(String.Format("Connected using TCP to {0}", modbusTCPHelper.tcpSynSckt.RemoteEndPoint), "Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogExtensions.CreateLog(String.Format("Connected using TCP to {0}", modbusTCPHelper.tcpSynSckt.RemoteEndPoint));
                isConnected = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogExtensions.CreateLog(ex.Message);
                isConnected = false;
                return;
            }
        }

        public void Disconnect()
        {
            if (modbusTCPHelper.tcpSynSckt != null)
            {
                modbusTCPHelper.tcpSynSckt.Close();
                modbusTCPHelper.tcpSynSckt.Dispose();
                modbusTCPHelper.tcpSynSckt = null;
                isConnected = false;
            }
            LogExtensions.CreateLog("Disconnected");
        }

        public void UpdateIPEndPoint(string ipAddr, int tcpPort)
        {
            IPAddr = ipAddr;
            TCPPort = tcpPort;
        }

        public void ReadDiscreteInputs()
        {
            //tagDataList = new List<TagData>(TagStaticDatas.tagDataList);
            //ModbusTCPHelper.transactionId = 0;
            ModbusTCPHelper.dataLength = 1;

            foreach (TagData td in tagDataList.Where(td => td.TagType == (int)ModbusTCPProtocol.TagType.DISCRETE_INPUT))
            {
                //ModbusTCPHelper.startAddress = td.Address;
                TagStaticDatas.currTagData = td;
                ModbusTCPHelper.startAddress = td.Address - 1;
                modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadDiscreteInputs);
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.GetBufferData()[td.Address - 1];
            }
        }

        public void ReadCoils()
        {
            //tagDataList = new List<TagData>(TagStaticDatas.tagDataList);
            //ModbusTCPHelper.transactionId = 0;
            ModbusTCPHelper.dataLength = 1;

            foreach (TagData td in tagDataList.Where(td => td.TagType == (int)ModbusTCPProtocol.TagType.COIL))
            {
                TagStaticDatas.currTagData = td;
                ModbusTCPHelper.startAddress = td.Address - 1;
                modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadCoils);
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.GetBufferData()[td.Address - 1];
            }
        }

        public void ReadHoldingRegister()
        {
            //tagDataList = new List<TagData>(TagStaticDatas.tagDataList);
            //ModbusTCPHelper.transactionId = 0;
            
            //ModbusTCPHelper.dataLength = 2;

            foreach (TagData td in tagDataList.Where(td => td.TagType == (int)ModbusTCPProtocol.TagType.HOLDING_REGISTER))
            {
                TagStaticDatas.currTagData = td;
                SetDataLength(td.TagDataType);
                ModbusTCPHelper.startAddress = td.Address - 1;
                modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadHoldingRegisters);
                //modbusTCPHelper.SwapData(td.TagDataType);
                //TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.GetBufferData()[td.Address];
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.SwapData(td);
            }
        }

        public void ReadInputRegister()
        {
            //tagDataList = new List<TagData>(TagStaticDatas.tagDataList);
            //ModbusTCPHelper.transactionId = 0;
            //ModbusTCPHelper.dataLength = 1;

            foreach (TagData td in tagDataList.Where(td => td.TagType == (int)ModbusTCPProtocol.TagType.INPUT_REGISTER))
            {
                TagStaticDatas.currTagData = td;
                SetDataLength(td.TagDataType);
                ModbusTCPHelper.startAddress = td.Address - 1;
                modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadInputRegister);
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.SwapData(td);
            }
        }

        public void ReadAllTagTypes()
        {
            //tagDataList = new List<TagData>(TagStaticDatas.tagDataList);
            //ModbusTCPHelper.transactionId = 0;
            //ModbusTCPHelper.dataLength = tagDataList.Where(td => td.TagType == 1).Count();

            //foreach (TagData td in tagDataList.Where(td => td.TagType == (int)ModbusTCPProtocol.FunctionCode.ReadDiscreteInputs))
            //{
            //    ModbusTCPHelper.startAddress = td.Address;
            //    modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadDiscreteInputs);
            //}
            

            modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadDiscreteInputs);
            modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadCoils);
            modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadHoldingRegisters);
            modbusTCPHelper.ExecuteReadCommand((int)ModbusTCPProtocol.FunctionCode.ReadInputRegister);
        }

        public void WriteSingleCoil()
        {
            foreach (TagData td in tagsWriteDataList)
            {
                TagStaticDatas.currTagData = td;
                SetDataLength(td.TagDataType);
                ModbusTCPHelper.startAddress = td.Address - 1;
                ModbusTCPHelper.value = td.Value;

                modbusTCPHelper.ExecuteWriteSingleCoilCommand();
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.SwapData(td);
            }

            //modbusTCPHelper.ExecuteWriteSingleCoilCommand();
        }

        public void WriteMultipleCoils()
        {
            foreach (TagData td in tagsWriteDataList)
            {
                TagStaticDatas.currTagData = td;
                SetDataLength(td.TagDataType);
                ModbusTCPHelper.startAddress = td.Address - 1;
                ModbusTCPHelper.value = td.Value;

                modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteMultipleCoils);
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.SwapData(td);
            }

            //modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteMultipleCoils);
        }

        public void WriteSingleRegister()
        {
            foreach (TagData td in tagsWriteDataList)
            {
                TagStaticDatas.currTagData = td;
                SetDataLength(td.TagDataType);
                ModbusTCPHelper.startAddress = td.Address - 1;
                //ModbusTCPHelper.value = td.Value;
                UpdateTcpSynBuffer(td);

                modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteSingleRegister);
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.SwapData(td);
            }

            //modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteSingleRegister);
        }

        public void WriteMultipleRegisters()
        {
            foreach (TagData td in tagsWriteDataList)
            {
                TagStaticDatas.currTagData = td;
                SetDataLength(td.TagDataType);
                ModbusTCPHelper.startAddress = td.Address - 1;
                //ModbusTCPHelper.value = td.Value;
                UpdateTcpSynBuffer(td);

                modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteMultipleRegisters);
                TagStaticDatas.tagDataList[TagStaticDatas.tagDataList.IndexOf(td)].Value = modbusTCPHelper.SwapData(td);
            }

            //modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteMultipleRegisters);
        }

        //public void WriteSingleCoilAndRegister()
        //{
        //    modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteSingleCoil);
        //    modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteSingleRegister);
        //}

        //public void WriteMultipleCoilAndRegisters()
        //{
        //    modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteMultipleCoils);
        //    modbusTCPHelper.ExecuteWriteCommand((int)ModbusTCPProtocol.FunctionCode.WriteMultipleRegisters);
        //}

        public void ParseCSVTag()
        {
            TagStaticDatas.tagDataList = new List<TagData>();
            csvHelper.ImportCSV();
            tagDataList = new List<TagData>(TagStaticDatas.tagDataList);
            if (tagDataList.Count != 0)
                LogExtensions.CreateLog("Import CSV tags complete!");
            else
                LogExtensions.CreateLog("No CSV tags imported!");
        }

        private byte[] GetKeepAliveData()
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));    // keep-alive 間隔
            BitConverter.GetBytes((uint)500).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2); // 嘗試間隔
            return inOptionValues;
        }

        private void SetDataLength(int tagDataType)
        {
            if (tagDataType == (int)ModbusTCPProtocol.TagDataType.INT16 || tagDataType == (int)ModbusTCPProtocol.TagDataType.UINT16 || tagDataType == (int)ModbusTCPProtocol.TagDataType.BCD16)
                ModbusTCPHelper.dataLength = 1;
            else if (tagDataType == (int)ModbusTCPProtocol.TagDataType.INT32 || tagDataType == (int)ModbusTCPProtocol.TagDataType.UINT32 || tagDataType == (int)ModbusTCPProtocol.TagDataType.BCD32 || tagDataType == (int)ModbusTCPProtocol.TagDataType.FLOAT)
                ModbusTCPHelper.dataLength = 2;
            else if (tagDataType == (int)ModbusTCPProtocol.TagDataType.INT64 || tagDataType == (int)ModbusTCPProtocol.TagDataType.UINT64 || tagDataType == (int)ModbusTCPProtocol.TagDataType.BCD64 || tagDataType == (int)ModbusTCPProtocol.TagDataType.DOUBLE)
                ModbusTCPHelper.dataLength = 4;
        }

        private void UpdateTcpSynBuffer(TagData td)
        {
            int actAddr = td.Address - 1;

            // Parse int, uint data to register values
            if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.INT16 || td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT16)
            {
                ushort res;
                if (ushort.TryParse(td.Value.ToString(), out res))
                {
                    UpdateTcpSynBuffer(res, actAddr);
                    //tcpSynBuffer_[actAddr] = res;
                }
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.INT32 || td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT32)
            {
                uint res;
                if (uint.TryParse(td.Value.ToString(), out res))
                {
                    UpdateTcpSynBuffer(res, actAddr);

                    //uint intRes = BitConverter.ToUInt32(BitConverter.GetBytes(res), 0);

                    //ushort firstPart = (ushort)(res >> 16);
                    //ushort secondPart = (ushort)(res & 0xFFFF);

                    //tcpSynBuffer_[actAddr] = firstPart;
                    //tcpSynBuffer_[actAddr + 1] = secondPart;
                }
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.INT64 || td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT64)
            {
                ulong res;
                if (ulong.TryParse(td.Value.ToString(), out res))
                {
                    UpdateTcpSynBuffer(res, actAddr);

                    //uint intRes = BitConverter.ToUInt32(BitConverter.GetBytes(res), 0);

                    //ushort firstPart = (ushort)(res >> 48);
                    //ushort secondPart = (ushort)(res >> 32);
                    //ushort thirdPart = (ushort)(res >> 16);
                    //ushort forthPart = (ushort)(res & 0xFFFF);

                    //tcpSynBuffer_[actAddr] = firstPart;
                    //tcpSynBuffer_[actAddr + 1] = secondPart;
                    //tcpSynBuffer_[actAddr + 2] = thirdPart;
                    //tcpSynBuffer_[actAddr + 3] = forthPart;
                }
            }

            //else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.UINT16)
            //{
            //    short res;
            //    if (short.TryParse(td.Value.ToString(), out res))
            //    {
            //        tcpSynBuffer_[actAddr] = (ushort)res;
            //    }
            //}

            // Parse BCD data to register values
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.BCD16)
            {
                short res;
                ushort intRes = 0;
                if (short.TryParse(td.Value.ToString(), out res))
                    intRes = (ushort) HexConvertor.HexToINT16(res);

                UpdateTcpSynBuffer(intRes, actAddr);
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.BCD32)
            {
                int res;
                uint intRes = 0;
                if (int.TryParse(td.Value.ToString(), out res))
                    intRes = (uint) HexConvertor.HexToINT32(res);

                UpdateTcpSynBuffer(intRes, actAddr);

                //ushort firstPart = (ushort)(intRes >> 16);
                //ushort secondPart = (ushort)(intRes & 0xFFFF);

                //tcpSynBuffer_[actAddr] = firstPart;
                //tcpSynBuffer_[actAddr + 1] = secondPart;
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.BCD64)
            {
                long res;
                ulong intRes = 0;
                if (long.TryParse(td.Value.ToString(), out res))
                    intRes = (ulong) HexConvertor.HexToINT64(res);

                UpdateTcpSynBuffer(intRes, actAddr);

                //ushort firstPart = (ushort)(intRes >> 48);
                //ushort secondPart = (ushort)(intRes >> 32);
                //ushort thirdPart = (ushort)(intRes >> 16);
                //ushort forthPart = (ushort)(intRes & 0xFFFF);

                //tcpSynBuffer_[actAddr] = firstPart;
                //tcpSynBuffer_[actAddr + 1] = secondPart;
                //tcpSynBuffer_[actAddr + 2] = thirdPart;
                //tcpSynBuffer_[actAddr + 3] = forthPart;
            }

            // Parse float, double data to register values
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.FLOAT)
            {
                float res;
                if (float.TryParse(td.Value.ToString(), out res))
                {
                    uint intRes = BitConverter.ToUInt32(BitConverter.GetBytes(res), 0);
                    UpdateTcpSynBuffer(intRes, actAddr);

                    //ushort firstPart = (ushort)(intRes >> 16);
                    //ushort secondPart = (ushort)(intRes & 0xFFFF);

                    //tcpSynBuffer_[actAddr] = firstPart;
                    //tcpSynBuffer_[actAddr + 1] = secondPart;
                }
            }
            else if (td.TagDataType == (int)ModbusTCPProtocol.TagDataType.DOUBLE)
            {
                double res;
                if (double.TryParse(td.Value.ToString(), out res))
                {
                    ulong intRes = BitConverter.ToUInt64(BitConverter.GetBytes(res), 0);
                    UpdateTcpSynBuffer(intRes, actAddr);

                    //ushort firstPart = (ushort)(intRes >> 48);
                    //ushort secondPart = (ushort)(intRes >> 32);
                    //ushort thirdPart = (ushort)(intRes >> 16);
                    //ushort forthPart = (ushort)(intRes & 0xFFFF);

                    //tcpSynBuffer_[actAddr] = firstPart;
                    //tcpSynBuffer_[actAddr + 1] = secondPart;
                    //tcpSynBuffer_[actAddr + 2] = thirdPart;
                    //tcpSynBuffer_[actAddr + 3] = forthPart;
                }
            }
        }

        private void UpdateTcpSynBuffer<T>(T data, int Addr)
        {
            Type dataType = typeof(T);
            int dataSize = Marshal.SizeOf(dataType);
            if (dataSize == (int) ModbusTCPProtocol.BitType.BIT_16)
            {
                ushort res = ushort.Parse(data.ToString());
                tcpSynBuffer_[Addr] = res;
            }
            else if (dataSize == (int)ModbusTCPProtocol.BitType.BIT_32)
            {
                uint res = uint.Parse(data.ToString());

                ushort firstPart = (ushort)(res >> 16);
                ushort secondPart = (ushort)(res & 0xFFFF);

                tcpSynBuffer_[Addr] = firstPart;
                tcpSynBuffer_[Addr + 1] = secondPart;
            }
            else if (dataSize == (int)ModbusTCPProtocol.BitType.BIT_64)
            {
                ulong res = ulong.Parse(data.ToString());

                ushort firstPart = (ushort)(res >> 48);
                ushort secondPart = (ushort)(res >> 32);
                ushort thirdPart = (ushort)(res >> 16);
                ushort forthPart = (ushort)(res & 0xFFFF);

                tcpSynBuffer_[Addr] = firstPart;
                tcpSynBuffer_[Addr + 1] = secondPart;
                tcpSynBuffer_[Addr + 2] = thirdPart;
                tcpSynBuffer_[Addr + 3] = forthPart;
            }
        }

        #endregion
    }
}
