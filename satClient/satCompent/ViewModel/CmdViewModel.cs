using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;

using TSFCS.DMDS.Client.Model;

namespace TSFCS.DMDS.Client.ViewModel
{
    /// <summary>
    /// PDXP帧头
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    public struct UH
    {
        [FieldOffset(0)]
        public byte ver;  //版本
        [FieldOffset(1)]
        public ushort mid;  //卫星代号
        [FieldOffset(3)]
        public uint sid;  //信源
        [FieldOffset(7)]
        public uint did;  //信宿
        [FieldOffset(11)]
        public uint bid;  //信息类别
        [FieldOffset(15)]
        public uint no;  //包序号
        [FieldOffset(19)]
        public byte flag;  //信息标志
        [FieldOffset(20)]
        public uint res;  //保留
        [FieldOffset(24)]
        public ushort jd;  //积日
        [FieldOffset(26)]
        public uint js;  //积秒
        [FieldOffset(30)]
        public ushort len;  //数据长度
    }

    [StructLayout(LayoutKind.Explicit, Pack = 4)]
    public struct CmdInfo
    {
        [FieldOffset(0)]
        public UH uh;  //协议帧头
        [FieldOffset(32)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string called;  //指令代号
        [FieldOffset(40)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string content;  //指令描述
        [FieldOffset(296)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string chain;  //指令链名称
        [FieldOffset(304)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string parameters;  //参数
    }

    public class CmdViewModel : ViewModelBase
    {
        #region Field
        private ObservableCollection<CmdModel> cmd;
        #endregion

        #region Property
        public ObservableCollection<CmdModel> Cmd
        {
          get { return cmd; }
          set 
          {
              cmd = value;
              RaisePropertyChanged("Cmd");
          }
        }
        #endregion

        #region Constructor
        public CmdViewModel()
        {
            cmd = CmdModel.GetCmd();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<byte[]>(this, "TC2IMCS00", Refresh);
        }
        #endregion

        #region Override Method
        public override void Cleanup()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister(this);
        }
        #endregion

        #region Method
        public void Refresh(byte[] data)
        {
            GCHandle ptr = GCHandle.Alloc(data, GCHandleType.Pinned);
            CmdInfo info = (CmdInfo)Marshal.PtrToStructure(ptr.AddrOfPinnedObject(), typeof(CmdInfo));

            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                cmd.Insert(0, new CmdModel() { Time = string.Format("{0:yyyy/MM/dd HH:mm:ss}", DateTime.Now), Called = info.called, Content = info.content, Chain = info.chain, Parameters = info.parameters });
                if (cmd.Count > 10)
                    cmd.RemoveAt(cmd.Count - 1);
            });

            ptr.Free();
        }
        #endregion
    }
}
