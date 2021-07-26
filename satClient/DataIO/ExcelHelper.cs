
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
        public int DataTableToExcel(DataTable sourceData)
        {

            if (sourceData == null)
            {
                throw new ArgumentException("要写入的DataTable不能为空");
            }

            if (!File.Exists(excelName)) //文件不存在,先创建excel
            {
                //createNewExcel();
                throw new ArgumentException("excel不存在");
            }

            try
            {

                FileStream fs = new FileStream(excelName, FileMode.Open);
 
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet1 = workbook.GetSheet(sourceData.TableName);

                if (sheet1 == null)
                {
                    throw new ArgumentException("写入的sheet不存在");
                }

                int num = sheet1.LastRowNum+1;//获取最大行数
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
                return sheet1.LastRowNum+1;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("写入excel错误:" + ex.Message);
                return -1;
            }

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


        public void createNewExcel(DataTable[] dtArr)
        {
            try
            {
                if (!File.Exists(excelName)) //文件不存在,先创建excel
                {
                    FileStream fs = new FileStream(excelName, FileMode.CreateNew);
                    IWorkbook workbook = new XSSFWorkbook();

                    setExcelFormat(workbook);   //模式设置没有效果,待检查

                    ISheet sheet0 = workbook.CreateSheet("origin");
                    foreach (DataTable dt in dtArr)
                    {
                        ISheet sheet1 = workbook.CreateSheet(dt.TableName);                   
                    }
                    ISheet sheet2 = workbook.CreateSheet("上行遥控");

                    workbook.Write(fs);
                    fs.Close();

                    createOriginTableFirstLine(); //写入第一行
                    foreach (DataTable dt in dtArr)  
                    {
                        DataTableToExcel(dt);
                    }
                    createUpCmdTableFirstLine();
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine("创建excel错误:" +ex.Message);
                return;
            }
         
        }

        private void createOriginTableFirstLine()
        {
       
            DataTable dt = new DataTable("origin");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //第一列为当前系统时间
            DataColumn dc = new DataColumn();
            dt.Columns.Add(dc);
            dr[0] = "时间";//DateTime.UtcNow;         
            for (ushort kc=1;kc<256;kc++)
            {
                DataColumn dc1 = new DataColumn();
                dt.Columns.Add(dc1);
                dr[kc] = "第"+ kc.ToString("")+"列";
            }
            DataTableToExcel(dt);
        }

        private void createUpCmdTableFirstLine()
        {

            DataTable dt = new DataTable("上行遥控");

            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            //第一列为当前系统时间
            DataColumn dc = new DataColumn();
            dt.Columns.Add(dc);
            dr[0] = "时间";//DateTime.UtcNow;

            //第二列为指令名称
            DataColumn dc1 = new DataColumn();
            dt.Columns.Add(dc1);
            dr[1] = "指令名称";

            //第三列为指令ASCII码
            DataColumn dc2 = new DataColumn();
            dt.Columns.Add(dc2);
            dr[2] = "指令";

            DataTableToExcel(dt);
        }


        private void setExcelFormat(IWorkbook wb)
        {
            ICellStyle style1 = wb.CreateCellStyle();//样式
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
            style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                                                                                  //设置边框
            style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style1.WrapText = true;//自动换行
        }

        #region IDisposable 成员

        public void Dispose()
        {
 
        }

        #endregion


        #region 无用
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

        private void excelTest()
        {
            //创建一个表
            //System.Data.DataTable dt = new System.Data.DataTable("Product");
            //System.Data.DataColumn dc = null;

            ////添加列，赋值
            //dc = dt.Columns.Add("id", Type.GetType("System.Int32"));
            //dc.AutoIncrement = true;
            //dc.AutoIncrementSeed = 1;
            //dc.AutoIncrementStep = 1;
            //dc.AllowDBNull = false;
            //dt.Columns.Add("pname", Type.GetType("System.String"));
            //dt.Columns.Add("price", Type.GetType("System.Double"));

            //System.Data.DataRow dr = dt.NewRow();
            //dr["pname"] = "red apple";
            //dr["price"] = 9.9;

            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["pname"] = "black apple";
            //dr["price"] = 19.9;  
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["pname"] = "gold apple";
            //dr["price"] = 29.9;
            //dt.Rows.Add(dr);

            //excelApp.DataTableToExcel(dt, "姿控");
        }
        #endregion


    }
}
