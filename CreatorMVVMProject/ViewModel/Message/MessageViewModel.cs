using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;

namespace CreatorMVVMProject.ViewModel.Message
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        private string message;
        private readonly bool isErrorMessage;
        private ICommand? okCommand;

        public MessageViewModel(string message, bool isErrorMessage)
        {
            this.message = message;
            this.isErrorMessage = isErrorMessage;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand OkCommand => okCommand ??= new DelegateCommand<object>(OkCommandHandler);

        public bool IsErrorMessage => isErrorMessage;

        public string Message { 
            get => message;
            set {
                message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        private void OkCommandHandler(object obj)
        {
            if (obj is Window win)
            {
                win.Close();
            }
        }
    }
}
