﻿using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
 
        public partial class App : Application
        {
            public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://bambushootwindows.azurewebsites.net"
        );
        }
    }

