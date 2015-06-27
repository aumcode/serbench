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
    private const string c_ScriptsFolder = "scripts";
    private const string c_StylesFolder = "styles";
    private const string c_WebFolder = "web";

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
      var targetDir = Path.Combine(rootPath, c_WebFolder);
      var scriptsDir = Path.Combine(targetDir, c_ScriptsFolder);
      var stylesDir = Path.Combine(targetDir, c_StylesFolder);
      Directory.CreateDirectory(scriptsDir);
      Directory.CreateDirectory(stylesDir);

      return targetDir;
    }

    protected virtual void DoAddResources(string path)
    {
      AddStockScriptResource(path, c_ScriptsFolder, "jquery-1.11.0.min.js");
      AddStockScriptResource(path, c_ScriptsFolder, "wv.js");
      AddStockScriptResource(path, c_ScriptsFolder, "wv.gui.js");
      AddStockScriptResource(path, c_ScriptsFolder, "wv.chart.svg.js");
      AddResourceFile(path, c_ScriptsFolder, "chart.js", @"scripts.chart.js");
      AddResourceFile(path, c_ScriptsFolder, "serbench.js", @"scripts.serbench.js");

      AddResourceFile(path, c_StylesFolder, "default.css", @"styles.default.css");
      AddResourceFile(path, c_StylesFolder, "chart.css", @"styles.chart.css");
      AddResourceFile(path, c_StylesFolder, "overview-table.css", @"styles.overview-table.css");
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
    protected void AddStockScriptResource(string targetDir, string subDir, string scriptName)
    {
      File.WriteAllText(Path.Combine(targetDir, subDir,  scriptName),
                         typeof(NFX.Wave.Templatization.WaveTemplate).
                         GetText("StockContent.Embedded.script." + scriptName));
    }

    protected void AddResourceFile(string targetDir, string subDir, string fileName, string resourceName)
    {
      var destinationName = Path.Combine(targetDir, subDir, fileName);
      File.WriteAllText(destinationName, typeof(DefaultWebPackager).GetText(resourceName));
    }

  }
}
