﻿using System.Windows;
using System.Windows.Input;
using CreatorMVVMProject.Model.Class.Commands;

namespace CreatorMVVMProject.ViewModel.Message
{
    public class MessageViewModel
    {
        private readonly string message;
        private readonly bool isErrorMessage;
        private ICommand? okCommand;

        public MessageViewModel(string message, bool isErrorMessage)
        {
            this.message = message;
            this.isErrorMessage = isErrorMessage;
        }

        public ICommand OkCommand => okCommand ??= new DelegateCommand<object>(OkCommandHandler);

        public string Message => message;

        public bool IsErrorMessage => isErrorMessage;

        private void OkCommandHandler(object obj)
        {
            if (obj is Window win)
            {
                win.Close();
            }
        }
    }
}
