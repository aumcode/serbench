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
      Directory.CreateDirectory(targetDir);
      return targetDir;
    }

    protected virtual void DoAddResources(string path)
    {
      AddStockScriptResource(path, "jquery-1.11.0.min.js");
      AddStockScriptResource(path, "wv.js");
      AddStockScriptResource(path, "wv.gui.js");
      AddStockScriptResource(path, "wv.chart.svg.js");

      AddResourceFile(Path.Combine(path, "default.css"), @"styles.default.css");
      AddResourceFile(Path.Combine(path, "chart.css"), @"styles.chart.css");
      AddResourceFile(Path.Combine(path, "overview-table.css"), @"styles.overview-table.css");
      AddResourceFile(Path.Combine(path, "chart.js"), @"scripts.chart.js");
      AddResourceFile(Path.Combine(path, "serbench.js"), @"scripts.serbench.js");
    }

    protected virtual void DoGeneratePages(string path)
    {
      var target = new NFX.Templatization.StringRenderingTarget(false);
      new Index().Render(target, null);
      File.WriteAllText(Path.Combine(path, "index.htm"), target.Value);

      target = new NFX.Templatization.StringRenderingTarget(false);
      new OverviewReport().Render(target, null);
      File.WriteAllText(Path.Combine(path, "overviewreport.htm"), target.Value);
    }

    /// <summary>
    /// Copies a named stock script into the output path
    /// </summary>
    protected void AddStockScriptResource(string path, string scriptName)
    {
      File.WriteAllText(Path.Combine(path, scriptName),
                         typeof(NFX.Wave.Templatization.WaveTemplate).
                         GetText("StockContent.Embedded.script." + scriptName));
    }

    protected void AddResourceFile(string destinationName, string resourceName)
    {
      File.WriteAllText(destinationName, typeof(DefaultWebPackager).GetText(resourceName));
    }

  }
}
