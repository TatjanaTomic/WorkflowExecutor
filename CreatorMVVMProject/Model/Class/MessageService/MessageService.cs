using System.Windows;
using CreatorMVVMProject.Model.Interface.DialogService;
using CreatorMVVMProject.View.Message;
using CreatorMVVMProject.ViewModel.Message;

namespace CreatorMVVMProject.Model.Class.DialogService
{
    public class MessageService : IDialogService
    {

        public void ShowMessage(MessageViewModel messageViewModel)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                MessageWindow messageWindow = new MessageWindow
                {
                    DataContext = messageViewModel
                };
                messageWindow.ShowDialog();
            });
            
        }
    }
}
