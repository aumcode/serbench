using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private const string _defaultAppNAme = "Serbench";

        private const string _externalScriptsLocation = "StockContent.Embedded.script.";

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
            if (appName.IsNullOrWhiteSpace()) appName = _defaultAppNAme;

            //sanitize app name so it can be used for directory name
            appName = new String(appName.Select(c => !Char.IsLetterOrDigit(c) ? '_' : c).ToArray());

            var name = Path.Combine(rootPath, "{0}-{1:yyyyMMddHHmm}".Args(appName, App.LocalizedTime));

            var dname = name;
            for (var i = 0; Directory.Exists(dname); i++) dname = name + i.ToString();

            NFX.IOMiscUtils.EnsureAccessibleDirectory(dname);

            return dname;
        }

        protected virtual void DoAddResources(string path)
        {
            var externalResources = new List<string>
            {
                "jquery-1.11.0.min.js" ,
                "wv.js" ,
                "wv.gui.js",
                "wv.chart.svg.js"
            };
            externalResources.ForEach(r => AddStockScriptResource(path, r));

            var internalResources = new List<string>
            {
                "default.css" ,
                "chart.css" ,
                "overview-table.css",
                "chart.js",
                "overview-table.js"
            };
            internalResources.ForEach(r => AddFileResource(path, r));
        }

        protected virtual void DoGeneratePages(string path)
        {
            var target = new NFX.Templatization.StringRenderingTarget(false);
            new Index().Render(target, null);
            var indexPagePath = Path.Combine(path, "index.htm");
            File.WriteAllText(indexPagePath, target.Value);
        }

        /// <summary>
        /// Copies a named stock script into the output path
        /// </summary>
        protected void AddStockScriptResource(string path, string scriptName)
        {
            File.WriteAllText(Path.Combine(path, scriptName),
                typeof(NFX.Wave.Templatization.WaveTemplate).
                    GetText(_externalScriptsLocation + scriptName));
        }

        private void AddFileResource(string path, string fileName)
        {
            File.WriteAllText(Path.Combine(path, fileName), typeof(DefaultWebPackager).GetText(fileName));
        }
    }
}
