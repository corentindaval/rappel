using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;



namespace rappel.Platforms.Android
{
    [Service]
    public class MyForegroundService : Service
    {
        public const string BroadcastAction = "MauiBackgroundTask.Platforms.Android.MyForegroundService.DATA_BROADCAST";
        public const string DataKey = "data_key";
        private const string TAG = "MyForegroundService";
        private string targetLocation = string.Empty;
        private int delair = 5;
        Location loc = new Location
        {
            Longitude = 0,
            Latitude = 0
        };
        List<Location> listdom = new List<Location> { };

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Debug(TAG, "Service créé");
            // Créer une notification pour le service
            var notification = new Notification.Builder(this, "background_channel")
                .SetContentTitle("Service en cours")
                .SetContentText("Votre tâche en arrière-plan est en cours.")
                .Build();

            StartForeground(1, notification);
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent.Action == "STOP_SERVICE")
            {
                StopSelf();
            }
            var seploc = "/!/";
            var seplonlat = ",";
            targetLocation = intent.GetStringExtra("TargetLocation") ?? string.Empty;
           if (int.TryParse(intent.GetStringExtra("delai") ?? string.Empty, out int valeur))
            {
                delair = valeur;
            } 
           // delair = int.Parse(intent.GetStringExtra("delai") ?? string.Empty);
            
            
            if (targetLocation != null)
            {
                List<Location> nlistdom = new List<Location> { };
                if (targetLocation.Contains(seploc))
                {
                    string[] ldom = targetLocation.Split(seploc);
                    //  SendBroadcastMessage($"Position atteinte : {ldom[1]}");
                    foreach (var dom in ldom)
                    {
                        string[] dnvdom = dom.Split(seplonlat);
                        var nvdom = new Location
                        {
                            Latitude = double.Parse(dnvdom[0]),
                            Longitude = double.Parse(dnvdom[1])
                        };

                        nlistdom.Add(nvdom);
                    }
                }
                else
                {
                    if (targetLocation.Contains(seplonlat))
                    {
                        string[] dnvdom = targetLocation.Split(seplonlat);
                        var nvdom = new Location
                        {
                            Latitude = double.Parse(dnvdom[0]),
                            Longitude = double.Parse(dnvdom[1])
                        };
                        nlistdom.Add(nvdom);
                    }
                }
                listdom = nlistdom;
            }
            Log.Debug(TAG, $"Donnée reçue : {targetLocation}");
            // Logique de la tâche en arrière-plan
            _ = Task.Run(() =>
            {
                while (true)
                {
                    // Simuler une comparaison de géolocalisation
                    GetCurrentLocation();
                    string currentLocation = Math.Round(loc.Latitude,4).ToString() + "," + Math.Round(loc.Longitude,4).ToString();
                    //  SendBroadcastMessage($"Position atteinte : {currentLocation}");
                    if (listdom != null)
                    {
                        bool adom = false;
                        foreach (var dom in listdom)
                        {
                            if (loc.Latitude == dom.Latitude && loc.Longitude == dom.Longitude)
                            {
                                SendBroadcastMessage($"{currentLocation}");
                                adom = true;
                                break;
                            }
                        }
                        if (adom == false)
                        {
                            SendBroadcastMessage($"{"horsdom"}");
                        }
                    }
                    Thread.Sleep(delair * 1000); // Pause pour économiser les ressources
                }
              
            });

            return StartCommandResult.Sticky;
        }

        private async void GetCurrentLocation()
        {
            var nvloc = await Geolocation.GetLocationAsync();
            loc.Longitude = Math.Round(nvloc.Longitude,4);
            loc.Latitude = Math.Round(nvloc.Latitude,4);
        }
       
        private void SendBroadcastMessage(string message)
        {
            var intent = new Intent("com.maui.backgroundtask.RESPONSE");
            intent.PutExtra("Data", message);
            SendBroadcast(intent);
        }

        private void SendDataToMainPage(string data)
        {
            var intent = new Intent(BroadcastAction);
            intent.PutExtra(DataKey, data);
            SendBroadcast(intent);
        }

        public override IBinder OnBind(Intent intent) => null;


        public override void OnDestroy()
        {
            base.OnDestroy();
            StopForeground(true);
        }
    }
}
