namespace ArrayOfBytes.PlanningPoker.Droid.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Views;
    using System.Collections.Generic;
    using System.Linq;

    public class SelectPointsViewModel
    {
        public SelectPointsViewModel(INavigationService navigationService)
        {
            var points = new int[] { 1, 2, 3, 5, 8, 13, 20, 40, 100 };
            this.PossiblePoints = points.Select(p => new PointsViewModel(p)).ToList();
            this.SelectPointsCommand = new RelayCommand<PointsViewModel>((s) => navigationService.NavigateTo(ViewModelLocator.DisplayPoints, s));
        }

        public IList<PointsViewModel> PossiblePoints { get; }

        public RelayCommand<PointsViewModel> SelectPointsCommand { get; }
    }
}
