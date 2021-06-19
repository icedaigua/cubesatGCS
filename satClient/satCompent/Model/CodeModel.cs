using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace TSFCS.DMDS.Client.Model
{
    public class CodeModel : ObservableObject
    {
        #region Field
        private string hex0;
        private string hex1;
        private string hex2;
        private string hex3;
        private string hex4;
        private string hex5;
        private string hex6;
        private string hex7;
        private string hex8;
        private string hex9;
        private string hex10;
        private string hex11;
        private string hex12;
        private string hex13;
        #endregion

        #region Property
        public string Hex0
        {
            get { return hex0; }
            set 
            { 
                hex0 = value;
                RaisePropertyChanged("Hex0");
            }
        }
        public string Hex1
        {
            get { return hex1; }
            set
            {
                hex1 = value;
                RaisePropertyChanged("Hex1");
            }
        }
        public string Hex2
        {
            get { return hex2; }
            set
            {
                hex2 = value;
                RaisePropertyChanged("Hex2");
            }
        }
        public string Hex3
        {
            get { return hex3; }
            set
            {
                hex3 = value;
                RaisePropertyChanged("Hex3");
            }
        }
        public string Hex4
        {
            get { return hex4; }
            set
            {
                hex4 = value;
                RaisePropertyChanged("Hex4");
            }
        }
        public string Hex5
        {
            get { return hex5; }
            set
            {
                hex5 = value;
                RaisePropertyChanged("Hex5");
            }
        }
        public string Hex6
        {
            get { return hex6; }
            set
            {
                hex6 = value;
                RaisePropertyChanged("Hex6");
            }
        }
        public string Hex7
        {
            get { return hex7; }
            set
            {
                hex7 = value;
                RaisePropertyChanged("Hex7");
            }
        }
        public string Hex8
        {
            get { return hex8; }
            set
            {
                hex8 = value;
                RaisePropertyChanged("Hex8");
            }
        }
        public string Hex9
        {
            get { return hex9; }
            set
            {
                hex9 = value;
                RaisePropertyChanged("Hex9");
            }
        }
        public string Hex10
        {
            get { return hex10; }
            set
            {
                hex10 = value;
                RaisePropertyChanged("Hex10");
            }
        }
        public string Hex11
        {
            get { return hex11; }
            set
            {
                hex11 = value;
                RaisePropertyChanged("Hex11");
            }
        }
        public string Hex12
        {
            get { return hex12; }
            set
            {
                hex12 = value;
                RaisePropertyChanged("Hex12");
            }
        }
        public string Hex13
        {
            get { return hex13; }
            set
            {
                hex13 = value;
                RaisePropertyChanged("Hex13");
            }
        }
        #endregion

        #region Method
        public static ObservableCollection<CodeModel> GetCode()
        {
            ObservableCollection<CodeModel> list = new ObservableCollection<CodeModel>();
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });
            list.Add(new CodeModel() { Hex0 = "00", Hex1 = "00", Hex2 = "00", Hex3 = "00", Hex4 = "00", Hex5 = "00", Hex6 = "00", Hex7 = "00", Hex8 = "00", Hex9 = "00", Hex10 = "00", Hex11 = "00", Hex12 = "00", Hex13 = "00" });

            return list;
        }
        #endregion

    }
}
