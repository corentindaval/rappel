using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using rappel.Platforms.Android;
using Resource = rappel.Platforms.Android;

namespace rappel
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    "background_channel",
                    "Tâches en arrière-plan",
                    NotificationImportance.Default);

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            // Enregistrer le BroadcastReceiver
            var receiver = new MyBroadcastReceiver();
            var filter = new IntentFilter(MyForegroundService.BroadcastAction);
            RegisterReceiver(receiver, filter);
        }
        public void ShowNotification(string title, string message)
        {
            var builder = new NotificationCompat.Builder(this, "my_channel_id")
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.notificonerappel) // icône obligatoire
                .SetPriority((int)NotificationPriority.Default);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(1000, builder.Build());
        }
    }
}
