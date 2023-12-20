using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace AutoVisualizer.Component.Templates
{
    public abstract class GH_Component_Sync : GH_Component
    {
        public string message = "Ready";

        public GH_Component_Sync(string name, string nickname, string description, string category, string subCategory)
            : base(name, nickname, description, category, subCategory)
        {
        }

        protected string POST(
            string url,
            string dataJSON,
            string contentType,
            string authorization,
            int timeout = 100000
            )
        {
            try
            {
                //var data = Newtonsoft.Json.JsonConvert.DeserializeObject(dataJSON);
                //AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, dataJSON);
                byte[] paramBytes = Encoding.UTF8.GetBytes(dataJSON);
                //byte[] data2 = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                //byte[] jsonDataBytes = Encoding.ASCII.GetBytes(dataJSON);
                //byte[] data = Encoding.ASCII.GetBytes(body);
                //var jsonDataBytes = WebRequestMethods.Http.FormUrlEncodedContent(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                //request.ContentType = contentType;
                request.ContentType = "application/json; charset=utf-8";
                request.ContentLength = paramBytes.Length;
                request.Timeout = timeout;

                if (authorization != null && authorization.Length > 0)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", authorization);
                }
                else
                {
                    request.Credentials = CredentialCache.DefaultCredentials;
                }

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(paramBytes, 0, paramBytes.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var responseStream = new StreamReader(response.GetResponseStream()))
                        {
                            string responseData = responseStream.ReadToEnd();
                            //dynamic responseJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
                            return responseData;
                        }
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Error: {response.StatusCode} - {new StreamReader(response.GetResponseStream()).ReadToEnd()}");
                        return "";
                    }
                }
            }
            catch (Exception ex)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something went wrong: " + ex.Message);
                return "";
            }
        }

        protected void UpdateMessage(string message)
        {
            Message = message;
        }

        //protected string POST(
        //    string url,
        //    string dataJSON,
        //    string contentType,
        //    string authorization,
        //    int timeout = 100000
        //    )
        //{
        //    try
        //    {
        //        var data = Newtonsoft.Json.JsonConvert.DeserializeObject(dataJSON);
        //        //byte[] data2 = Newtonsoft.Json.JsonConvert.SerializeObject(data);
        //        //byte[] jsonDataBytes = Encoding.UTF8.GetBytes(dataJSON);
        //        //byte[] data = Encoding.ASCII.GetBytes(body);
        //        var jsonDataBytes = WebRequestMethods.Http.FormUrlEncodedContent(data);

        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Method = "POST";
        //        request.ContentType = contentType;
        //        request.ContentLength = dataJSON.Length;
        //        request.Timeout = timeout;

        //        if (authorization != null && authorization.Length > 0)
        //        {
        //            ServicePointManager.Expect100Continue = true;
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //            request.PreAuthenticate = true;
        //            request.Headers.Add("Authorization", authorization);
        //        }
        //        else
        //        {
        //            request.Credentials = CredentialCache.DefaultCredentials;
        //        }

        //        using (var stream = request.GetRequestStream())
        //        {
        //            stream.Write(jsonDataBytes, 0, jsonDataBytes.Length);
        //        }

        //        using (var response = (HttpWebResponse)request.GetResponse())
        //        {
        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                using (var responseStream = new StreamReader(response.GetResponseStream()))
        //                {
        //                    string responseData = responseStream.ReadToEnd();
        //                    dynamic responseJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);
        //                    return responseJSON;
        //                }
        //            }
        //            else
        //            {
        //                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Error: {response.StatusCode} - {new StreamReader(response.GetResponseStream()).ReadToEnd()}");
        //                return "";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something went wrong: " + ex.Message);
        //        return "";
        //    }
        //}
    }
}