using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for ReportViewWindow.xaml
    /// </summary>
    public partial class ReportViewWindow : Window
    {
        private ReportListWindow reportListWindow;
       
    
        //Window constructor that initializes its components
        public ReportViewWindow(ReportListWindow reportListWindow)
        {
            InitializeComponent();
            this.reportListWindow = reportListWindow;
        
     
        }
    }
}
