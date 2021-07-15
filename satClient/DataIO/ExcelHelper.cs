using System.Runtime.InteropServices; //提供 COM互操作的库
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using System;
using System.IO;
using System.Reflection;

namespace DataIO
{
    public class ExcelHelper
    {
        #region 参数
        private const int MaxRow = 1000000, MaxColumn = 1000;
        private Excel.Application excelApp;
        private string excelName;
        private Excel.Workbooks wb;
        private Excel._Workbook ws;
        #endregion

        public ExcelHelper()
        {
            
        }
        //创建excel应用程序


        public bool openExcel(string excelinfo)
        {
            try
            {
                string currTime = DateTime.Now.ToString("yyyy-MM");
                
                excelName = excelinfo + "\\"+currTime + ".xlsx";
                excelApp = new Excel.Application();
              
                if (File.Exists(excelName))
                {
                    wb = excelApp.Workbooks;
                    ws = wb.Add(excelName);
                }
                else
                {
                    wb = excelApp.Workbooks;
                    ws = wb.Add(true);

                    Excel.Sheets shs = ws.Sheets;
                    addNewSheet("元器件验证载荷");
                    addNewSheet("姿控");
                    addNewSheet("星务电源测控");
                    addNewSheet("上行指令");

                    //这个地方只能采用该方法保存，不然在指定路径下保存文件外，在我的文档中也会生成一个对应的副本
                    ws.SaveAs(excelName, Missing.Value, Missing.Value, 
                        Missing.Value, Missing.Value, Missing.Value, 
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, 
                        Missing.Value, Missing.Value, Missing.Value, 
                        Missing.Value, Missing.Value);
                }

                excelApp.Visible = true;
                excelApp.DisplayAlerts = false; //禁用弹出警告窗口
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("打开Excel错误:" + ex.Message);
                return false;
            }
        }

        private bool addNewSheet(string sheetName)
        {
            //需增加功能判断表格是否存在
            Excel._Worksheet _wsh = excelApp.Worksheets.Add(); //添加新的Excel工作表
            _wsh.Name = sheetName;
            _wsh.Cells.NumberFormat = "@"; //设置数字文本格式
            return true;
         }

        public bool save(string info)
        {

            return true;
        }
            //关闭应用程序
            public void closeExcel()
        {
            try
            {
                if (ws != null)
                {
                    //这个地方只能采用该方法保存，不然在指定路径下保存文件外，在我的文档中也会生成一个对应的副本
                    ws.SaveAs(excelName, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value, Missing.Value,
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value);
                    ws.Close();
                    wb.Close();
                    excelApp.Workbooks.Close(); //关闭所有工作簿
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }           
                excelApp = null;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("关闭Excel错误:" + ex.Message);
            }

        }
    }
}
