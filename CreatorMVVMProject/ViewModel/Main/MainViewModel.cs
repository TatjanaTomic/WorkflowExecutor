using CreatorMVVMProject.Model.Class.StatusReportService;
using CreatorMVVMProject.Model.Class.Main;
using System.Collections.Generic;

namespace CreatorMVVMProject.ViewModel.Main
{
    public class MainViewModel
    {
        private readonly MainModel mainModel;
        private List<StageViewModel> stageViewModels = new();
        private StageViewModel selectedStage;

        public MainViewModel(MainModel model)
        {
            this.mainModel = model;
            foreach(StageStatus stage in this.mainModel.Stages)
            {
                stageViewModels.Add(new(stage));
            }
            this.selectedStage = stageViewModels[0];
        }

        public List<StageViewModel> StageViewModels
        {
            get => this.stageViewModels;
            set => this.stageViewModels = value;
        }
        public StageViewModel SelectedStage
        {
            get => this.selectedStage;
            set => this.selectedStage = value;
        }

    }
}
