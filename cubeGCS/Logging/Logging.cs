﻿using System;

using System.Diagnostics;
using System.IO;

namespace Logging
{
    public class Logging: TraceListener
    {

        //private string logFileName = Directory.GetCurrentDirectory();

        public string logFileName { get; private set; }

        public Logging(string Path)
        {
            string time_now = DateTime.Now.ToString("yyyy-MM-dd") +
     '(' + DateTime.Now.ToLongTimeString().ToString().Replace(':', '-') + ')';

            logFileName = Path + time_now;
        }

        public override void Write(string message)
        {
            File.AppendAllText(logFileName + "--operator.log", message);
        }

        public override void WriteLine(string message)
        {
            File.AppendAllText(logFileName + "--operator.log" , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ") + message + Environment.NewLine);
        }


        public override void Write(object o, string category)
        {
            string msg = "";

            if (string.IsNullOrWhiteSpace(category) == false) //category参数不为空
            {
                msg = category + " : ";
            }

            if (o is Exception) //如果参数o是异常类,输出异常消息+堆栈,否则输出o.ToString()
            {
                var ex = (Exception)o;
                msg += ex.Message + Environment.NewLine;
                msg += ex.StackTrace;
            }
            else if (o != null)
            {
                msg = o.ToString();
            }

            WriteLine(msg);
        }

    }
}
