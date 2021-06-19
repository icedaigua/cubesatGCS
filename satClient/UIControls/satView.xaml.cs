using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Diagnostics;


namespace UIControls
{
    /// <summary>
    /// satView.xaml 的交互逻辑
    /// </summary>
    public partial class satView : UserControl
    {
        private DataTable dt = new DataTable();

        public satView()
        {
            InitializeComponent();
        }



        public void satView_Initz(Dictionary<string, string> dic)
        {
  
            try
            {
                foreach (KeyValuePair<string, string> item in dic)
                {
                    dt.Columns.Add(new DataColumn(item.Value));
                }

                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("初始化satView控制错误:" + ex.Message);
                return;
            }
        }

        public void AddNewsatView(Dictionary<string, string> dic)
        {
            DataRow dr = dt.NewRow();
            UInt16 columIndex = 0;

            //遍历字典输出键与值
            foreach (KeyValuePair<string, string> item in dic)
            {

                //Debug.WriteLine("key:" + item.Key + '\t' + "value:" + item.Value);
                //ss.AppendLine(item.Key + "  :  " + item.Value);
                dr[columIndex++] = (item.Value).ToString();
            }

            //Trace.WriteLine("number = " + columIndex.ToString());
            dt.Rows.Add(dr);

            //this.Dispatcher.Invoke(new Action(() =>
            //{
            //    dt.Rows.Add(dr);
            //}));
        }

    }
}
