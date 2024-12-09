using Caliburn.Micro;
using CourseManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseManager
{
    //this where app gets started, hooks for Caliburn
    internal class Startup : BootstrapperBase //using Caliburn.Micro
    {
        public Startup()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewForAsync<MainViewModel>();
    }
}
