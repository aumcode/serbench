using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using NFX;
using NFX.Environment;

namespace Serbench.WebViewer
{
  /// <summary>
  /// Represents a base default implementation of packager that preps fiels for web viewer (bundles java script, css and other files)
  /// </summary>
  public class DefaultWebPackager
  {
     public DefaultWebPackager(Data.ITestDataStore data, IConfigSectionNode config)
     {

     }


     /// <summary>
     /// Builds a HTML web view package with all resources
     /// </summary>
     public string Build(string rootPath)
     {
        var dir = DoCreateFolder(rootPath);

        DoAddResources(dir);
        DoGeneratePages(dir);

        return dir;
     }


     protected virtual string DoCreateFolder(string rootPath)
     {
        var appName = App.Name;
        if (appName.IsNullOrWhiteSpace()) appName = "Serbench";

        //sanitize app name so it can be used for directory name
        appName = new String( appName.Select( c => !Char.IsLetterOrDigit(c) ? '_' : c).ToArray() ); 
        
        var name = Path.Combine(rootPath, "{0}-{1:yyyyMMddHHmm}".Args(appName, App.LocalizedTime));
        
        var dname = name;
        for(var i=0; Directory.Exists(dname); i++) dname = name + i.ToString();

        NFX.IOMiscUtils.EnsureAccessibleDirectory(dname);

        return dname;
     }

     protected virtual void DoAddResources(string path)
     {
        AddStockScriptResource(path, "jquery-1.11.0.min.js");
        AddStockScriptResource(path, "wv.js");
        AddStockScriptResource(path, "wv.gui.js");
        AddStockScriptResource(path, "wv.chart.svg.js");
       
        File.WriteAllText(Path.Combine(path, "default.css"), typeof(DefaultWebPackager).GetText("default.css"));
     }

     protected virtual void DoGeneratePages(string path)
     {

     }

     /// <summary>
     /// Copies a named stock script into the output path
     /// </summary>
     protected void AddStockScriptResource(string path, string scriptName)
     {
         File.WriteAllText(Path.Combine(path,scriptName), 
                            typeof(NFX.Wave.Templatization.WaveTemplate).
                            GetText("StockContent.Embedded.script."+scriptName));
     }



  }
}
