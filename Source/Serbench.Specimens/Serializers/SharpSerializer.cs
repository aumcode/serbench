using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NFX;
using NFX.Environment;

using Polenter.Serialization;


namespace Serbench.StockSerializers
{
    /// <summary>
    ///     Represents SharpSerializer:
    /// See here: http://www.sharpserializer.com/en/index.html  
    /// Add: PM> Install-Package SharpSerializer
    /// </summary>
    public class SharpSerializer : Serializer
    {

        private Polenter.Serialization.SharpSerializer m_Serializer;

        public SharpSerializer(TestingSystem context, IConfigSectionNode conf)
            : base(context, conf)
        {
            var settings = new Polenter.Serialization.SharpSerializerBinarySettings
              {
                  Mode = BinarySerializationMode.Burst
              };
            m_Serializer = new Polenter.Serialization.SharpSerializer(settings);   
        }

        public override void Serialize(object root, Stream stream)
        {
            m_Serializer.Serialize(root, stream);
        }

        public override object Deserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }

        public override void ParallelSerialize(object root, Stream stream)
        {
            m_Serializer.Serialize(root, stream);
        }

        public override object ParallelDeserialize(Stream stream)
        {
            return m_Serializer.Deserialize(stream);
        }
    }
}