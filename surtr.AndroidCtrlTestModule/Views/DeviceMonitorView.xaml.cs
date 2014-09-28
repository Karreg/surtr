using System.Windows.Controls;
using Microsoft.Practices.Prism.Mvvm;
using surtr.AndroidCtrlTestModule.ViewModels;

namespace surtr.AndroidCtrlTestModule.Views
{
    /// <summary>
    /// Logique d'interaction pour DeviceMonitorView.xaml
    /// </summary>
    public partial class DeviceMonitorView : UserControl, IView
    {
        public DeviceMonitorView(DeviceMonitorViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
