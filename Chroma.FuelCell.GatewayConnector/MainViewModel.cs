using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.UI.Wpf.Common;
using Chroma.Common;
using System.Collections.ObjectModel;
using Chroma.FuelCell.GatewayConnector.Model;

namespace Chroma.FuelCell.GatewayConnector
{
    public class MainViewModel : ViewModelBase
    {
        ModbusTCPTool mbTCPTool;
        public MainViewModel()
        {
            MBTCPViewModel = new ModbusTCPViewModel();
        }

        ModbusTCPViewModel mbTCPViewModel;
        public ModbusTCPViewModel MBTCPViewModel
        {
            get { return mbTCPViewModel; }
            set
            {
                mbTCPViewModel = value;
                RaisePropertyChanged("MBTCPViewModel");
            }
        }

        public override ViewModelBase Clone()
        {
            throw new NotImplementedException();
        }
    }
}
