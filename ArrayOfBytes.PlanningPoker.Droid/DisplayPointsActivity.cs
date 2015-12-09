namespace ArrayOfBytes.PlanningPoker.Droid
{
    using Android.App;
    using Android.OS;
    using Android.Widget;
    using GalaSoft.MvvmLight.Views;
    using ViewModels;

    [Activity(Label = "DisplayPointsActivity")]
    public class DisplayPointsActivity : ActivityBase
    {
        private PointsViewModel viewmodel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DisplayPoints);

            TextView textView = FindViewById<TextView>(Resource.Id.PointsTextView);

            // http://www.mvvmlight.net/doc/nav4.cshtml
            var nav = ((NavigationService)ViewModelLocator.NavigationService);
            this.viewmodel = nav.GetAndRemoveParameter<PointsViewModel>(this.Intent);

            textView.Text = this.viewmodel.Value.ToString();
            textView.SetTextSize(Android.Util.ComplexUnitType.Dip, 200);

            textView.Click += (sender, e) => nav.GoBack();

            var toast = Toast.MakeText(this, Resource.String.PressToReturn, ToastLength.Long);
            toast.Show();
        }
    }
}