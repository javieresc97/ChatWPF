using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatBackend
{
    [ServiceContract]
    public interface IChatBackend
    {
        [OperationContract(IsOneWay = true)]
        void DisplayMessage(CompositeType composite);

        [OperationContract(IsOneWay = true)]
        void TransferFile(string filename, byte[] fileData);

        void SendMessage(string text);
        
    }

    [DataContract]
    public class CompositeType
    {
        public CompositeType() { }
        public CompositeType(string u, string m)
        {
            Username = u;
            Message = m;
        }

        public CompositeType(string fn, byte[] f)
        {
            Filename = fn;
            File = f;
        }

        [DataMember]
        public string Username { get; set; } = "Anonymous";

        [DataMember]
        public string Message { get; set; } = "";

        [DataMember]
        public string Filename { get; set; }

        [DataMember]
        public byte[] File { get; set; }
    }

    public delegate void DisplayMessageDelegate(CompositeType data);
}
