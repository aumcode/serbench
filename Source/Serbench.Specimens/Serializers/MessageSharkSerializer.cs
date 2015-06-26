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
    [SerializerInfo(
   Family = SerializerFamily.Binary,
   MetadataRequirement = MetadataRequirement.None,
   VendorName = "TJ Bakre",
   VendorLicense = "The GNU Library General Public License (LGPL)",
   VendorURL = "https://github.com/rpgmaker/MessageShark",
   VendorPackageAddress = "Install-Package MessageShark",
   FormatName = "MessageShark",
   LinesOfCodeK = 0,
   DataTypes = 0,
   Assemblies = 1,
   ExternalReferences = 0,
   PackageSizeKb = 49
)]
    public class MessageSharkSerializer : Serializer
    {

        private Type m_primaryType;

        public MessageSharkSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
        }

        public override void BeforeRuns(Test test)
        {
            m_RootType = test.GetPayloadRootType();
        }


        private Type m_RootType;

        public override void Serialize(object root, Stream stream)
        {
            var buf = MessageShark.MessageSharkSerializer.Serialize(root);
            stream.Write(buf, 0, buf.Length);
            //using (var sw = new StreamWriter(stream))
            //{
            //    sw.Write(MessageShark.MessageSharkSerializer.Serialize(root));
            //}
        }

        public override object Deserialize(Stream stream)
        {
            //using (MemoryStream ms = new MemoryStream())
            //{
            //     stream.CopyTo(ms);
            //    return MessageShark.MessageSharkSerializer.Deserialize<object>(ms.ToArray());
            //}
            using (var sr = new StreamReader(stream))
            {
                var temp = sr.ReadToEnd();
                byte[] bytes = new byte[temp.Length * sizeof(char)];
                System.Buffer.BlockCopy(temp.ToCharArray(), 0, bytes, 0, bytes.Length);
                return MessageShark.MessageSharkSerializer.Deserialize<object>(bytes);
            }
            // return null;   // TODO: How to call  Deserialize<T>(ms.ToArray()) with  m_primaryType ?
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(MessageShark.MessageSharkSerializer.Serialize(root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                var temp = sr.ReadToEnd();
                byte[] bytes = new byte[temp.Length * sizeof(char)];
                System.Buffer.BlockCopy(temp.ToCharArray(), 0, bytes, 0, bytes.Length);
                return MessageShark.MessageSharkSerializer.Deserialize<object>(bytes);
            }
        }
    }
}