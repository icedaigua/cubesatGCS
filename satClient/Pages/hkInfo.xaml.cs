using System.Collections.Generic;
using System.Windows.Controls;

using DataProcess;

namespace Pages
{
    /// <summary>
    /// hkInfo.xaml 的交互逻辑
    /// </summary>
    public partial class hkInfo : UserControl
    {

       // private satDataParse satProc = new satDataParse();
        public hkInfo()
        {
            InitializeComponent();
        }

        public void hkInfo_Initz()
        {
           // satProc.satDataProcess_Initz("enlai.json");


          //  if(satProc.getSatDataName() != null)
          //      sat_View.satView_Initz(satProc.getSatDataName());
        }

        public void hkInfo_AddNewView(byte[] buffer)
        {
          //  satProc.DataProcessFunc(buffer);
            //if(satProc.dicForShow!=null)
            //    sat_View.AddNewsatView(satProc.dicForShow);
        }



    }
}
