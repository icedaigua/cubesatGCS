using System;
using System.Reflection;
using System.Diagnostics;

namespace JSONHelper
{
    public static class ipyProc
    {
        public static string ipyRun(string funcName, float fval)
        {

            try
            {
                var engine = IronPython.Hosting.Python.CreateEngine();
                var scope = engine.CreateScope();
                var source = engine.CreateScriptSourceFromFile("ipyrun.py");
                source.Execute(scope);
                var func = scope.GetVariable<Func<object, object>>(funcName);
                var text = func(fval).ToString();
                return text;
            }
            catch(Exception ex)
            {
                Trace.WriteLine("PYthon调用错误:" + ex.Message);
                return "";
            }
        }

        public static string ipyRunParams(string funcName, object[] values)
        {

            var engine = IronPython.Hosting.Python.CreateEngine();
            var scope = engine.CreateScope();
            var source = engine.CreateScriptSourceFromFile("ipyrun.py");
            source.Execute(scope);
            var func = scope.GetVariable<Func<object>>(funcName);
            var text = func().ToString();
            return text;
        }

        /// </summary>
        /// <param name="coeff"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReflectionRun(string coeff, string value)
        {
            //"coeff":"func,functionName,a,b"

            try
            {
                string[] str = coeff.Split(',');
                if (str[0] == "func")
                {
                    //使用反射，传递函数名
                    string functionName = str[1];

                    //1.Load（命名空间名称），GetType（命名空间.类名）
                    Type ty = Assembly.Load("satProcessHelper").GetType("satProcessHelper.satFuncHelper");

                    //1.2 另一种方法
                    //var ty = Type.GetType("DataProcess.DataProcessFunc");

                    //2.GeMethod(需要调用的方法名称)
                    MethodInfo method = ty.GetMethod(functionName);
                    //3.调用的实例化方法（非静态方法），需要创建类型的一个实例
                    //object obj = Activator.CreateInstance(ty);
                    //4.方法需要传入的参数
                    object[] parameters = new object[] { value, str };
                    //5.调用方法
                    return method.Invoke(null, parameters).ToString(); //非静态方法将null改为obj
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("调用函数错误:" + ex.Message + ex.StackTrace);
                return null;
            }
        }
    }
}
