# SERBENCH - Serializer Benchmarking Suite
The purpose of this tool is to test the performance/capabilities of various
serializers available for the CLR/.NET platform.

Both tests and serializers are descendants of corresponding base classes which can
be injected via a named config into the tool. This approach allows for high degree of flexibility
as everything is configurable.

Every test determines what payload kind/type, pattern and size is used against every serializer, so
the tool permutes all test instances and serializer isnatnces to test different combinations.

Not all serializers support all features, i.e. ProtoBuf does not support cyclical object graphs. This
 is normal and actually is expected to be discovered by this tool.
 
The tool relies on the NFX library, hence it does not have much code in it: 
https://github.com/aumcode/nfx


