using System.ComponentModel;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    public class TagData : INotifyPropertyChanged
    {
        int tagType;
        public int TagType
        {
            get { return tagType; }
            set
            {
                tagType = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TagType"));
            }
        }

        int address;
        public int Address
        {
            get { return address; }
            set
            {
                address = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Address"));
            }
        }

        int tagDataType;
        public int TagDataType
        {
            get { return tagDataType; }
            set
            {
                tagDataType = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TagDataType"));
            }
        }

        bool isLittleEndian;
        public bool IsLittleEndian
        {
            get { return isLittleEndian; }
            set
            {
                isLittleEndian = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsLittleEndian"));
            }
        }

        bool isReverse;
        public bool IsReverse
        {
            get { return isReverse; }
            set
            {
                isReverse = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsReverse"));
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
