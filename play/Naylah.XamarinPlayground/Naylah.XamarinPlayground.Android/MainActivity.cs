﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Naylah.App.Xamarin.Android.Services;

namespace Naylah.XamarinPlayground.Droid
{
    [Activity(
        Label = "Naylah.XamarinPlayground",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)
        ]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            HapticFeedbackService.Init(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            //var app = new NyApplicationBuilder<App>()
            //    //.UseServiceProviderFactory()
            //    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            //    .ConfigureContainer<ContainerBuilder>((appctx, builder) =>
            //    {
            //        builder.Register<string>(x => "teste");
            //        // registering services in the Autofac ContainerBuilder
            //    })
            //    .Build();

            //var a = app.Services.GetService(typeof(string));

            //app.Run(this);

            LoadApplication(new App()); //Dont need cause app.Run(this); already do this
        }
    }
}