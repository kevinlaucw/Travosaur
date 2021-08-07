using System;
using System.Web;

namespace Travosaur
{
    public class TravosaurHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the Web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //write your handler implementation here.
            
            //To enable this, check <httpHandlers> in Web.config
            string RequestedPage = context.Request.Url.Segments[2].ToLower();
            if (RequestedPage == "About.aspx")
                context.Response.Redirect("~/About");
            else if (RequestedPage == "Contact.aspx")
                context.Response.Redirect("~/Contact");
            else
                context.Response.Redirect("~/Home");              
        }

        #endregion
    }
}
