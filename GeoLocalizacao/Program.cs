using System;
using System.Device.Location;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace GeoLocalizacao
{
    class Program
    {
        static void Main(string[] args)
        {

            GeoCoordinateWatcher watcher;
            bool verf = !true; // false

            void GetLocationEvent()
            {

                watcher = new GeoCoordinateWatcher();                
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                bool started = watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));                

            }
            void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
            {
                if (verf != true) // false
                {
                    if (e.Position.Location.Latitude > 0 || e.Position.Location.Longitude > 0)
                    {
                        verf = !false; // true
                        mostarLocalizacao(e.Position.Location.Latitude, e.Position.Location.Longitude);
                    }

                }
            }

            void mostarLocalizacao(double Latitude, double Longitude)
            {

                RootObject rootObject = getAddress(Convert.ToString(Latitude), Convert.ToString(Longitude));

                Console.WriteLine("Latitude: {0}", rootObject.lat);
                Console.WriteLine("Longtude: {0}", rootObject.lon);
                Console.WriteLine("display_name: {0}", rootObject.display_name);
                Console.WriteLine("road: {0}", rootObject.address.road);
                Console.WriteLine("neighbourhood: {0}", rootObject.address.neighbourhood); 
                Console.WriteLine("suburb: {0}", rootObject.address.suburb);
                Console.WriteLine("city: {0}", rootObject.address.city);
                Console.WriteLine("county: {0}", rootObject.address.county);
                Console.WriteLine("state_district: {0}", rootObject.address.state_district);
                Console.WriteLine("state: {0}", rootObject.address.state);
                Console.WriteLine("postcode: {0}", rootObject.address.postcode);
                Console.WriteLine("country: {0}", rootObject.address.country);
                Console.WriteLine("country_code: {0}", rootObject.address.country_code);
                Console.WriteLine("place: {0}", rootObject.address.place);
                Console.WriteLine("house_number: {0}", rootObject.address.house_number);
                Console.WriteLine("railway: {0}", rootObject.address.railway);
                Console.WriteLine("city_district: {0}", rootObject.address.city_district);
            }
            GetLocationEvent();

            Console.ReadKey();
        }       

        public static RootObject getAddress(string latitude, string longitute)

        {
            latitude = latitude.Replace(',', '.'); longitute = longitute.Replace(',', '.');
            
            WebClient webClient = new WebClient();

            webClient.Headers.Add("user-agent", "Mozilla/4.0(compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            webClient.Headers.Add("Referer", "http://www.microsoft.com");

            var jsonData = webClient.DownloadData("http://nominatim.openstreetmap.org/reverse?format=json&lat=" + latitude + "&lon=" + longitute);

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(RootObject));

            RootObject rootObject = (RootObject)ser.ReadObject(new MemoryStream(jsonData));

            return rootObject;

        }
    }
}
