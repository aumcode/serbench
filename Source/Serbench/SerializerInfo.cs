using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Serbench
{

  public enum SerializerFamily
  {
     Binary,
     Textual,
     Hybrid
  }


  public enum MetadataRequirement
  {
     /// <summary>
     /// Any type can be serialized without the need for explicit decoration
     /// </summary>
     None,

     /// <summary>
     /// The type must be decorated with attributes in code
     /// </summary>
     Attributes,

     /// <summary>
     /// The type must be described in the IDL-like language (i.e. Thrift/Protobuf/BOND etc.)
     /// </summary>
     IDL
  }




  [AttributeUsage(AttributeTargets.Class)]
  public class SerializerInfo : Attribute
  {
      /// <summary>
      /// I.e. Bin vs Text
      /// </summary>
      public SerializerFamily Family{ get; set;}
      
      /// <summary>
      /// What kind of decoration is needed for source types
      /// </summary>
      public MetadataRequirement MetadataRequirement{ get; set;}


      /// <summary>
      /// Who did the serializer implementation
      /// </summary>
      public string VendorName{ get; set;}

      /// <summary>
      /// License name
      /// </summary>
      public string VendorLicense{ get; set;}


      /// <summary>
      /// Where to get the code/info
      /// </summary>
      public string VendorURL{ get; set;}


      /// <summary>
      /// Name of package, .i.e. NUGET name
      /// </summary>
      public string VendorPackageURL{ get; set;}

      
      /// <summary>
      /// Name of format, such as XML, JSON etc...
      /// </summary>
      public string FormatName{get; set;}
  
      
      /// <summary>
      /// Approximate number of lines of code in the implementation
      /// </summary>
      public int LinesOfCodeK {get; set;}


      /// <summary>
      /// Approximate number of data structures in the implementation, such as classes, intfs and structs etc.
      /// </summary>
      public int DataTypes {get; set;}


      /// <summary>
      /// How many assemblies/files the serializer is shipped in
      /// </summary>
      public int Assemblies{ get; set;}

      /// <summary>
      /// Number of references to external assemblies which are not a part of the serialization technology
      /// and are not a part of .NET core framework (System.* namespaces)
      /// </summary>
      public int ExternalReferences { get; set;}


      /// <summary>
      /// The size of all files which are needed to install serializer (Assemblies+ExtRefs+other files)
      /// </summary>
      public int PackageSizeKb { get; set;}
  }



}
