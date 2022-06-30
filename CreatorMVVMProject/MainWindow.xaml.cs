using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.ViewModel.Main;
using System.Windows;
using CreatorMVVMProject.Model.Class.DIBuilder;
using Autofac;
using CreatorMVVMProject.Model.Interface.StatusReportService;

namespace CreatorMVVMProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new MainViewModel(ServiceContainer.Resolve<MainModel>());    
        }
    }
}
