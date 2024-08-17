using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.UI.Wpf.Common;
using Chroma.Common;
using System.Collections.ObjectModel;
using Chroma.FuelCell.GatewayConnector.Model;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;

namespace Chroma.FuelCell.GatewayConnector
{
    public class ModbusTCPViewModel : ViewModelBase
    {
        ModbusTCPTool mbTCPTool;

        #region Ctor

        public ModbusTCPViewModel()
        {
            mbTCPTool = new ModbusTCPTool(IPAddr, TCPPort);
            LogData = new ObservableCollectionEx<string>();
            tagTypeList = new List<string>();
            CoilDataList = new ObservableCollectionEx<TagDataModel>();
            DiscreteInputDataList = new ObservableCollectionEx<TagDataModel>();
            HoldingRegisterDataList = new ObservableCollectionEx<TagDataModel>();
            InputRegisterDataList = new ObservableCollectionEx<TagDataModel>();
            dataTableMappingList = new Dictionary<ModbusTCPProtocol.TagType, ObservableCollectionEx<TagDataModel>>();
        }

        #endregion

        #region Properties

        string ipAddr;
        public string IPAddr
        {
            get
            {
                if (string.IsNullOrEmpty(ipAddr))
                    return "192.168.1.1";
                return ipAddr;
            }

            set
            {
                if (ipAddr != value)
                {
                    ipAddr = value;
                    //mbTCPTool.UpdateIPEndPoint(IPAddr, TCPPort);
                }

                RaisePropertyChanged("IPAddr");
            }
        }

        int tcpPort;
        public int TCPPort
        {
            get
            {
                if (tcpPort.Equals(0))
                    return 502;
                return tcpPort;
            }

            set
            {
                if (tcpPort != value)
                {
                    tcpPort = value;
                    //mbTCPTool.UpdateIPEndPoint(IPAddr, TCPPort);
                }

                RaisePropertyChanged("TCPPort");
            }
        }

        ObservableCollectionEx<TagDataModel> coilDataList;
        public ObservableCollectionEx<TagDataModel> CoilDataList
        {
            get { return coilDataList; }
            set
            {
                if (coilDataList != value)
                {
                    coilDataList = value;
                }

                RaisePropertyChanged("CoilDataList");
            }
        }

        ObservableCollectionEx<TagDataModel> discreteInputDataList;
        public ObservableCollectionEx<TagDataModel> DiscreteInputDataList
        {
            get { return discreteInputDataList; }
            set
            {
                if (discreteInputDataList != value)
                {
                    discreteInputDataList = value;
                }

                RaisePropertyChanged("DiscreteInputDataList");
            }
        }

        ObservableCollectionEx<TagDataModel> holdingRegisterDataList;
        public ObservableCollectionEx<TagDataModel> HoldingRegisterDataList
        {
            get { return holdingRegisterDataList; }
            set
            {
                if (holdingRegisterDataList != value)
                {
                    holdingRegisterDataList = value;
                }

                RaisePropertyChanged("HoldingRegisterDataList");
            }
        }

        ObservableCollectionEx<TagDataModel> inputRegisterDataList;
        public ObservableCollectionEx<TagDataModel> InputRegisterDataList
        {
            get { return inputRegisterDataList; }
            set
            {
                if (inputRegisterDataList != value)
                {
                    inputRegisterDataList = value;
                }

                RaisePropertyChanged("InputRegisterDataList");
            }
        }

        TagDataModel coilDataModel;
        public TagDataModel CoilDataModel
        {
            get { return coilDataModel; }
            set
            {
                if (coilDataModel != value)
                {
                    coilDataModel = value;
                }

                RaisePropertyChanged("CoilDataModel");
            }
        }

        TagDataModel discreteInputDataModel;
        public TagDataModel DiscreteInputDataModel
        {
            get { return discreteInputDataModel; }
            set
            {
                if (discreteInputDataModel != value)
                {
                    discreteInputDataModel = value;
                }

                RaisePropertyChanged("DiscreteInputDataModel");
            }
        }

        TagDataModel holdingRegisterDataModel;
        public TagDataModel HoldingRegisterDataModel
        {
            get { return holdingRegisterDataModel; }
            set
            {
                if (holdingRegisterDataModel != value)
                {
                    holdingRegisterDataModel = value;
                }

                RaisePropertyChanged("HoldingRegisterDataModel");
            }
        }

        TagDataModel inputRegisterDataModel;
        public TagDataModel InputRegisterDataModel
        {
            get { return inputRegisterDataModel; }
            set
            {
                if (inputRegisterDataModel != value)
                {
                    inputRegisterDataModel = value;
                }

                RaisePropertyChanged("InputRegisterDataModel");
            }
        }

        //comboBox: Register Types 當前選擇項目
        string tagTypeSelected;
        public string TagTypeSelected
        {
            get { return tagTypeSelected; }
            set
            {
                tagTypeSelected = value;
                RaisePropertyChanged("TagTypeSelected");
            }
        }

