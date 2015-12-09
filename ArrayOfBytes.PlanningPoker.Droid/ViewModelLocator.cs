namespace ArrayOfBytes.PlanningPoker.Droid
{
    using Microsoft.Practices.ServiceLocation;
    using GalaSoft.MvvmLight.Ioc;
    using ViewModels;
    using GalaSoft.MvvmLight.Views;

    // http://stackoverflow.com/questions/24073471/how-to-inject-service-to-view-model-in-mvvm-light
    public class ViewModelLocator
    {
        public static readonly string DisplayPoints = "Display";

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // https://marcominerva.wordpress.com/2014/10/10/navigationservice-in-mvvm-light-v5/
            var navigationService = new NavigationService();
            navigationService.Configure(DisplayPoints, typeof(DisplayPointsActivity));
            SimpleIoc.Default.Register<INavigationService>(() => navigationService);

            SimpleIoc.Default.Register<SelectPointsViewModel>();
        }

        public static SelectPointsViewModel SelectPoints
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SelectPointsViewModel>();
            }
        }

        public static INavigationService NavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }
    }
}