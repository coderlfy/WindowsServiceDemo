using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace ECenterService
{
    [RunInstaller(true)]
    public partial class ServiceInstall : System.Configuration.Install.Installer
    {
        public ServiceInstall()
        {
            InitializeComponent();
        }
    }
}
