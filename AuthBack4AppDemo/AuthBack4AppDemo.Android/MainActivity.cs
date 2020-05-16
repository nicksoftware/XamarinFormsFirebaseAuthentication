using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase.Firestore;
using Firebase;
using Firebase.Auth;

namespace AuthBack4AppDemo.Droid
{
    [Activity(Label = "AuthBack4AppDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static FirebaseApp app;
         public static FirebaseAuth Auth;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            InitFirebaseAuth();
            LoadApplication(new App());

        }
        private void InitFirebaseAuth()
        {
            var options = new FirebaseOptions.Builder()
                .SetProjectId("xxxxx").SetApplicationId("xxxxx")
                .SetApiKey("xxxx")
                .SetDatabaseUrl("xxxxxxx")
                .SetStorageBucket("xxxxxxxx").Build();
            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);
            Auth = FirebaseAuth.GetInstance(app);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}