        //綁到comboBox: Register Types
        List<string> tagTypeList;
        public List<string> TagTypeList
        {
            get
            {
                //return EnumExtensions.GetDescriptionFromValue(tagTypeSelected);
                IEnumerable<WriteTagType> tagTypeEnum = Enum.GetValues(typeof(WriteTagType)).Cast<WriteTagType>();
                //List<string> tagTypeValues = new List<string>();
                foreach (WriteTagType enum_ in tagTypeEnum)
                {
                    tagTypeList.Insert(0, enum_ + " (" + EnumExtensions.GetDescriptionFromValue((WriteTagType)Enum.Parse(typeof(WriteTagType), enum_.ToString())) + ")");
                }
                tagTypeList.Reverse();

                return tagTypeList;
            }
        }

        private string selectedlogData;
        public string SelectedlogData
        {
            get { return selectedlogData; }
            set
            {
                selectedlogData = value;
                RaisePropertyChanged("SelectedlogData");
            }
        }

        ObservableCollectionEx<string> logData;
        public ObservableCollectionEx<string> LogData
        {
            get { return logData; }
            set
            {
                logData = value;
                RaisePropertyChanged("LogData");
            }
        }

        Visibility isShowSettingInfos = Visibility.Collapsed;
        public Visibility IsShowSettingInfos
        {
            get { return isShowSettingInfos; }
            set
            {
                isShowSettingInfos = value;
                RaisePropertyChanged("IsShowSettingInfos");
            }
        }

        bool isReadTagDataCommand = false;
        public bool IsReadTagDataCommand
        {
            get { return isReadTagDataCommand; }
            set
            {
                isReadTagDataCommand = value;
                RaisePropertyChanged("IsReadTagDataCommand");
            }
        }

        bool isWriteTagDataCommand = false;
        public bool IsWriteTagDataCommand
        {
            get { return isWriteTagDataCommand; }
            set
            {
                isWriteTagDataCommand = value;
                RaisePropertyChanged("IsWriteTagDataCommand");
            }
        }

        bool isConnectModbusGatewayCommand = true;
        public bool IsConnectModbusGatewayCommand
        {
            get { return isConnectModbusGatewayCommand; }
            set
            {
                isConnectModbusGatewayCommand = value;
                RaisePropertyChanged("IsConnectModbusGatewayCommand");
            }
        }

        Dictionary<ModbusTCPProtocol.TagType, ObservableCollectionEx<TagDataModel>> dataTableMappingList;

        #endregion

        #region ICommand

        public ICommand ConnectModbusGatewayCommand { get { return new CommandHandler(ConnectModbusGateway); } }
        public ICommand DisconnectModbusGatewayCommand { get { return new CommandHandler(DisconnectModbusGateway); } }
        public ICommand ImportCSVTagCommand { get { return new CommandHandler(ImportCSVTag); } }
        public ICommand ReadTagDataCommand { get { return new CommandHandler(ReadTagData); } }
        public ICommand WriteTagDataCommand { get { return new CommandHandler(WriteTagData, CanWriteTagData); } }

        void ImportCSVTag()
        {
            mbTCPTool.ParseCSVTag();
            if (mbTCPTool.tagDataList != null)
            {
                IsWriteTagDataCommand = true;
                IsReadTagDataCommand = true;
                AppendLog();
            }
        }

        void ConnectModbusGateway()
        {
            mbTCPTool.UpdateIPEndPoint(IPAddr, TCPPort);
            mbTCPTool.Connect();
            AppendLog();

            if (mbTCPTool.IsConnected)
            {
                IsConnectModbusGatewayCommand = false;
                IsShowSettingInfos = Visibility.Visible;
            }
            else
            {
                IsConnectModbusGatewayCommand = true;
                IsShowSettingInfos = Visibility.Collapsed;
            }
        }

        void DisconnectModbusGateway()
        {
            mbTCPTool.Disconnect();
            AppendLog();

            if (mbTCPTool.IsConnected)
            {
                IsConnectModbusGatewayCommand = false;
                IsShowSettingInfos = Visibility.Visible;
            }
            else
            {
                IsConnectModbusGatewayCommand = true;
                IsShowSettingInfos = Visibility.Collapsed;
            }
        }

        void ReadTagData()
        {
            mbTCPTool.ReadCoils();
            mbTCPTool.ReadDiscreteInputs();
            mbTCPTool.ReadHoldingRegister();
            mbTCPTool.ReadInputRegister();

            AppendLog();
            UpdateDataTable();
        }

