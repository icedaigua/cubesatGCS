using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using GalaSoft.MvvmLight;

using satCompent.Model;

namespace satCompent.ViewModel
{
    public class ResultViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<ResultModel> result;
        #endregion

        #region Property
        public ObservableCollection<ResultModel> Result
        {
            get { return result; }
            set 
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }
        #endregion

        #region Contructor
        public ResultViewModel()
        {
            result = ResultModel.GetAllResult();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DataTable>(this, "Result", HandleResult);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Messenger Handler
        public void HandleResult(DataTable msg)
        {
            //result[0].Val = Convert.ToString(msg.panelVolt.val);  //帆板电压
            //result[1].Val = Convert.ToString(msg.panelCurrXP.val);  //帆板+X电流:mA
            //result[2].Val = Convert.ToString(msg.panelTempXP.val);  //帆板+X温度:℃
            //result[3].Val = Convert.ToString(msg.panelCurrXN.val);  //帆板-X电流:mA
            //result[4].Val = Convert.ToString(msg.panelTempXN.val);  //帆板-X温度:℃
            //result[5].Val = Convert.ToString(msg.panelCurrYP.val);  //帆板+Y电流:mA
            //result[6].Val = Convert.ToString(msg.panelTempYP.val);  //帆板+Y温度:℃
            //result[7].Val = Convert.ToString(msg.panelCurrYN.val);  //帆板-Y电流:mA
            //result[8].Val = Convert.ToString(msg.panelTempYN.val);  //帆板-Y温度:℃
            //result[9].Val = Convert.ToString(msg.batteryVolt.val);  //电池组电压:V
            //result[10].Val = Convert.ToString(msg.batteryCurr.val);  //电池组电流:mA
            //result[11].Val = Convert.ToString(msg.batteryTemp.val);  //电池组温度:℃
            //result[12].Val = Convert.ToString(msg.lineVolt14.val);  //+14V母线电压:V
            //result[13].Val = Convert.ToString(msg.lineCurr14.val);  //+14V母线电流:mA
            //result[14].Val = Convert.ToString(msg.lineVolt12P.val);  //+12V电压:V
            //result[15].Val = Convert.ToString(msg.lineCurr12P.val);  //+12V电流:mA
            //result[16].Val = Convert.ToString(msg.lineVolt12N.val);  //-12V电压:V
            //result[17].Val = Convert.ToString(msg.lineCurr12N.val);  //-12V电流:A
            //result[18].Val = Convert.ToString(msg.lineVolt5.val);  //+5V电压:V
            //result[19].Val = Convert.ToString(msg.lineCurr5.val);  //+5V电流:mA
            //result[20].Val = Convert.ToString(msg.subCurrPcm.val);  //PCM电流:mA
            //result[21].Val = Convert.ToString(msg.subVoltBd2.val);  //BD2电压:V
            //result[22].Val = Convert.ToString(msg.subCurrObc.val);  //OBC电流:A
            //result[23].Val = Convert.ToString(msg.tempObc.val);  //OBC温度:℃
            //result[24].Val = Convert.ToString(msg.subCurrTx.val);  //TX电流:mA
            //result[25].Val = Convert.ToString(msg.tempRf.val);  //RF温度:℃
            //result[26].Val = Convert.ToString(msg.rxAgc.val);  //RX AGC电平遥测:dB
            //result[27].Val = Convert.ToString(msg.rxAfc.val);  //RX AFC电平遥测:Hz
            //result[28].Val = Convert.ToString(msg.txPower.val);  //TX功放输出功率:mW

            for (int i = 0; i < msg.Columns.Count; i++)
            {
                this.result[i].Val = Convert.ToString(msg.Rows[0][i]);
            }
        }
        #endregion
    }
}
