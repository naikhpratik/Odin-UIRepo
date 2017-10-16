using System.IO;
using System.Web;
using System.Web.Mvc;

    public static class CssHelper
    {
        public static IHtmlString EmbedCss(this HtmlHelper htmlHelper, string path)
        {
            // take a path that starts with "~" and map it to the filesystem.
            var cssFilePath = HttpContext.Current.Server.MapPath(path);
            // load the contents of that file
            try
            {
                var cssText = File.ReadAllText(cssFilePath);
                return htmlHelper.Raw("<style>\n" + cssText + "\n</style>");
            }
            catch
            {
                // return nothing if we can't read the file for any reason
                return null;
            }
        }
    }
