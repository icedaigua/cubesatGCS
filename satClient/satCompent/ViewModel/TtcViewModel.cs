using System;
using System.Collections.Generic;
using System.ComponentModel;

using GalaSoft.MvvmLight;

using TSFCS.DMDS.Client.Helper;

namespace TSFCS.DMDS.Client.ViewModel
{
    public class TtcViewModel : ViewModelBase
    {
        #region 字段
        private string pcm;
        private string bcr;
        private float panelVolt;
        private float panelCurrXP;
        private float panelCurrXN;
        private float panelCurrYP;
        private float panelCurrYN;
        private float panelTempXP;
        private float panelTempXN;
        private float panelTempYP;
        private float panelTempYN;
        private float batteryVolt;
        private float batteryCurr;
        private float batteryTemp;
        private float lineVolt14;
        private float lineVolt12P;
        private float lineVolt12N;
        private float lineVolt5;
        private float lineCurr14;
        private float lineCurr12P;
        private float lineCurr12N;
        private float lineCurr5;
        private float subCurrPcm;
        private float subCurrTx;
        private float subCurrObc;
        private float subVoltBd2;

        private float tempRf;
        private float rxAgc;
        private float rxAfc;
        private float txPower;

        private float tempObc;

        private string status0;
        private string status00;
        private string status01;
        private string status02;
        private string status03;
        private string status04;
        private string status05;
        private string status06;
        private string status07;
        private string status1;
        private string status10;
        private string status11;
        private string status12;
        private string status13;
        private string status14;
        private string status2;
        private string status20;
        private string status21;
        private string status22;
        private string status23;
        private string status24;
        private string status25;
        private string status3;
        private string status30;
        private string status31;
        private string status32;
        private string status33;
        private string status34;

        private string digit;
        private string digit0;
        private string digit1;
        private string digit2;
        private string digit3;
        private string digit4;
        #endregion

