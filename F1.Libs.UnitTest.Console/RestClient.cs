using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace F1.Libs.UnitTest.Console
{
    internal class RestClient
    {
        public enum HttpVerbEnum
        {
            GET,
            POST
        }

        public class ContentTypeConst
        {
            public const string Json = "application/json";
            public const string Text = "text/xml";
            public const string PostForm = "application/x-www-form-urlencoded";
        }

        public string EndPoint { get; set; }
        public HttpVerbEnum Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }
        public int TimeOut { get; set; }

        public RestClient()
        {
            EndPoint = string.Empty;
            Method = HttpVerbEnum.GET;
            ContentType = ContentTypeConst.Text;
            PostData = string.Empty;
        }

        public RestClient(string endpoint, HttpVerbEnum method, string contentType)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = contentType;
            PostData = string.Empty;
        }

        public string MakeRequest(string userCredentials = "", string passCredentials = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(userCredentials) && !string.IsNullOrEmpty(passCredentials))
            {
                request.Credentials = new NetworkCredential(userCredentials, passCredentials);
            }

            if (!string.IsNullOrEmpty(PostData) && Method == HttpVerbEnum.POST)
            {
                var bytes = Encoding.UTF8.GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            var responseValue = string.Empty;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            return responseValue;
        }

        public string MakeRequestTimeOut(int timeOut, string userCredentials = "", string passCredentials = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Timeout = timeOut;
            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(userCredentials) && !string.IsNullOrEmpty(passCredentials))
            {
                request.Credentials = new NetworkCredential(userCredentials, passCredentials);
            }

            if (!string.IsNullOrEmpty(PostData) && Method == HttpVerbEnum.POST)
            {
                var bytes = Encoding.UTF8.GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }

            var responseValue = string.Empty;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            return responseValue;
        }

        public string MakeRequestCatchException(string userCredentials = "", string passCredentials = "")
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;

            if (!string.IsNullOrEmpty(userCredentials) && !string.IsNullOrEmpty(passCredentials))
            {
                request.Credentials = new NetworkCredential(userCredentials, passCredentials);
            }

            var responseValue = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(PostData) && Method == HttpVerbEnum.POST)
                {
                    var bytes = Encoding.UTF8.GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }
                
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)wex.Response;
                    StreamReader responseReader = new StreamReader(response.GetResponseStream());
                    responseValue = responseReader.ReadToEnd();
                    response.Close();
                }
                else
                {
                    responseValue = string.Format("{0} - {1} - {2}", wex.Message, wex.Status.ToString(), wex.StackTrace);
                }
            }

            return responseValue;
        }
    }
}
