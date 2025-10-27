using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POC_Home_Page
{
    public class HomeItemResolvercs : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            string referrerUrl = System.Web.HttpContext.Current.Request.UrlReferrer?.AbsoluteUri;
            string mySiteUrl = "homepagepoc.localhost"; // this is my site name in IIS, if you have it differently hosted, just change this to yours

          // When path is of root and there is no referer that means its organically hitting the site home page
           if(string.IsNullOrEmpty(referrerUrl) && System.Web.HttpContext.Current.Request.FilePath.Equals("/"))
            {
                var teaserItem = GetTeaserItem();
                if (teaserItem != null)
                    Context.Item = teaserItem;
            }
           // when path is of root and when there is a refere that means, home link was clicked from somewhere 
            else if(!string.IsNullOrEmpty(referrerUrl) && System.Web.HttpContext.Current.Request.FilePath.Equals("/"))
            {
              // if from search engine or email capiangs, if refer comes then also it should load teaser page, for our internal pages, it should always open old home page
               if(!referrerUrl.Contains("localhost") && !referrerUrl.Contains(mySiteUrl))
                {
                    var teaserItem = GetTeaserItem();
                    if (teaserItem != null)
                        Context.Item = teaserItem;
                }
               // for all other links load home page
                var homeItem = GetHomeItem();
                if (homeItem != null)
                    Context.Item = homeItem;
            }
           
        }

        private Item GetTeaserItem()
        {
            var sitecoreDB = Sitecore.Context.Database;
            var sitecoreQuery = $"/sitecore/content/Home/newhome";
            if (sitecoreDB != null)
            {
                var teaserItem = sitecoreDB.SelectSingleItem(sitecoreQuery);
                return teaserItem;
            }
            return null;
        }

        private Item GetHomeItem()
        {
            var sitecoreDB = Sitecore.Context.Database;
            var sitecoreQuery = $"/sitecore/content/Home";
            if (sitecoreDB != null)
            {
                var homeItem = sitecoreDB.SelectSingleItem(sitecoreQuery);
                return homeItem;
            }
            return null;
        }
    }
}