        #region 属性
        public string Pcm
        {
            get { return pcm; }
            set
            {
                pcm = value;
                RaisePropertyChanged("");
            }
        }
        public string Bcr
        {
            get { return bcr; }
            set
            {
                bcr = value;
                RaisePropertyChanged("");
            }
        }
        public float PanelVolt
        {
            get { return panelVolt; }
            set
            {
                panelVolt = value;
                RaisePropertyChanged("PanelVolt");
            }
        }
        public float PanelCurrXP
        {
            get { return panelCurrXP; }
            set
            {
                panelCurrXP = value;
                RaisePropertyChanged("PanelCurrXP");
            }
        }
        public float PanelCurrXN
        {
            get { return panelCurrXN; }
            set
            {
                panelCurrXN = value;
                RaisePropertyChanged("PanelCurrXN");
            }
        }
        public float PanelCurrYP
        {
            get { return panelCurrYP; }
            set
            {
                panelCurrYP = value;
                RaisePropertyChanged("PanelCurrYP");
            }
        }
        public float PanelCurrYN
        {
            get { return panelCurrYN; }
            set
            {
                panelCurrYN = value;
                RaisePropertyChanged("PanelCurrYN");
            }
        }
        public float PanelTempXP
        {
            get { return panelTempXP; }
            set
            {
                panelTempXP = value;
                RaisePropertyChanged("PanelTempXP");
            }
        }
        public float PanelTempXN
        {
            get { return panelTempXN; }
            set
            {
                panelTempXN = value;
                RaisePropertyChanged("PanelTempXN");
            }
        }
        public float PanelTempYP
        {
            get { return panelTempYP; }
            set
            {
                panelTempYP = value;
                RaisePropertyChanged("PanelTempYP");
            }
        }
        public float PanelTempYN
        {
            get { return panelTempYN; }
            set
            {
                panelTempYN = value;
                RaisePropertyChanged("PanelTempYN");
            }
        }
        public float BatteryVolt
        {
            get { return batteryVolt; }
            set
            {
                batteryVolt = value;
                RaisePropertyChanged("BatteryVolt");
            }
        }
        public float BatteryCurr
        {
            get { return batteryCurr; }
            set
            {
                batteryCurr = value;
                RaisePropertyChanged("BatteryCurr");
            }
        }
        public float BatteryTemp
        {
            get { return batteryTemp; }
            set
            {
                batteryTemp = value;
                RaisePropertyChanged("BatteryTemp");
            }
        }
        public float LineVolt14
        {
            get { return lineVolt14; }
            set
            {
                lineVolt14 = value;
                RaisePropertyChanged("LineVolt14");
            }
        }
        public float LineVolt12P
        {
            get { return lineVolt12P; }
            set
            {
                lineVolt12P = value;
                RaisePropertyChanged("LineVolt12P");
            }
        }
        public float LineVolt12N
        {
            get { return lineVolt12N; }
            set
            {
                lineVolt12N = value;
                RaisePropertyChanged("LineVolt12N");
            }
        }
        public float LineVolt5
        {
            get { return lineVolt5; }
            set
            {
                lineVolt5 = value;
                RaisePropertyChanged("LineVolt5");
            }
        }
        public float LineCurr14
        {
            get { return lineCurr14; }
            set
            {
                lineCurr14 = value;
                RaisePropertyChanged("LineCurr14");
            }
        }
        public float LineCurr12P
        {
            get { return lineCurr12P; }
            set
            {
                lineCurr12P = value;
                RaisePropertyChanged("LineCurr12P");
            }
        }
        public float LineCurr12N
        {
            get { return lineCurr12N; }
            set
            {
                lineCurr12N = value;
                RaisePropertyChanged("LineCurr12N");
            }
        }
        public float LineCurr5
        {
            get { return lineCurr5; }
            set
            {
                lineCurr5 = value;
                RaisePropertyChanged("LineCurr5");
            }
        }
        public float SubCurrPcm
        {
            get { return subCurrPcm; }
            set
            {
                subCurrPcm = value;
                RaisePropertyChanged("SubCurrPcm");
            }
        }
        public float SubCurrTx
        {
            get { return subCurrTx; }
            set
            {
                subCurrTx = value;
                RaisePropertyChanged("SubCurrTx");
            }
        }
        public float SubCurrObc
        {
            get { return subCurrObc; }
            set
            {
                subCurrObc = value;
                RaisePropertyChanged("SubCurrObc");
            }
        }
        public float SubVoltBd2
        {
            get { return subVoltBd2; }
            set
            {
                subVoltBd2 = value;
                RaisePropertyChanged("SubVoltBd2");
            }
        }
        public float TempRf
        {
            get { return tempRf; }
            set
            {
                tempRf = value;
                RaisePropertyChanged("TempRf");
            }
        }
        public float RxAgc
        {
            get { return rxAgc; }
            set 
            { 
                rxAgc = value;
                RaisePropertyChanged("RxAgc");
            }
        }
        public float RxAfc
        {
            get { return rxAfc; }
            set
            {
                rxAfc = value;
                RaisePropertyChanged("RxAfc");
            }
        }
        public float TxPower
        {
            get { return txPower; }
            set
            {
                txPower = value;
                RaisePropertyChanged("TxPower");
            }
        }
        public float TempObc
        {
            get { return tempObc; }
            set
            {
                tempObc = value;
                RaisePropertyChanged("TempObc");
            }
        }
        public string Status0
        {
            get { return status0; }
            set
            {
                status0 = value;
                RaisePropertyChanged("Status0");
            }
        }
        public string Status00
        {
            get { return status00; }
            set
            {
                status00 = value;
                RaisePropertyChanged("Status00");
            }
        }
        public string Status01
        {
            get { return status01; }
            set
            {
                status01 = value;
                RaisePropertyChanged("Status01");
            }
        }
        public string Status02
        {
            get { return status02; }
            set
            {
                status02 = value;
                RaisePropertyChanged("Status02");
            }
        }
        public string Status03
        {
            get { return status03; }
            set
            {
                status03 = value;
                RaisePropertyChanged("Status03");
            }
        }
        public string Status04
        {
            get { return status04; }
            set
            {
                status04 = value;
                RaisePropertyChanged("Status04");
            }
        }
        public string Status05
        {
            get { return status05; }
            set
            {
                status05 = value;
                RaisePropertyChanged("Status05");
            }
        }
        public string Status06
        {
            get { return status06; }
            set
            {
                status06 = value;
                RaisePropertyChanged("Status06");
            }
        }
        public string Status07
        {
            get { return status07; }
            set
            {
                status07 = value;
                RaisePropertyChanged("Status07");
            }
        }
        public string Status1
        {
            get { return status1; }
            set
            {
                status1 = value;
                RaisePropertyChanged("Status1");
            }
        }
        public string Status10
        {
            get { return status10; }
            set
            {
                status10 = value;
                RaisePropertyChanged("Status10");
            }
        }
        public string Status11
        {
            get { return status11; }
            set
            {
                status11 = value;
                RaisePropertyChanged("Status11");
            }
        }
        public string Status12
        {
            get { return status12; }
            set
            {
                status12 = value;
                RaisePropertyChanged("Status12");
            }
        }
        public string Status13
        {
            get { return status13; }
            set
            {
                status13 = value;
                RaisePropertyChanged("Status13");
            }
        }
        public string Status14
        {
            get { return status14; }
            set
            {
                status14 = value;
                RaisePropertyChanged("Status14");
            }
        }
        public string Status2
        {
            get { return status2; }
            set
            {
                status2 = value;
                RaisePropertyChanged("Status2");
            }
        }
        public string Status20
        {
            get { return status20; }
            set
            {
                status20 = value;
                RaisePropertyChanged("Status20");
            }
        }
        public string Status21
        {
            get { return status21; }
            set
            {
                status21 = value;
                RaisePropertyChanged("Status21");
            }
        }
        public string Status22
        {
            get { return status22; }
            set
            {
                status22 = value;
                RaisePropertyChanged("Status22");
            }
        }
        public string Status23
        {
            get { return status23; }
            set
            {
                status23 = value;
                RaisePropertyChanged("Status23");
            }
        }
        public string Status24
        {
            get { return status24; }
            set
            {
                status24 = value;
                RaisePropertyChanged("Status24");
            }
        }
        public string Status25
        {
            get { return status25; }
            set
            {
                status25 = value;
                RaisePropertyChanged("Status25");
            }
        }
        public string Status3
        {
            get { return status3; }
            set
            {
                status3 = value;
                RaisePropertyChanged("Status3");
            }
        }
        public string Status30
        {
            get { return status30; }
            set
            {
                status30 = value;
                RaisePropertyChanged("Status30");
            }
        }
        public string Status31
        {
            get { return status31; }
            set
            {
                status31 = value;
                RaisePropertyChanged("Status31");
            }
        }
        public string Status32
        {
            get { return status32; }
            set
            {
                status32 = value;
                RaisePropertyChanged("Status32");
            }
        }
        public string Status33
        {
            get { return status33; }
            set
            {
                status33 = value;
                RaisePropertyChanged("Status33");
            }
        }
        public string Status34
        {
            get { return status34; }
            set
            {
                status34 = value;
                RaisePropertyChanged("Status34");
            }
        }
        public string Digit
        {
            get { return digit; }
            set
            {
                digit = value;
                RaisePropertyChanged("Digit");
            }
        }
        public string Digit0
        {
            get { return digit0; }
            set
            {
                digit0 = value;
                RaisePropertyChanged("Digit0");
            }
        }
        public string Digit1
        {
            get { return digit1; }
            set
            {
                digit1 = value;
                RaisePropertyChanged("Digit1");
            }
        }
        public string Digit2
        {
            get { return digit2; }
            set
            {
                digit2 = value;
                RaisePropertyChanged("Digit2");
            }
        }
        public string Digit3
        {
            get { return digit3; }
            set
            {
                digit3 = value;
                RaisePropertyChanged("Digit3");
            }
        }
        public string Digit4
        {
            get { return digit4; }
            set
            {
                digit4 = value;
                RaisePropertyChanged("Digit4");
            }
        }
        #endregion

