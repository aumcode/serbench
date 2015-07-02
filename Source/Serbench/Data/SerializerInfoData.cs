using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.DataAccess.CRUD;

namespace Serbench.Data
{

  /// <summary>
  /// Represents data gathered during test runs.
  /// This data is saved into App.DataStore
  /// </summary>
  public class SerializerInfoData : TypedRow
  {
      public SerializerInfoData() {}

      /// <summary>
      /// Inits the data from attribute if one is set, or allocates empty instance if there is not attr
      /// </summary>
      public SerializerInfoData(Serializer ser) 
      {
        var t = ser.GetType();
        var attr = t.GetCustomAttributes(typeof(SerializerInfo), false).FirstOrDefault() as SerializerInfo;
        if (attr==null) return;

        SerializerType = ser.GetType().FullName;
        SerializerName = ser.Name;

        foreach(var pi in attr.GetType().GetProperties(System.Reflection.BindingFlags.DeclaredOnly | 
                                                       System.Reflection.BindingFlags.Instance |
                                                       System.Reflection.BindingFlags.Public))
         this[pi.Name] = pi.GetValue(attr);
      }
      

      [Field]
      public string SerializerType {get; set;}

      [Field]
      public string SerializerName {get; set;}


      /// <summary>
      /// I.e. Bin vs Text
      /// </summary>
      [Field]
      public SerializerFamily Family{ get; set;}
      
      /// <summary>
      /// What kind of decoration is needed for source types
      /// </summary>
      [Field]
      public MetadataRequirement MetadataRequirement{ get; set;}


      /// <summary>
      /// Who did the serializer implementation
      /// </summary>
      [Field]
      public string VendorName{ get; set;}

      /// <summary>
      /// License name
      /// </summary>
      [Field]
      public string VendorLicense{ get; set;}


      /// <summary>
      /// Where to get the code/info
      /// </summary>
      [Field]
      public string VendorURL{ get; set;}


      /// <summary>
      /// Name of package, .i.e. NUGET name or name of dll or address of code base
      /// </summary>
      [Field] 
      public string VendorPackageAddress{ get; set;}

      
      /// <summary>
      /// Name of format, such as XML, JSON etc...
      /// </summary>
      [Field]
      public string FormatName{get; set;}
  
      
      /// <summary>
      /// Approximate number of lines of code in the implementation
      /// </summary>
      [Field] 
      public int LinesOfCodeK {get; set;}


      /// <summary>
      /// Approximate number of data structures in the implementation, such as classes, intfs and structs etc.
      /// </summary>
      [Field] 
      public int DataTypes {get; set;}


      /// <summary>
      /// How many assemblies/files the serializer is shipped in
      /// </summary>
      [Field] 
      public int Assemblies{ get; set;}

      /// <summary>
      /// Number of references to external assemblies which are not a part of the serialization technology
      /// and are not a part of .NET core framework (System.* namespaces)
      /// </summary>
      [Field]
      public int ExternalReferences { get; set;}


      /// <summary>
      /// The size of all files which are needed to install serializer (Assemblies+ExtRefs+other files)
      /// </summary>
      [Field] 
      public int PackageSizeKb { get; set;}

  }



}
