namespace ArrayOfBytes.PlanningPoker.Droid
{
    using Android.App;
    using Android.Widget;
    using Android.OS;
    using System.Linq;
    using ViewModels;
    using GalaSoft.MvvmLight.Helpers;
    using Android.Views;
    using GalaSoft.MvvmLight.Views;

    [Activity(Label = "Planning Poker Cards", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ActivityBase
    {
        private SelectPointsViewModel viewmodel;
        
        private ObservableAdapter<PointsViewModel> adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            this.viewmodel = ViewModelLocator.SelectPoints;

            // http://blog.galasoft.ch/posts/2014/05/preview-an-observableadaptert-for-xamarin-android-in-mvvm-light-vnext/
            ListView listview = FindViewById<ListView>(Resource.Id.SelectPointsList);
            this.adapter = this.viewmodel.PossiblePoints.GetAdapter(this.UpdateTemplate);
            listview.Adapter = this.adapter;
            listview.ItemClick += (sender, e) => this.viewmodel.SelectPointsCommand.Execute(this.adapter[e.Position]);
        }
        
        private View UpdateTemplate(int position, PointsViewModel viewModel, View convertView)
        {
            if (convertView == null)
            {
                // Use a built in list item template.
                convertView = LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }

            var text = convertView.FindViewById<TextView>(Android.Resource.Id.Text1);
            text.Text = viewModel.Value.ToString();
            return convertView;
        }
    }
}

