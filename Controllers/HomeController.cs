using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiGitHubBack.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("GetUsuario")]
        public ActionResult GetUsuario(string usuario)
        {
            try
            {
                string url = "https://api.github.com/users/" + usuario;

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = "request";

                HttpWebResponse res;

                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                }
                catch (Exception)
                {
                    return new ContentResult() { StatusCode = (int)HttpStatusCode.NoContent };
                }

                string user = "";

                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    user = sr.ReadToEnd();
                }

                string repos = "";

                url = "https://api.github.com/users/" + usuario + "/repos";
                req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = "request";
                res = (HttpWebResponse)req.GetResponse();

                using (Stream stream = res.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    repos = sr.ReadToEnd();
                }

                string content = "[" + user + "," + repos + "]";

                return new ContentResult() { Content = content, StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception ex)
            {
                return new ContentResult() { Content = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}
