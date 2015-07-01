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
    private const string SCRIPTS_DIR = "scripts";
    private const string STYLES_DIR = "styles";
    private const string WEB_DIR = "web";

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
      var targetDir = Path.Combine(rootPath, WEB_DIR);
      var scriptsDir = Path.Combine(targetDir, SCRIPTS_DIR);
      var stylesDir = Path.Combine(targetDir, STYLES_DIR);
      Directory.CreateDirectory(scriptsDir);
      Directory.CreateDirectory(stylesDir);

      return targetDir;
    }

    protected virtual void DoAddResources(string path)
    {
      AddStockScriptResource(path, SCRIPTS_DIR, "jquery-1.11.0.min.js");
      AddStockScriptResource(path, SCRIPTS_DIR, "wv.js");
      AddStockScriptResource(path, SCRIPTS_DIR, "wv.gui.js");
      AddStockScriptResource(path, SCRIPTS_DIR, "wv.chart.svg.js");
      AddResourceFile(path, SCRIPTS_DIR, "chart.js", @"scripts.chart.js");
      AddResourceFile(path, SCRIPTS_DIR, "serbench.js", @"scripts.serbench.js");

      AddResourceFile(path, STYLES_DIR, "default.css", @"styles.default.css");
      AddResourceFile(path, STYLES_DIR, "table.css", @"styles.table.css");
      AddResourceFile(path, STYLES_DIR, "overview-table.css", @"styles.overview-table.css");
      AddResourceFile(path, STYLES_DIR, "overview-charts.css", @"styles.overview-charts.css");
    }

    protected virtual void DoGeneratePages(string path)
    {
      var target = new NFX.Templatization.StringRenderingTarget(false);
      new OverviewTable().Render(target, null);
      File.WriteAllText(Path.Combine(path, "overview-table.htm"), target.Value);

      target = new NFX.Templatization.StringRenderingTarget(false);
      new OverviewCharts().Render(target, null);
      File.WriteAllText(Path.Combine(path, "overview-charts.htm"), target.Value);
    }

    
    // Copies a named stock script into the output path.
    // 20150701 DKh: This was taking it from WAVE before, moved to local files, decided to keep stock resource in separate method
    protected void AddStockScriptResource(string targetDir, string subDir, string scriptName)
    {
      var destinationName = Path.Combine(targetDir, subDir, scriptName);
      File.WriteAllText(destinationName, typeof(DefaultWebPackager).GetText("scripts." + scriptName));
    }

    protected void AddResourceFile(string targetDir, string subDir, string fileName, string resourceName)
    {
      var destinationName = Path.Combine(targetDir, subDir, fileName);
      File.WriteAllText(destinationName, typeof(DefaultWebPackager).GetText(resourceName));
    }

  }
}
