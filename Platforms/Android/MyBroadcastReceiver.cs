using Android.Content;

namespace rappel.Platforms.Android
{
    public class MyBroadcastReceiver : BroadcastReceiver
    {
        public static event Action<string> OnDataReceived;

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent?.Action == "com.maui.backgroundtask.RESPONSE")
            {
                var data = intent.GetStringExtra("Data");
                if (!string.IsNullOrEmpty(data))
                {
                    // Déclencher l'événement pour notifier la MainPage
                    OnDataReceived?.Invoke(data);
                }
            }

            /*  if (intent.Action == MyForegroundService.BroadcastAction)
              {
                  var data = intent.GetStringExtra(MyForegroundService.DataKey);
                  OnDataReceived?.Invoke(data);
              }*/
        }
    }
}