
using System;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Diagnostics;

namespace DataIO
{
    public class ExcelHelper : IDisposable
    {
        #region 参数
        private const int MaxRow = 1000000, MaxColumn = 1000;
        private string excelName;

        #endregion

        public ExcelHelper(string excelinfo)
        {
            string currTime = DateTime.Now.ToString("yyyy-MM-dd");
            excelName = excelinfo + "\\" + currTime + ".xlsx";
        }


        /// <summary>
        /// 把DataTable的数据写入到指定的excel文件中
        /// </summary>
        /// <param name="sourceData">要写入的数据</param>
        /// <param name="sheetName">excel表中的sheet的名称，可以根据情况自己起</param>
        /// <returns>返回写入的行数</returns>
        public int DataTableToExcel(DataTable sourceData, string sheetName)
        {

            if (sourceData == null)
            {
                throw new ArgumentException("要写入的DataTable不能为空");
            }

            if (!File.Exists(excelName)) //文件不存在,先创建excel
            {
                createNewExcel();
            }

            FileStream fs = new FileStream(excelName, FileMode.Open);
 
            IWorkbook workbook = new XSSFWorkbook(fs);
            ISheet sheet1 = workbook.GetSheet(sheetName);

            if (sheet1 == null)
            {
                throw new ArgumentException("写入的sheet不存在");
            }

            int num = sheet1.LastRowNum + 1;//获取最大行数
            //写入数据
            for (int row = 0; row < sourceData.Rows.Count; row++)
            {
                //sheet表创建新的一行
                IRow newRow = sheet1.CreateRow(num + row);
                for (int column = 0; column < sourceData.Columns.Count; column++)
                {

                    newRow.CreateCell(column).SetCellValue(sourceData.Rows[row][column].ToString());

                }
            }

            FileStream fout = new FileStream(excelName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);//写入流

            workbook.Write(fout);
            fs.Close();
            fout.Close();
            return sheet1.LastRowNum + 1;

        }





        /// <summary>
        /// 从Excel中读入数据到DataTable中
        /// </summary>
        /// <param name="sourceFileNamePath">Excel文件的路径</param>
        /// <param name="sheetName">excel文件中工作表名称</param>
        /// <param name="IsHasColumnName">文件是否有列名</param>
        /// <returns>从Excel读取到数据的DataTable结果集</returns>
        public DataTable ExcelToDataTable(string sheetName)
        {

            if (!File.Exists(excelName))
            {
                throw new ArgumentException("excel文件的路径不存在或者excel文件没有创建好");
            }

            if (sheetName == null || sheetName.Length == 0)
            {
                throw new ArgumentException("工作表sheet的名称不能为空");
            }

            //根据Excel文件的后缀名创建对应的workbook
            IWorkbook workbook = null;
            //打开文件
            FileStream fs = new FileStream(excelName, FileMode.Open, FileAccess.Read);
            if (excelName.IndexOf(".xlsx") > 0)
            {  //2007版本的excel
                workbook = new XSSFWorkbook(fs);
            }
            else if (excelName.IndexOf(".xls") > 0) //2003版本的excel
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                return null;    //都不匹配或者传入的文件根本就不是excel文件，直接返回
            }




            //获取工作表sheet
            ISheet sheet = workbook.GetSheet(sheetName);
            //获取不到，直接返回
            if (sheet == null) return null;



            //开始读取的行号
            int StartReadRow = 0;
            DataTable targetTable = new DataTable();


            //为DataTable添加列名 
            {
                //获取要读取的工作表的第一行
                IRow columnNameRow = sheet.GetRow(0);   //0代表第一行
                                                        //获取该行的列数(即该行的长度)
                int CellLength = columnNameRow.LastCellNum;

                //遍历读取
                for (int columnNameIndex = 0; columnNameIndex < CellLength; columnNameIndex++)
                {
                    //不为空，则读入
                    if (columnNameRow.GetCell(columnNameIndex) != null)
                    {
                        //获取该单元格的值
                        string cellValue = columnNameRow.GetCell(columnNameIndex).StringCellValue;
                        if (cellValue != null)
                        {
                            //为DataTable添加列名
                            targetTable.Columns.Add(new DataColumn(cellValue));
                        }
                    }
                }

                StartReadRow++;
            }



            ///开始读取sheet表中的数据

            //获取sheet文件中的行数
            int RowLength = sheet.LastRowNum;
            //遍历一行一行地读入
            for (int RowIndex = StartReadRow; RowIndex < RowLength; RowIndex++)
            {
                //获取sheet表中对应下标的一行数据
                IRow currentRow = sheet.GetRow(RowIndex);   //RowIndex代表第RowIndex+1行

                if (currentRow == null) continue;  //表示当前行没有数据，则继续
                //获取第Row行中的列数，即Row行中的长度
                int currentColumnLength = currentRow.LastCellNum;

                //创建DataTable的数据行
                DataRow dataRow = targetTable.NewRow();
                //遍历读取数据
                for (int columnIndex = 0; columnIndex < currentColumnLength; columnIndex++)
                {
                    //没有数据的单元格默认为空
                    if (currentRow.GetCell(columnIndex) != null)
                    {
                        dataRow[columnIndex] = currentRow.GetCell(columnIndex);
                    }
                }
                //把DataTable的数据行添加到DataTable中
                targetTable.Rows.Add(dataRow);
            }


            //释放资源
            fs.Close();
            workbook.Close();

            return targetTable;
        }




        private void createNewExcel()
        {
            try
            {
                FileStream fs = new FileStream(excelName, FileMode.CreateNew);
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet1 = workbook.CreateSheet("姿控");
                createExcelHeader(sheet1);

                ISheet sheet2 = workbook.CreateSheet("星务电源测控");
                createExcelHeader(sheet2);

                ISheet sheet3 = workbook.CreateSheet("载荷");
                createExcelHeader(sheet3);

                workbook.Write(fs);
                fs.Close();
            }
            catch(Exception ex)
            {
                Trace.WriteLine("创建excel错误:" +ex.Message);
                return;
            }



           
        }
        private void createExcelHeader(ISheet sheet)
        {
            if (sheet == null)
            {
                throw new ArgumentException("写入的sheet不存在");
            }
            IRow row = sheet.CreateRow(0);

            ICell cell = row.CreateCell(0);  //在第二行中创建单元格
            cell.SetCellValue("名称");//循环往第二行的单元格中添加数据
            cell = row.CreateCell(1);  //在第二行中创建单元格
            cell.SetCellValue("时间");//循环往第二行的单元格中添加数据
            cell = row.CreateCell(2);  //在第二行中创建单元格
            cell.SetCellValue("角度");//循环往第二行的单元格中添加数据

        }


        #region IDisposable 成员

        public void Dispose()
        {
 
        }

        #endregion





    }
}
