using SqliteORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearClaw.Common
{
    //[Table]
    public class Params : INotifyPropertyChanged
    {
        //[PrimaryKey]
        public long Id { get; set; }

        private string _fieldGroup;
        //[Field]
        public string FieldGroup {
            get { return _fieldGroup; }
            set
            {
                _fieldGroup = value;
                OnPropertyChanged("FieldGroup");
            }
        }

        private string _name;
        //[Field]
        public string Name {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private string _value;
        //[Field]
        public string Value {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        virtual internal protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            //DbMysql.UpdateParamValue(this);
        }
    }
}
