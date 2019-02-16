using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace EventHubUIClient
{
    public partial class Form1 : Form
    {
        static string eventHubName = "tempevent";
        static string connectionString = "Endpoint=sb://weatherevents.servicebus.windows.net/;SharedAccessKeyName=mypolicyhub;SharedAccessKey=JH1kVmfKs3L1s2OTj4r12yAShwBxKOAKh7u06UqK78s=";
        //
        //
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendingRandomMessages();
        }

        static void SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            while (true)
            {


                var from = DateTime.Now;
                Random r = new Random();
                // const int numberOfEvents = 6;

                Dictionary<string, DeviceTemperature> devices = new Dictionary<string, DeviceTemperature>();
                devices.Add("Device1", new DeviceTemperature
                {
                    DeviceId = "Device1",
                    LocationName = "Ghazipur",
                    Longitude = "25.5840° N",
                    Lattitude = "83.5770° E"
                });
                devices.Add("Device2", new DeviceTemperature
                {
                    DeviceId = "Device2",
                    LocationName = "Varanasi",
                    Longitude = "25.3176° N",
                    Lattitude = "82.9739° E"
                });

                devices.Add("Device3", new DeviceTemperature
                {
                    DeviceId = "Device3",
                    LocationName = "Lucknow",
                    Longitude = "26.8467° N",
                    Lattitude = "80.9462° E"
                });

                devices.Add("Device4", new DeviceTemperature
                {
                    DeviceId = "Device4",
                    LocationName = "Agra",
                    Longitude = "27.1767° N",
                    Lattitude = "78.0081° E"
                });
                devices.Add("Device5", new DeviceTemperature
                {
                    DeviceId = "Device5",
                    LocationName = "Delhi",
                    Longitude = "28.7041° N",
                    Lattitude = "77.1025° E"
                });


                foreach (var device in devices)
                {
                    device.Value.Temperature = r.Next(20, 40).ToString();
                    device.Value.DeviceDateTime = DateTime.Now.ToString();
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(DeviceTemperature));
                    MemoryStream msObj = new MemoryStream();
                    js.WriteObject(msObj, device.Value);
                    msObj.Position = 0;


                    StreamReader sr = new StreamReader(msObj);
                    string json = sr.ReadToEnd();
                    sr.Close();
                    msObj.Close();
                    StringBuilder jsonAppent = new StringBuilder("[");
                    jsonAppent.Append(json);
                    jsonAppent.Append("]");


                    var ms = new MemoryStream();
                    
                    // write opening tag
                    byte[] finalJson = Encoding.UTF8.GetBytes(jsonAppent.ToString());
                    ms.Write(finalJson, 0, finalJson.Length);
                    ms.Position = 0;
                    //sr = new StreamReader(ms);
                    //json = sr.ReadToEnd();
                    //sr.Close();
                    // Send JSON to event hub.
                    //System.Threading.Thread.Sleep(1000);
                    EventData eventData = new EventData(ms);
                    eventHubClient.Send(eventData);
                    sr.Close();
                    ms.Close();



                }





                //JsonReaderWriterFactory.
                //var deviceIds = new[] { "device1", "device2", "device3" };

                //var events = new List<string>();
                //for (int i = 0; i < numberOfEvents; ++i)
                //{
                //    for (int device = 0; device < deviceIds.Length; ++device)
                //    {
                //        // Generate event and serialize as JSON object:
                //        // { "deviceTimestamp": "utc timestamp", "deviceId": "guid", "value": 123.456 }
                //        events.Add(
                //            String.Format(
                //                CultureInfo.InvariantCulture,
                //                @"{{ ""deviceTimestamp"": ""{0}"", ""deviceId"": ""{1}"", ""value"": {2} }}",
                //                (from + TimeSpan.FromSeconds(i * 30)).ToString("o"),
                //                deviceIds[device],
                //                r.NextDouble()));
                //    }
                //}


                //using (var ms = new MemoryStream())
                //using (var sw = new StreamWriter(ms))
                //{
                //    StringBuilder bldr = new StringBuilder("[");
                //    // Wrap events into JSON array:
                //    //sw.Write("[");
                //    for (int i = 0; i < events.Count; ++i)
                //    {
                //        if (i > 0)
                //        {
                //            bldr.Append(",");
                //            //sw.Write(',');
                //        }
                //        bldr.Append(events[i]);
                //    }
                //    bldr.Append("]");
                //    sw.Write(bldr.ToString());

                //    sw.Flush();
                //    ms.Position = 0;

                //    // Send JSON to event hub.
                //    //EventData eventData = new EventData(ms);
                //    //eventHubClient.Send(eventData);
                //}

                //var message = Guid.NewGuid().ToString() + "," + DateTime.Now.ToString() + ",30";
                //                Console.WriteLine(message);
                //                eventHubClient.Send(new EventData(message));
                //            }
                //            catch (Exception exception)
                //            {
                //                Console.ForegroundColor = ConsoleColor.Red;
                //                Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                //                Console.ResetColor();
                //            }

                //            Thread.Sleep(200);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
