using System;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class CodeViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<CodeModel> code;
        #endregion

        #region Property
        public ObservableCollection<CodeModel> Code
        {
            get { return code; }
            set
            { 
                code = value;
                RaisePropertyChanged("Code");
            }
        }
        #endregion

        #region Construcor
        public CodeViewModel()
        {
            code = CodeModel.GetCode();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<byte[]>(this, "TMCODE", HandleCode);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        public void HandleCode(byte[] data)
        {
            for (int i = 0; i < 8; i++)
            {
                code[i].Hex0 = data[i * 14].ToString("X2");
                code[i].Hex1 = data[i * 14 + 1].ToString("X2");
                code[i].Hex2 = data[i * 14 + 2].ToString("X2");
                code[i].Hex3 = data[i * 14 + 3].ToString("X2");
                code[i].Hex4 = data[i * 14 + 4].ToString("X2");
                code[i].Hex5 = data[i * 14 + 5].ToString("X2");
                code[i].Hex6 = data[i * 14 + 6].ToString("X2");
                code[i].Hex7 = data[i * 14 + 7].ToString("X2");
                code[i].Hex8 = data[i * 14 + 8].ToString("X2");
                code[i].Hex9 = data[i * 14 + 9].ToString("X2");
                code[i].Hex10 = data[i * 14 + 10].ToString("X2");
                code[i].Hex11 = data[i * 14 + 11].ToString("X2");
                code[i].Hex12 = data[i * 14 + 12].ToString("X2");
                code[i].Hex13 = data[i * 14 + 13].ToString("X2");
            }
        }
        #endregion
    }
}
