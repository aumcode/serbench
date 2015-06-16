using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



using NFX;
using NFX.Environment;
using NFX.Serialization.Slim;

namespace Serbench.StockSerializers
{
  
  /// <summary>
  /// Represents NFX.Serialization.Slim technology
  /// </summary>
  public class NFXSlim : Serializer
  {

    public NFXSlim(TestingSystem context, IConfigSectionNode conf) : base(context, conf) 
    {
      Type[] known = ReadKnownTypes(conf); 

      //we create type registry with well-known types that serializer does not have to emit every time
      m_TypeRegistry = new TypeRegistry(TypeRegistry.BoxedCommonTypes,
                                        TypeRegistry.BoxedCommonNullableTypes, 
                                        TypeRegistry.CommonCollectionTypes,
                                        known);

      m_Serializer = new SlimSerializer(m_TypeRegistry);

      //batching allows to remember the encountered types and hence it is a "stateful" mode
      //where serialization part and deserialization part retain the type registries that 
      //get auto-updated. This mode is not thread safe
      if (m_Batching)
      {
        m_BatchSer = new SlimSerializer(m_TypeRegistry);
        m_BatchSer.TypeMode = TypeRegistryMode.Batch;

        m_BatchDeser = new SlimSerializer(m_TypeRegistry);
        m_BatchDeser.TypeMode = TypeRegistryMode.Batch;
      }
    }

    private TypeRegistry m_TypeRegistry;
    private SlimSerializer m_Serializer;
    private SlimSerializer m_BatchSer;
    private SlimSerializer m_BatchDeser;

    [Config]
    private bool m_Batching;

    public override void Serialize(object root, Stream stream)
    {
     if (m_Batching)
       m_BatchSer.Serialize(stream, root);
     else
       m_Serializer.Serialize(stream, root); 
    }

    public override object Deserialize(Stream stream)
    {
      if (m_Batching)
       return m_BatchDeser.Deserialize(stream);

      return m_Serializer.Deserialize(stream);
    }

    public override void BeforeSerializationIterationBatch(Test test)
    {
      if (m_Batching && 
          (test.SerIterations>1 || test.DeserIterations>1))
       throw new SerbenchException("SlimSerializer test is not properly configured. If BATCHING=true, then test may have many runs, not many ser/deser iterations as batching retains the stream state and is not an idempotent operation");
    }


    public override void ParallelSerialize(object root, Stream stream)
    {
      //parallel mode can not use batching because it is not thread-safe
      m_Serializer.Serialize(stream, root);
    }

    public override object ParallelDeserialize(Stream stream)
    {
      //parallel mode can not use batching because it is not thread-safe
      return m_Serializer.Deserialize(stream);
    }

  }

}
