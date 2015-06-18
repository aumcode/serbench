using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using MessageShark;

namespace Serbench.Specimens.Serializers
{
    /// <summary>
    ///     Represents MessageShark by TJ Bakre:
    /// See here https://github.com/rpgmaker/MessageShark
    /// >PM Install-Package MessageShark 
    /// </summary>
    public class MessageSharkSerializer : Serializer
    {

        private Type m_primaryType;

        public MessageSharkSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

        public override void BeforeRuns(Test test)
        {
            m_primaryType = test.GetPayloadRootType();
        }

        public override void Serialize(object root, Stream stream)
        { 
            var buf = MessageShark.MessageSharkSerializer.Serialize(root);
            stream.Write(buf, 0, buf.Length);
        }

        public override object Deserialize(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                 stream.CopyTo(ms);
                return MessageShark.MessageSharkSerializer.Deserialize<object>(ms.ToArray());
            }
           // return null;   // TODO: How to call  Deserialize<T>(ms.ToArray()) with  m_primaryType ?
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            var buf = MessageShark.MessageSharkSerializer.Serialize(root);
            //stream.Write(buf, 0, buf.Length);
        }

        public override object ParallelDeserialize(Stream stream)
        {
          using (MemoryStream ms = new MemoryStream())
            {
                 stream.CopyTo(ms);
                return MessageShark.MessageSharkSerializer.Deserialize<object>(ms.ToArray());
            }
           // return null;   // TODO: How to call  Deserialize<T>(ms.ToArray()) with  m_primaryType ?
       }
    }
}