namespace ArrayOfBytes.PlanningPoker.Droid
{
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Views;
    using Android.Widget;
    using GalaSoft.MvvmLight.Views;
    using ViewModels;
    using Android.Runtime;
    using System.IO;
    using Android.Graphics;

    [Activity(Label = "Points", Theme = "@android:style/Theme.Holo.Light")]
    public class DisplayPointsActivity : ActivityBase
    {
        private const int SELECT_PHOTO = 1;

        private PointsViewModel viewmodel;

        private Toast toast;

        private TextView textView;

        private ImageView imageView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DisplayPoints);

            this.textView = FindViewById<TextView>(Resource.Id.PointsTextView);
            this.imageView = FindViewById<ImageView>(Resource.Id.PointsImageView);

            // http://www.mvvmlight.net/doc/nav4.cshtml
            var nav = ((NavigationService)ViewModelLocator.NavigationService);
            this.viewmodel = nav.GetAndRemoveParameter<PointsViewModel>(this.Intent);

            this.textView.Text = this.viewmodel.Value.ToString();
            this.textView.SetTextSize(Android.Util.ComplexUnitType.Dip, 200);

            this.textView.Click += (sender, e) => nav.GoBack();
            this.imageView.Click += (sender, e) => nav.GoBack();

            this.TryOpenImage();

            this.toast = Toast.MakeText(this, Resource.String.PressToReturn, ToastLength.Long);
            toast.Show();
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (this.toast != null)
            {
                this.toast.Cancel();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            // http://developer.android.com/training/appbar/actions.html#add-actions
            this.MenuInflater.Inflate(Resource.Menu.DisplayPointsMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_select_image:
                    // http://stackoverflow.com/a/5086706/5608540
                    Intent photoPicker = new Intent(Intent.ActionPick);
                    photoPicker.SetType("image/*");
                    this.StartActivityForResult(photoPicker, SELECT_PHOTO);
                    return true;
                case Resource.Id.action_clear_image:
                    this.DeleteFile(this.viewmodel.Value.ToString());
                    this.TryOpenImage();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            switch (requestCode)
            {
                case SELECT_PHOTO:
                    if (resultCode != Result.Ok)
                    {
                        return;
                    }

                    Android.Net.Uri selectedImage = data.Data;
                    Stream imageStream = this.ContentResolver.OpenInputStream(selectedImage);
                    var outputFile = this.OpenFileOutput(this.viewmodel.Value.ToString(), FileCreationMode.Private);
                    imageStream.CopyTo(outputFile);
                    this.TryOpenImage();
                    break;
            }
        }

        private void TryOpenImage()
        {
            var filename = new Java.IO.File(this.FilesDir, this.viewmodel.Value.ToString());
            if (!filename.Exists())
            {
                this.textView.Visibility = ViewStates.Visible;
                this.imageView.Visibility = ViewStates.Gone;
                return;
            }

            this.textView.Visibility = ViewStates.Gone;
            this.imageView.Visibility = ViewStates.Visible;
            
            // http://stackoverflow.com/a/28351881/5608540
            var bitmap = BitmapFactory.DecodeFile(filename.AbsolutePath, new BitmapFactory.Options());
            this.imageView.SetImageBitmap(bitmap);
        }
    }
}