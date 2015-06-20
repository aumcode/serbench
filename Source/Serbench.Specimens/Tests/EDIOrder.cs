using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Serbench.Specimens.Tests
{
    [ProtoContract]
    [DataContract]
    [Serializable]
    public class EDIOrder
    {
        [ProtoMember(1)]
        [DataMember]
        public Header Header;
        [ProtoMember(2)]
        [DataMember]
        public CustomerDetails CustomerDetails;
        [ProtoMember(3)]
        [DataMember]
        public List<OrderItem> OrderItems;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class Header
    {
        [ProtoMember(1)]
        [DataMember]
        public int OrderId;
        [ProtoMember(2)]
        [DataMember]
        public int StatusCode;
        [ProtoMember(3)]
        [DataMember]
        public decimal NetAmount;
        [ProtoMember(4)]
        [DataMember]
        public double TotalAmount;
        [ProtoMember(5)]
        [DataMember]
        public double Tax;
        [ProtoMember(6)]
        [DataMember]
        public DateTime Date;
    }

    [ProtoContract]
    [DataContract]
    [Serializable]
    public class CustomerDetails
    {
        [ProtoMember(1)]
        [DataMember]
        public string UserName;
        [ProtoMember(2)]
        [DataMember]
        public string FirstName;
        [ProtoMember(3)]
        [DataMember]
        public string LastName;
        [ProtoMember(4)]
        [DataMember]
        public string AddressLine;
        [ProtoMember(5)]
        [DataMember]
        public string City;
        [ProtoMember(5)]
        [DataMember]
        public string State;
        [ProtoMember(6)]
        [DataMember]
        public string Zip;
    }

    [ProtoContract]
    [DataContract]
    [CollectionDataContract]
    [Serializable]
    public class OrderItem
    {
        [ProtoMember(1)]
        [DataMember]
        public int Position;
        [ProtoMember(2)]
        [DataMember]
        public int Quantity;
        [ProtoMember(3)]
        [DataMember]
        public int ProductId;
        [ProtoMember(4)]
        [DataMember]
        public string Title;
        [ProtoMember(5)]
        [DataMember]
        public double Price;
    }
}
