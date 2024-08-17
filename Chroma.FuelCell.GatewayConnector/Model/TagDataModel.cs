using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Chroma.FuelCell.GatewayConnector
{
    public class TagDataModel : INotifyPropertyChanged
    {
        ushort address;
        public ushort Address
        {
            get { return address; }
            set
            {
                address = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Address"));
            }
        }

        object value_;
        public object Value
        {
            get { return value_; }
            set
            {
                value_ = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        int tagCount;
        public int TagCount
        {
            get { return tagCount; }
            set
            {
                tagCount = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TagCount"));
            }
        }

        bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