        #region 构造方法
        public TtcViewModel()
        {
            this.pcm = "PCM:A通道";
            this.bcr = "BCR:B通道";
            this.status0 = this.status1 = this.status2 = this.status3 = this.digit = "0x";
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<MsgOfResult>(this, "TMRSLT", HandleTtc);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region 方法
        private void HandleTtc(MsgOfResult msg)
        {
            if (msg.pcm == 0)
                Pcm = "PCM:A通道";
            else
                Pcm = "PCM:A+B通道";
            if (msg.bcr == 0)
                Bcr = "BCR:A通道";
            else
                Bcr = "BCR:B通道";

            Status0 = "0x" + Convert.ToString(msg.state0, 16);
            Status00 = ByteHelper.GetBit(msg.state0, 0, 0) == 1 ? "1" : string.Empty;
            Status01 = ByteHelper.GetBit(msg.state0, 1, 1) == 1 ? "1" : string.Empty;
            Status02 = ByteHelper.GetBit(msg.state0, 2, 2) == 1 ? "1" : string.Empty;
            Status03 = ByteHelper.GetBit(msg.state0, 3, 3) == 1 ? "1" : string.Empty;
            Status04 = ByteHelper.GetBit(msg.state0, 4, 4) == 1 ? "1" : string.Empty;
            Status05 = ByteHelper.GetBit(msg.state0, 5, 5) == 1 ? "1" : string.Empty;
            Status06 = ByteHelper.GetBit(msg.state0, 6, 6) == 1 ? "1" : string.Empty;
            Status07 = ByteHelper.GetBit(msg.state0, 7, 7) == 1 ? "1" : string.Empty;
            Status1 = "0x" + Convert.ToString(msg.state1, 16);
            Status10 = ByteHelper.GetBit(msg.state1, 0, 0) == 1 ? "1" : string.Empty;
            Status11 = ByteHelper.GetBit(msg.state1, 2, 2) == 1 ? "1" : string.Empty;
            Status12 = ByteHelper.GetBit(msg.state1, 5, 5) == 1 ? "1" : string.Empty;
            Status13 = ByteHelper.GetBit(msg.state1, 6, 6) == 1 ? "1" : string.Empty;
            Status14 = ByteHelper.GetBit(msg.state1, 7, 7) == 1 ? "1" : string.Empty;
            Status2 = "0x" + Convert.ToString(msg.state2, 16);
            Status20 = ByteHelper.GetBit(msg.state2, 0, 0) == 1 ? "1" : string.Empty;
            Status21 = ByteHelper.GetBit(msg.state2, 1, 1) == 1 ? "1" : string.Empty;
            Status22 = ByteHelper.GetBit(msg.state2, 2, 2) == 1 ? "1" : string.Empty;
            Status23 = ByteHelper.GetBit(msg.state2, 4, 4) == 1 ? "1" : string.Empty;
            Status24 = ByteHelper.GetBit(msg.state2, 5, 5) == 1 ? "1" : string.Empty;
            Status25 = ByteHelper.GetBit(msg.state2, 6, 6) == 1 ? "1" : string.Empty;
            Status3 = "0x" + Convert.ToString(msg.state3, 16);
            Status30 = ByteHelper.GetBit(msg.state3, 0, 0) == 1 ? "1" : string.Empty;
            Status31 = ByteHelper.GetBit(msg.state3, 1, 1) == 1 ? "1" : string.Empty;
            Status32 = ByteHelper.GetBit(msg.state3, 2, 2) == 1 ? "1" : string.Empty;
            Status33 = ByteHelper.GetBit(msg.state3, 4, 4) == 1 ? "1" : string.Empty;
            Status34 = ByteHelper.GetBit(msg.state3, 6, 6) == 1 ? "1" : string.Empty;

            byte digit = Convert.ToByte(msg.digit);  //数字量第1路
            Digit = "0x" + Convert.ToString(digit, 16);
            Digit0 = ByteHelper.GetBit(digit, 1, 1) == 1 ? "1" : string.Empty;
            Digit1 = ByteHelper.GetBit(digit, 2, 2) == 1 ? "1" : string.Empty;
            Digit2 = ByteHelper.GetBit(digit, 3, 3) == 1 ? "1" : string.Empty;
            Digit3 = ByteHelper.GetBit(digit, 4, 4) == 1 ? "1" : string.Empty;
            Digit4 = ByteHelper.GetBit(digit, 6, 6) == 1 ? "1" : string.Empty;

            PanelVolt = msg.panelVolt.val;  //帆板电压
            PanelCurrXP = msg.panelCurrXP.val;  //帆板+X电流:mA
            PanelTempXP = msg.panelTempXP.val;  //帆板+X温度:℃
            PanelCurrXN = msg.panelCurrXN.val;  //帆板-X电流:mA
            PanelTempXN = msg.panelTempXN.val;  //帆板-X温度:℃
            PanelCurrYP = msg.panelCurrYP.val;  //帆板+Y电流:mA
            PanelTempYP = msg.panelTempYP.val;  //帆板+Y温度:℃
            PanelCurrYN = msg.panelCurrYN.val;  //帆板-Y电流:mA
            PanelTempYN = msg.panelTempYN.val;  //帆板-Y温度:℃
            BatteryVolt = msg.batteryVolt.val;  //电池组电压:V
            BatteryCurr = msg.batteryCurr.val;  //电池组电流:mA
            BatteryTemp = msg.batteryTemp.val;  //电池组温度:℃
            LineVolt14 = msg.lineVolt14.val;  //+14V母线电压:V
            LineCurr14 = msg.lineCurr14.val;  //+14V母线电流:mA
            LineVolt12P = msg.lineVolt12P.val;  //+12V电压:V
            LineCurr12P = msg.lineCurr12P.val;  //+12V电流:mA
            LineVolt12N = msg.lineVolt12N.val;  //-12V电压:V
            LineCurr12N = msg.lineCurr12N.val;  //-12V电流:A
            LineVolt5 = msg.lineVolt5.val;  //+5V电压:V
            LineCurr5 = msg.lineCurr5.val;  //+5V电流:mA
            SubCurrPcm = msg.subCurrPcm.val;  //PCM电流:mA
            SubVoltBd2 = msg.subVoltBd2.val;  //BD2电压:V
            SubCurrObc = msg.subCurrObc.val;  //OBC电流:A
            TempObc = msg.tempObc.val;  //OBC温度:℃
            SubCurrTx = msg.subCurrTx.val;  //TX电流:mA
            TempRf = msg.tempRf.val;  //RF温度:℃
            RxAgc = msg.rxAgc.val;  //RX AGC电平遥测:dB
            RxAfc = msg.rxAfc.val;  //RX AFC电平遥测:Hz
            TxPower = msg.txPower.val;  //TX功放输出功率:mW
        }
        #endregion
    }
}
