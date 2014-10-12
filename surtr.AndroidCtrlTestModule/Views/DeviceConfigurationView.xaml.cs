using System.Windows.Controls;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.Services;
using surtr.AndroidCtrlTestModule.ViewModels;

namespace surtr.AndroidCtrlTestModule.Views
{
    /// <summary>
    /// Logique d'interaction pour DeviceMonitorView.xaml
    /// </summary>
    public partial class DeviceConfigurationView : UserControl, IView
    {
        public DeviceConfigurationView(DeviceConfigurationViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
