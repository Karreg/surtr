using Microsoft.Practices.Prism.Mvvm;

namespace surtr.AndroidCtrlTestModule.Services
{
    public class Device : BindableBase
    {
        private string id;
        private string ip;
        private string mode;
        private string model;
        private string product;
        private string serial;
        private string state;

        public string Id
        {
            get { return this.id; }
            set
            {
                if (this.id == value) return;
                this.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public string Ip
        {
            get { return this.ip; }
            set
            {
                if (this.ip == value) return;
                this.ip = value;
                this.OnPropertyChanged("Ip");
            }
        }

        public string Mode
        {
            get { return this.mode; }
            set
            {
                if (this.mode == value) return;
                this.mode = value;
                this.OnPropertyChanged("Mode");
            }
        }

        public string Model
        {
            get { return this.model; }
            set
            {
                if (this.model == value) return;
                this.model = value;
                this.OnPropertyChanged("Model");
            }
        }

        public string Product
        {
            get { return this.product; }
            set
            {
                if (this.product == value) return;
                this.product = value;
                this.OnPropertyChanged("Product");
            }
        }

        public string Serial
        {
            get { return this.serial; }
            set
            {
                if (this.serial == value) return;
                this.serial = value;
                this.OnPropertyChanged("Serial");
            }
        }

        public string State
        {
            get { return this.state; }
            set
            {
                if (this.state == value) return;
                this.state = value;
                this.OnPropertyChanged("State");
            }
        }
    }
}