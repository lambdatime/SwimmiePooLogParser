//using System;
//using System.IO;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using System.Web;


//namespace SwimmiePooLogParserDrone.UI.Helpers
//{
//    public class JsonpMediaTypeFormatter : JsonMediaTypeFormatter
//    {
//        private string callbackQueryParameter;


//        public JsonpMediaTypeFormatter()
//        {
//            SupportedMediaTypes.Add(DefaultMediaType);
//            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));


//            MediaTypeMappings.Add(new UriPathExtensionMapping("jsonp", DefaultMediaType));
//        }


//        public string CallbackQueryParameter
//        {
//            get { return callbackQueryParameter ?? "callback"; }
//            set { callbackQueryParameter = value; }
//        }
//        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
//        {
//            string callback;


//            if (IsJsonpRequest(transportContext.Response.RequestMessage, out callback))
//            {
//                return Task.Factory.StartNew(() =>
//                {
//                    var writer = new StreamWriter(stream);
//                    writer.Write(callback + "(");
//                    writer.Flush();


//                    base.WriteToStreamAsync(type, value, writeStream, content,
//                                             transportContext).Wait();

//                    writer.Write(")");
//                    writer.Flush();
//                });
//            }
//            else
//            {
//                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
//            }
//        }

//        protected override Task OnWriteToStreamAsync(Type type, object value, Stream stream,
//                                                     HttpContentHeaders contentHeaders,
//                                                     FormatterContext formatterContext,
//                                                     TransportContext transportContext)
//        {
//            string callback;


//            if (IsJsonpRequest(formatterContext.Response.RequestMessage, out callback))
//            {
//                return Task.Factory.StartNew(() =>
//                {
//                    var writer = new StreamWriter(stream);
//                    writer.Write(callback + "(");
//                    writer.Flush();


//                    base.OnWriteToStreamAsync(type, value, stream, contentHeaders,
//                                            formatterContext, transportContext).Wait();

//                    writer.Write(")");
//                    writer.Flush();
//                });
//            }
//            else
//            {
//                return base.OnWriteToStreamAsync(type, value, stream, contentHeaders, formatterContext, transportContext);
//            }
//        }


//        private bool IsJsonpRequest(HttpRequestMessage request, out string callback)
//        {
//            callback = null;


//            if (request.Method != HttpMethod.Get)
//            {
//                return false;
//            }


//            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
//            callback = query[CallbackQueryParameter];


//            return !string.IsNullOrEmpty(callback);
//        }
//    }

//}