//using System;
//using System.IO;
//using System.Linq;
//using Jil;
//using NFX;
//using NFX.Environment;

//namespace Serbench.Specimens.Serializers
//{
//    /// <summary>
//    ///     Represents Newtonsoft's JsonSerializer:
//    /// See here https://github.com/kevin-montrose/Jil
//    /// >PM Install-Package Jil 
//    /// </summary>
//    public class JilSerializer : Serializer
//    {
//        private readonly JilSerializer m_Serializer;

//        public JilSerializer(TestingSystem context, IConfigSectionNode conf)
//            : base(context, conf)
//        {
//        }

//        public override void Serialize(object root, Stream stream)
//        {
//            using (var sw = new StreamWriter(stream))
//            {
//                JSON.Serialize(root, sw,
//                    new Options(
//                        unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC));
//            }

//        }

//        public override object Deserialize(Stream stream)
//        {
//            using (var sr = new StreamReader(stream))
//            {
//                return JSON.Deserialize(sr,
//                    new Options(
//                        unspecifiedDateTimeKindBehavior: UnspecifiedDateTimeKindBehavior.IsUTC));
//            }
//        }

//        public override void ParallelSerialize(object root, Stream stream)
//        {
//            using (var sw = new StreamWriter(stream))
//            using (var jw = new JsonTextWriter(sw))
//            {
//                m_Serializer.Serialize(jw, root);
//            }
//        }

//        public override object ParallelDeserialize(Stream stream)
//        {
//            using (var sr = new StreamReader(stream))
//            using (var jr = new JsonTextReader(sr))
//            {
//                return m_Serializer.Deserialize(jr);
//            }
//        }
//    }
//}