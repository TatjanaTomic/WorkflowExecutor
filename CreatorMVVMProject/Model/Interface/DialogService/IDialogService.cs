using CreatorMVVMProject.ViewModel.Message;

namespace CreatorMVVMProject.Model.Interface.DialogService;

public interface IDialogService
{
    public void ShowMessage(MessageViewModel messageViewModel);
}
