using System.Windows;
using CreatorMVVMProject.Model.Class.DIBuilder;
using CreatorMVVMProject.Model.Class.Main;
using CreatorMVVMProject.ViewModel.Main;

namespace CreatorMVVMProject.View.Main
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
