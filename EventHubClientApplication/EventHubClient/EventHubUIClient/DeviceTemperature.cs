using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace EventHubUIClient
{
    [DataContract]
    public class DeviceTemperature
    {
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string DeviceDateTime { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Lattitude { get; set; }
        [DataMember]
        public string Temperature { get; set; }
    }
}
