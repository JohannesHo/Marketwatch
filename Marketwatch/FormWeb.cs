using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Marketwatch
{
    public partial class FormWeb : Form
    {
        private SteamWeb steamWeb;

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        public FormWeb(SteamWeb steamWeb) : this()
        {
            this.steamWeb = steamWeb;
            if (this.steamWeb != null && this.steamWeb.initalized)
            {
                InternetSetCookie("http://steamcommunity.com", "sessionid", steamWeb.SessionId);
                InternetSetCookie("http://steamcommunity.com", "steamLogin", steamWeb.Token);
                InternetSetCookie("http://steamcommunity.com", "steamLoginSecure", steamWeb.TokenSecure);
            }
        }

        public FormWeb()
        {
            InitializeComponent();
            InternetSetCookie("http://steamcommunity.com", "bCompletedTradeOfferTutorial", "true");
            InternetSetCookie("http://steamcommunity.com", "strInventoryLastContext", "730_2");
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(e.Url.AbsoluteUri == "https://steamcommunity.com/login/home/?goto=%2Ftradeoffer%2Fnew%2F%3Fpartner%3D44396159%26token%3DPAn2liAp")
            {
                // Find the page header and co., and remove it.  This gives the login form a more streamlined look.
                try
                {
                    dynamic htmldoc = webBrowser.Document.DomDocument;
                    dynamic globalHeader = htmldoc.GetElementById("global_header");
                    dynamic whyJoinLeft = htmldoc.GetElementsByClassName("whyJoinLeft")[0];
                    dynamic whyJoinRight = htmldoc.GetElementsByClassName("whyJoinRight")[0];
                    dynamic responsivePage = htmldoc.GetElementsByClassName("responsive_page")[0];

                    if (globalHeader != null)
                        globalHeader.parentNode.removeChild(globalHeader);

                    if (whyJoinLeft != null)
                        whyJoinLeft.parentNode.removeChild(whyJoinLeft);

                    if (whyJoinRight != null)
                        whyJoinRight.parentNode.removeChild(whyJoinRight);

                    if (responsivePage != null)
                        responsivePage.style.minHeight = "75%";
                }
                catch (Exception) { }

            } else if (e.Url.AbsoluteUri == "https://steamcommunity.com/tradeoffer/new/?partner=44396159&token=PAn2liAp")
            {
                this.webBrowser.Document.InvokeScript("Tutorial.EndTutorial();");
            }

            CookieContainer cookieContainer = GetUriCookieContainer(e.Url);
            CookieCollection cookies = cookieContainer.GetCookies(e.Url);
            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine(cookie.Name + ": " + cookie.Value);
            }

            this.Show();
            return;
        }

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(
        string url,
        string cookieName,
        StringBuilder cookieData,
        ref int size,
        Int32 dwFlags,
        IntPtr lpReserved);

        private const Int32 InternetCookieHttponly = 0x2000;

        /// <summary>
        /// Gets the URI cookie container.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            // Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, InternetCookieHttponly, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;
                // Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(
                    uri.ToString(),
                    null, cookieData,
                    ref datasize,
                    InternetCookieHttponly,
                    IntPtr.Zero))
                    return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }
    }
}
