using System;
using System.Runtime.Serialization;

namespace JosephGuadagno.Utilities.VirtualEarth
{
    [Serializable]
    [DataContract]
    public class PushPin
    {
        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public string Label { get; set; }
    }
}