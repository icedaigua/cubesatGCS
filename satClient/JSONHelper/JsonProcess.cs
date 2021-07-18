
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace JSONHelper
{
    public class JSONProcess
    {
        //public Dictionary<string, string> dicForShow { get; private set; }
        public Dictionary<string, string> dicForSave { get; private set; }
 
        public JArray ja { get; private set; }

        public JSONProcess(string path)
        {
            try
            {
                StreamReader sr = File.OpenText(path);
                string json = sr.ReadToEnd();
                ja = (JArray)JsonConvert.DeserializeObject(json);
                sr.Close();
            }
            catch
            {
                throw  new ArgumentException("读入json文件错误");
            }
        }

        public void ReloadJSONFile(string path)
        {
            try
            {
                StreamReader sr = File.OpenText(path);
                string json = sr.ReadToEnd();
                ja = (JArray)JsonConvert.DeserializeObject(json);
                sr.Close();
            }
            catch
            {
                throw new ArgumentException("重新载入json文件错误");
            }
        }

        public Dictionary<string, string> getJsonChinese()
        {
            Dictionary<string, string> dicChinese = new Dictionary<string, string>();

            for (var i = 0; i < ja.Count; i++)
            {
                JToken js = JToken.Parse(ja[i].ToString());

                JArray jaa = (JArray)js["content"];
                for (var j = 0; j < jaa.Count; j++)
                {
                    JToken jss = JToken.Parse(jaa[j].ToString());
                    string id = jss["id"].ToString();
                    string idCN = jss["chinese"].ToString();
                    byte showenable = (byte)jss["visible"];
                    if (showenable == 1)
                    {
                        dicChinese.Add(id, idCN);
                    }
                }
            }

            return dicChinese;
        }

        public void DecodePackage(byte[] buf)
        {
            dicForSave = new Dictionary<string, string>();
            //dicForShow = new Dictionary<string, string>();

            try
            {
                JArray jaByHeader = getJsonByHeader(buf);

                if (jaByHeader != null)
                {
                    for (var i = 0; i < jaByHeader.Count; i++)
                    {

                        JToken js = JToken.Parse(jaByHeader[i].ToString());

                        string id = js["id"].ToString();
                        byte leng = (byte)js["leng"];
                        byte index = (byte)js["index"];
                        string type = js["type"].ToString();
                        //byte bit_index = (byte)js["bit-index"];
                        //byte showenable = (byte)js["visible"];
                        string coeff = js["coeff"].ToString();
                        string valueRange = js["range"].ToString();


                        //System.Diagnostics.Debug.WriteLine("id = " + id);
                        string value = getValueByReflection(buf, type, index, leng, coeff, valueRange);
                        dicForSave.Add(id, value);
                        //if (showenable == 1)
                        //    dicForShow.Add(id, value);

                    }
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine("Json解析错误:" + ex.Message);
            }
        

        }


        private JArray getJsonByHeader(byte[] header)
        {

            //for (var i = 0; i < ja.Count; i++)
            //{
            //    JToken js = JToken.Parse(ja[i].ToString());

            //    byte header0 = (byte)js["header"];

            //    if ((header0 == header[header_index + 0]) && (header1 == header[header_index + 1]))
            //        return (JArray)js["content"];
            //    else
            //        continue;
            //}

            //return null;
            JToken js = JToken.Parse(ja[0].ToString());
            string header0 = (string)js["header"];
            return (JArray)js["content"];
        }


        private string getValueByReflection(byte[] buf, string type, byte index, byte length, string coeff, string vRange)
        {
            //dicForOrigin = new Dictionary<string, string>();
            string value = "", hex_value = "";
            byte[] covBuf = new byte[4] { 0, 0, 0, 0 };
            switch (type)
            {
                case "byte":
                    byte bvalue = buf[index];
                    hex_value = bvalue.ToString("X");
                    value = hex_value + ',' + bvalue.ToString() + ',' + computeRealValue(bvalue, coeff);
                    break;
                case "uint16":    //高位在前,低位在后
                    UInt16 uival = 0;
                    covBuf[0] = buf[index + 1];
                    covBuf[1] = buf[index + 0];
                    uival = BitConverter.ToUInt16(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X");
                    value = hex_value + ',' + uival.ToString() + ',' + computeRealValue(uival, coeff);
                    break;
                case "-uint16": //低位在前,高位在后
                    UInt16 uival_M = 0;
                    uival_M = BitConverter.ToUInt16(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X");
                    value = hex_value + ',' + uival_M.ToString() + ',' + computeRealValue(uival_M, coeff);
                    break;
                case "int16":  //高位在前,低位在后
                    Int16 ival = 0;
                    covBuf[0] = buf[index + 1];
                    covBuf[1] = buf[index + 0];
                    ival = BitConverter.ToInt16(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X");
                    value = hex_value + ',' + ival.ToString() + ',' + computeRealValue(ival, coeff);
                    break;
                case "-int16": //低位在前,高位在后
                    Int16 ival_M = 0;
                    ival_M = BitConverter.ToInt16(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X");
                    value = hex_value + ',' + ival_M.ToString() + ',' + computeRealValue(ival_M, coeff);
                    break;

                case "float":      //高位在前,低位在后
                    float fval;
                    covBuf[0] = buf[index + 3];
                    covBuf[1] = buf[index + 2];
                    covBuf[2] = buf[index + 1];
                    covBuf[3] = buf[index + 0];
                    fval = BitConverter.ToSingle(covBuf, 0);

                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X")
                        + "  " + covBuf[2].ToString("X") + "  " + covBuf[3].ToString("X");

                    value = hex_value + ',' + fval.ToString() + ',' + computeRealValue(fval, coeff);
                    break;
                case "-float":    //低位在前,高位在后
                    float fval_M;
                    fval_M = BitConverter.ToSingle(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X")
                        + "  " + buf[index + 1].ToString("X") + "  " + buf[index + 1].ToString("X");
                    value = hex_value + ',' + fval_M.ToString() + ',' + computeRealValue(fval_M, coeff);
                    break;

                case "uint32":     //高位在前,低位在后
                    UInt32 ui32val = 0;
                    covBuf[0] = buf[index + 3];
                    covBuf[1] = buf[index + 2];
                    covBuf[2] = buf[index + 1];
                    covBuf[3] = buf[index + 0];
                    ui32val = BitConverter.ToUInt32(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X")
                       + "  " + covBuf[2].ToString("X") + "  " + covBuf[3].ToString("X");
                    value = hex_value + ',' + ui32val.ToString() + ',' + computeRealValue(ui32val, coeff);
                    break;

                case "-uint32": //低位在前,高位在后
                    UInt32 ui32val_M = 0;
                    ui32val_M = BitConverter.ToUInt32(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X")
                         + "  " + buf[index + 2].ToString("X") + "  " + buf[index + 3].ToString("X");
                    value = hex_value + ',' + ui32val_M.ToString() + ',' + computeRealValue(ui32val_M, coeff);
                    break;

                case "int32":      //高位在前,低位在后
                    Int32 i32val_M = 0;
                    covBuf[0] = buf[index + 3];
                    covBuf[1] = buf[index + 2];
                    covBuf[2] = buf[index + 1];
                    covBuf[3] = buf[index + 0];
                    i32val_M = BitConverter.ToInt32(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X")
                  + "  " + covBuf[2].ToString("X") + "  " + covBuf[3].ToString("X");
                    value = hex_value + ',' + i32val_M.ToString() + ',' + computeRealValue(i32val_M, coeff);
                    break;

                case "-int32": //低位在前,高位在后
                    Int32 i32val = 0;
                    i32val = BitConverter.ToInt32(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X")
                       + "  " + buf[index + 2].ToString("X") + "  " + buf[index + 3].ToString("X");
                    value = hex_value + ',' + i32val.ToString() + ',' + computeRealValue(i32val, coeff);
                    break;

                case "bit":
                    //byte bitVal = 0;

                    //byte aa = (byte)(buf[index] >> bit_index);
                    //byte bb = (byte)(Math.Pow(2, length - 1));
                    //byte cc = (byte)(aa & bb);

                    //bitVal = (byte)((byte)(buf[index] >> bit_index) & (byte)(Math.Pow(2, length) - 1));
                    ////value =
                    //hex_value = buf[index].ToString("X");
                    //value = hex_value + ',' + bitVal.ToString() + ',' + computeRealValue(bitVal, coeff); ;
                    break;
                case "int64":
                    value = BitConverter.ToInt64(buf, index).ToString();
                    break;
                case "uint64":
                    value = BitConverter.ToUInt64(buf, index).ToString();
                    break;
                case "double":
                    value = BitConverter.ToDouble(buf, index).ToString();
                    break;

                default:
                    break;
            }
            return value;
        }

        private string getValue(byte[] buf, string type, byte index, byte bit_index, byte length, string coeff, string vRange)
        {
            //dicForOrigin = new Dictionary<string, string>();
            string value = "", hex_value = "";
            byte[] covBuf = new byte[4] { 0, 0, 0, 0 };
            switch (type)
            {
                case "byte":
                    byte bvalue = buf[index];
                    hex_value = bvalue.ToString("X");
                    value = hex_value + ',' + bvalue.ToString() + ',' + computeRealValue_Byte(bvalue, coeff);
                    break;
                case "uint16":    //高位在前,低位在后
                    UInt16 uival = 0;
                    covBuf[0] = buf[index + 1];
                    covBuf[1] = buf[index + 0];
                    uival = BitConverter.ToUInt16(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X");
                    value = hex_value + ',' + uival.ToString() + ',' + computeRealValue_UInt16(uival, coeff);
                    break;
                case "-uint16": //低位在前,高位在后
                    UInt16 uival_M = 0;
                    uival_M = BitConverter.ToUInt16(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X");
                    value = hex_value + ',' + uival_M.ToString() + ',' + computeRealValue_UInt16(uival_M, coeff);
                    break;
                case "int16":  //高位在前,低位在后
                    Int16 ival = 0;
                    covBuf[0] = buf[index + 1];
                    covBuf[1] = buf[index + 0];
                    ival = BitConverter.ToInt16(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X");
                    value = hex_value + ',' + ival.ToString() + ',' + computeRealValue_Int16(ival, coeff);
                    break;
                case "-int16": //低位在前,高位在后
                    Int16 ival_M = 0;
                    ival_M = BitConverter.ToInt16(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X");
                    value = hex_value + ',' + ival_M.ToString() + ',' + computeRealValue_Int16(ival_M, coeff);
                    break;
          
                case "float":      //高位在前,低位在后
                    float fval;
                    covBuf[0] = buf[index + 3];
                    covBuf[1] = buf[index + 2];
                    covBuf[2] = buf[index + 1];
                    covBuf[3] = buf[index + 0];
                    fval = BitConverter.ToSingle(covBuf, 0);

                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X")
                        + "  " + covBuf[2].ToString("X") + "  " + covBuf[3].ToString("X");

                    value = hex_value + ',' + fval.ToString() + ',' + computeRealValue_Float(fval, coeff);
                    break;
                case "-float":    //低位在前,高位在后
                    float fval_M;
                    fval_M = BitConverter.ToSingle(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X")
                        + "  " + buf[index + 1].ToString("X") + "  " + buf[index + 1].ToString("X");
                    value = hex_value + ',' + fval_M.ToString() + ',' + computeRealValue_Float(fval_M, coeff);
                    break;
             
                case "uint32":     //高位在前,低位在后
                    UInt32 ui32val = 0;
                    covBuf[0] = buf[index + 3];
                    covBuf[1] = buf[index + 2];
                    covBuf[2] = buf[index + 1];
                    covBuf[3] = buf[index + 0];
                    ui32val = BitConverter.ToUInt32(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X")
                       + "  " + covBuf[2].ToString("X") + "  " + covBuf[3].ToString("X");
                    value = hex_value + ',' + ui32val.ToString() + ',' + computeRealValue_UInt32(ui32val, coeff);
                    break;

                case "-uint32": //低位在前,高位在后
                    UInt32 ui32val_M = 0;
                    ui32val_M = BitConverter.ToUInt32(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X")
                         + "  " + buf[index + 2].ToString("X") + "  " + buf[index + 3].ToString("X");
                    value = hex_value + ',' + ui32val_M.ToString() + ',' + computeRealValue_UInt32(ui32val_M, coeff);
                    break;
              
                case "int32":      //高位在前,低位在后
                    Int32 i32val_M = 0;
                    covBuf[0] = buf[index + 3];
                    covBuf[1] = buf[index + 2];
                    covBuf[2] = buf[index + 1];
                    covBuf[3] = buf[index + 0];
                    i32val_M = BitConverter.ToInt32(covBuf, 0);
                    hex_value = covBuf[0].ToString("X") + "  " + covBuf[1].ToString("X")
                  + "  " + covBuf[2].ToString("X") + "  " + covBuf[3].ToString("X");
                    value = hex_value + ',' + i32val_M.ToString() + ',' + computeRealValue_Int32(i32val_M, coeff);
                    break;

                case "-int32": //低位在前,高位在后
                    Int32 i32val = 0;
                    i32val = BitConverter.ToInt32(buf, index);
                    hex_value = buf[index].ToString("X") + "  " + buf[index + 1].ToString("X")
                       + "  " + buf[index + 2].ToString("X") + "  " + buf[index + 3].ToString("X");
                    value = hex_value + ',' + i32val.ToString() + ',' + computeRealValue_Int32(i32val, coeff);
                    break;

                case "bit":
                    byte bitVal = 0;

                    byte aa = (byte)(buf[index] >> bit_index);
                    byte bb = (byte)(Math.Pow(2, length - 1));
                    byte cc = (byte)(aa & bb);

                    bitVal = (byte)((byte)(buf[index] >> bit_index) & (byte)(Math.Pow(2, length) - 1));
                    //value =
                    hex_value = buf[index].ToString("X");
                    value = hex_value + ',' + bitVal.ToString() + ',' + computeRealValue_Byte(bitVal, coeff); ;
                    break;
                case "int64":
                    value = BitConverter.ToInt64(buf, index).ToString();
                    break;
                case "uint64":
                    value = BitConverter.ToUInt64(buf, index).ToString();
                    break;
                case "double":
                    value = BitConverter.ToDouble(buf, index).ToString();
                    break;

                default:
                    break;
            }
            return value;
        }


        private byte checkDataValid(float fval, string vRange)
        {
            string[] range_para = vRange.Split(',');

            if (range_para[0] == "none") return 1;

            if ((fval >= float.Parse(range_para[0])) && (fval <= float.Parse(range_para[1])))
                return 1;
            else
                return 0;
        }

        #region 参数解析计算
        private string computeRealValue<T>(T para, string coeff) where T:IConvertible
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            //typeof(T)

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                   // value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ReflectionRun(coeff, para.ToString());
                    break;
                default:
                    break;
            }

            return value;
        }
        private string computeRealValue_Byte(byte para, string coeff)
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }

            return value;
        }

        private string computeRealValue_Int16(Int16 para, string coeff)
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }

            return value;
        }
        private string computeRealValue_UInt16(UInt16 para, string coeff)
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }
            return value;
        }
        private string computeRealValue_Int32(Int32 para, string coeff)
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }
            return value;
        }
        private string computeRealValue_UInt32(UInt32 para, string coeff)
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }
            return value;
        }
        private string computeRealValue_Float(float para, string coeff)
        {
            string value = "";
            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }

            return value;
        }
        private string computeRealValue_Bit(byte para, string coeff)
        {
            string value = "";

            string[] coeff_para = coeff.Split(',');

            switch (coeff_para[0])
            {
                case "none":
                    value = para.ToString();
                    break;
                case "para":
                    value = (para * float.Parse(coeff_para[1]) + float.Parse(coeff_para[2])).ToString();
                    break;
                case "func":
                    value = ipyProc.ipyRun(coeff_para[1], (float)para);
                    break;
                default:
                    break;
            }

            return value;
        }
        #endregion
    }

}
