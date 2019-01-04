using CenterService.Entity;
using ClientFramework.Common;
using ECenterWebApiService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace ECenterService
{
    public partial class MainService : ServiceBase
    {
        private delegate void AsyncHandler();
        private static AsyncHandler InitAsynchandler = new AsyncHandler(InitService);
        public bool InitSysConstVar()
        {
            Logger.SysWriteLog("初始化系统常量");
            var ipAddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            var ipv4List = (from n in ipAddressList where n.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork select n).ToList();

            SysConstVar.LocalIP = ipv4List.First().ToString();
            SysConstVar.LocalPath = SysHelper.GetCurPath();

            SysConstVar.LocalIISPath = SysConstVar.LocalPath + "\\LocalIIS";
            SysConstVar.LocalIISCache = SysConstVar.LocalIISPath + "\\App\\Cache\\";

            SysConstVar.LocalBlankHtmlUrl = string.Format("file:///" + SysConstVar.LocalIISPath + "/Blank.html").Replace("\\", "/");

            if (!Directory.Exists(SysConstVar.LocalIISCache))
            {
                Directory.CreateDirectory(SysConstVar.LocalIISCache);
            }

            SysConstVar.LocalAddrPortWebApiBLL = 9028;
            SysConstVar.LocalAddrPortHttpFile = 9029;
            //SysConstVar.LocalAddrPortWebApiFile = 9027;

            SysConstVar.LocalAddrWebApiBLL = string.Format("http://{0}:{1}", "127.0.0.1", SysConstVar.LocalAddrPortWebApiBLL);
            SysConstVar.LocalAddrHttpFile = string.Format("http://127.0.0.1:{0}{1}", SysConstVar.LocalAddrPortHttpFile.ToString(), "/");
            //SysConstVar.LocalAddrWebApiFile = string.Format("http://{0}:{1}", "127.0.0.1", SysConstVar.LocalAddrPortWebApiFile);

            return true;
        }

        public MainService()
        {
            InitializeComponent();
        }

        public static void InitService()
        {
            HostManager.WebApiBLLHostOpen(
                SysConstVar.LocalIP, 
                SysConstVar.LocalAddrPortWebApiBLL.ToString());

            HostManager.HttpFileHostStart(
                SysConstVar.LocalAddrPortHttpFile, 
                "/", 
                SysConstVar.LocalIISPath);
        }


        protected override void OnStart(string[] args)
        {
            if (this.InitSysConstVar())
            {
                InitAsynchandler.BeginInvoke(null, null);
            }

        }

        protected override void OnStop()
        {

        }
    }
}
