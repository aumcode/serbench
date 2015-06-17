using System;
using System.IO;
using System.Linq;
using NFX;
using NFX.Environment;

using System.Web.Script.Serialization;

namespace Serbench.StockSerializers
{
    /// <summary>
    ///     Represents fastJSON:
    /// See here https://msdn.microsoft.com/en-us/library/system.web.script.serialization.javascriptserializer%28v=vs.110%29.aspx
    /// Add: reference to System.Web.Extensions.dll
    /// Add: using System.Web.Script.Serialization;
    /// </summary>
    public class MSJavaScriptSerializer : Serializer
    {
        private readonly JavaScriptSerializer m_Serializer = new JavaScriptSerializer();
        private Type[] m_KnownTypes;
        private Type m_primaryType;

        public MSJavaScriptSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            m_KnownTypes = ReadKnownTypes(conf);
        }

        public override void BeforeRuns(Test test)
        {
            var m_primaryType = test.GetPayloadRootType(); 
        }

        public override void Serialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_Serializer.Serialize(root));
            }
        }

        public override object Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(sr.ReadToEnd(), m_primaryType);
            }
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(m_Serializer.Serialize(root));
            }
        }

        public override object ParallelDeserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return m_Serializer.Deserialize(sr.ReadToEnd(), m_primaryType);
            }
        }
    }
}