        void WriteTagData()
        {
            WriteTagType writeTagTypeSelected = (WriteTagType)Enum.Parse(typeof(WriteTagType), tagTypeSelected.Split(' ').FirstOrDefault());
            string writeTagTypeDesc = EnumExtensions.GetDescriptionFromValue((WriteTagType)Enum.Parse(typeof(WriteTagType), writeTagTypeSelected.ToString()));

            List<TagDataModel> tagsSelected;
            if (writeTagTypeSelected == WriteTagType.WRITE_SINGLE_COIL || writeTagTypeSelected == WriteTagType.WRITE_MULTIPLE_COILS)
                tagsSelected = dataTableMappingList[ModbusTCPProtocol.TagType.COIL].Where(tdm => tdm.IsChecked == true).ToList();
            else
                tagsSelected = dataTableMappingList[ModbusTCPProtocol.TagType.HOLDING_REGISTER].Where(tdm => tdm.IsChecked == true).ToList();

            HashSet<int> tagsAddrSelected = new HashSet<int>(tagsSelected.Select(tdm => (int)tdm.Address));

            if (tagsSelected.Count == 0)
            {
                MessageBoxResult mbr1 = MessageBoxEx.ShowOK($"No tags are selected, please select tag and reclick write!",
                                                            LanguageResources.GetValue("MsgExWriteTagValueTitle"),
                                                            LanguageResources.GetValue("MsgExOK"),
                                                            MessageBoxImage.Error);

                if (mbr1 == MessageBoxResult.OK)
                    return;
            }

            //MessageBox.Show($"Write type: {writeTagTypeDesc} is selected, proceed writing {tagsSelected.Count} selected tags?", "Write Tag Data", MessageBoxButton.OK, MessageBoxImage.Warning);
            MessageBoxResult mbr2 = MessageBoxEx.ShowOKCancel($"Write type: {writeTagTypeDesc} is selected, proceed writing {tagsSelected.Count} selected tags?",
                                                               LanguageResources.GetValue("MsgExWriteTagValueTitle"),
                                                               LanguageResources.GetValue("MsgExOK"),
                                                               LanguageResources.GetValue("MsgExCancel"),
                                                               MessageBoxImage.Warning);

            if (mbr2 == MessageBoxResult.Cancel)
                return;

            // Set writing tags data list
            mbTCPTool.tagsWriteDataList = TagStaticDatas.tagDataList.Where(td => tagsAddrSelected.Contains(td.Address)).ToList();

            foreach (TagDataModel tdm in tagsSelected)
            {
                mbTCPTool.tagsWriteDataList[mbTCPTool.tagsWriteDataList.FindIndex(td => td.Address == tdm.Address)].Value = tdm.Value;
            }

            switch (writeTagTypeSelected)
            {
                case WriteTagType.WRITE_SINGLE_COIL:            // 單一線圈寫入
                    mbTCPTool.WriteSingleCoil();
                    break;
                case WriteTagType.WRITE_MULTIPLE_COILS:         // 多重線圈寫入
                    mbTCPTool.WriteMultipleCoils();
                    break;
                case WriteTagType.WRITE_SINGLE_REGISTER:        // 單一寄存器寫入
                    mbTCPTool.WriteSingleRegister();
                    break;
                case WriteTagType.WRITE_MULTIPLE_REGISTER:      // 單一寄存器寫入
                    mbTCPTool.WriteMultipleRegisters();
                    break;
                default:
                    break;
            }

            AppendLog();
        }

        bool CanWriteTagData()
        {
            if (string.IsNullOrEmpty(TagTypeSelected) || dataTableMappingList.Count == 0)
                return false;
            else
                return true;
        }

        void AppendLog()
        {
            LogData.AddRange(LogExtensions.GetLogger());
            LogExtensions.ClearLogger();
        }

        public void UpdateDataTable()
        {
            CreateDataTableMappingList();
            foreach (ModbusTCPProtocol.TagType tagtype in Enum.GetValues(typeof(ModbusTCPProtocol.TagType)).Cast<ModbusTCPProtocol.TagType>())
                FillDataTable(tagtype);
        }

        private void FillDataTable(ModbusTCPProtocol.TagType tagtype)
        {
            int i = 0;
            dataTableMappingList[tagtype].Clear();
            foreach (TagData td in TagStaticDatas.tagDataList.Where(td => td.TagType == (int)tagtype))
            {
                dataTableMappingList[tagtype].Add(new TagDataModel()
                {
                    Address = (ushort)td.Address,
                    Value = td.Value,
                    TagCount = i++
                });
            }
        }

        private void CreateDataTableMappingList()
        {
            dataTableMappingList.Clear();
            dataTableMappingList.Add(ModbusTCPProtocol.TagType.COIL, CoilDataList);
            dataTableMappingList.Add(ModbusTCPProtocol.TagType.DISCRETE_INPUT, DiscreteInputDataList);
            dataTableMappingList.Add(ModbusTCPProtocol.TagType.HOLDING_REGISTER, HoldingRegisterDataList);
            dataTableMappingList.Add(ModbusTCPProtocol.TagType.INPUT_REGISTER, InputRegisterDataList);
        }

        #endregion

        public override ViewModelBase Clone()
        {
            throw new NotImplementedException();
        }
    }
}
