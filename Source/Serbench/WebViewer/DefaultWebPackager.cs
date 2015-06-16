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
        var dir = DoCreateTargetDir(rootPath);

        DoAddResources(dir);
        DoGeneratePages(dir);

        return dir;
     }

     /// <summary>
     /// Override to package web output into sub-folder
     /// </summary>
     protected virtual string DoCreateTargetDir(string rootPath)
     {
        var targetDir = Path.Combine(rootPath, "web");
        NFX.IOMiscUtils.EnsureAccessibleDirectory(targetDir);
        return targetDir;
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
        var target = new NFX.Templatization.StringRenderingTarget(false);
        new Index().Render(target, null);
        File.WriteAllText(Path.Combine(path, "index.htm"), target.Value); 
       

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
