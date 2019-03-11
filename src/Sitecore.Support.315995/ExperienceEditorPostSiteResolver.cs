using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.Support.Pipelines.HttpRequest
{
  public class ExperienceEditorPostSiteResolver : HttpRequestProcessor
  {
    private const string ribbonPath = "/sitecore/client/applications/experienceeditor/ribbon.aspx";
    private static readonly string[] systemSites = new string[] { "shell", "login", "admin", "service", "modules_shell", "scheduler", "system", "publisher" };

    // Methods
    public override void Process(HttpRequestArgs args)
    {
      SiteContext site = Context.Site;
      if ((site == null) || ((site != null) && !systemSites.Any<string>(p => p.Equals(site.Name))))
      {
        Uri urlReferrer = HttpContext.Current.Request.UrlReferrer;
        if ((urlReferrer != null) && urlReferrer.AbsolutePath.ToLowerInvariant().Equals("/sitecore/client/applications/experienceeditor/ribbon.aspx"))
        {
          string str = HttpContext.Current.Server.UrlDecode(urlReferrer.Query);
          if (str.Contains("?sc_site=") || str.Contains("&sc_site="))
          {
            char[] separator = new char[] { '&', '?' };
            string str2 = str.Split(separator, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault<string>(p => p.Contains("sc_site="));
            if (!string.IsNullOrWhiteSpace(str2))
            {
              char[] chArray2 = new char[] { '=' };
              string[] strArray = str2.Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
              if ((strArray.Length == 2) && !string.IsNullOrWhiteSpace(strArray[1]))
              {
                SiteContext siteContext = SiteContextFactory.GetSiteContext(strArray[1]);
                if (siteContext != null)
                {
                  Context.Site=siteContext;
                }
              }
            }
          }
        }
      }
    }
  }
}
