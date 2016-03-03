using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace JosephGuadagno.Utilities.Web
{
    public class JSONPSupportInspector : IDispatchMessageInspector
    {
        // Assume utf-8, note that Data Services supports
        // charset negotiation, so this needs to be more
        // sophisticated (and per-request) if clients will 
        // use multiple charsets
        private static readonly Encoding encoding = Encoding.UTF8;

        private class Writer : BodyWriter
        {
            private readonly string content;

            public Writer(string content)
                : base(false)
            {
                this.content = content;
            }

            protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
            {
                writer.WriteStartElement("Binary");
                byte[] buffer = encoding.GetBytes(content);
                writer.WriteBase64(buffer, 0, buffer.Length);
                writer.WriteEndElement();
            }
        }

        #region IDispatchMessageInspector Members

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (request.Properties.ContainsKey("UriTemplateMatchResults"))
            {
                HttpRequestMessageProperty httpmsg =
                    (HttpRequestMessageProperty) request.Properties[HttpRequestMessageProperty.Name];
                UriTemplateMatch match = (UriTemplateMatch) request.Properties["UriTemplateMatchResults"];

                string format = match.QueryParameters["$format"];
                if ("json".Equals(format, StringComparison.InvariantCultureIgnoreCase))
                {
                    // strip out $format from the query options to avoid an error
                    // due to use of a reserved option (starts with "$")
                    match.QueryParameters.Remove("$format");

                    // replace the Accept header so that the Data Services runtime 
                    // assumes the client asked for a JSON representation
                    httpmsg.Headers["Accept"] = "application/json;odata=verbose";
                    httpmsg.Headers["Accept-Charset"] = "utf-8";


                    string callback = match.QueryParameters["$callback"];
                    if (!string.IsNullOrEmpty(callback))
                    {
                        match.QueryParameters.Remove("$callback");
                        return callback;
                    }
                }
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (correlationState != null && correlationState is string)
            {
                // if we have a JSONP callback then buffer the response, wrap it with the
                // callback call and then re-create the response message

                string callback = (string) correlationState;

                XmlDictionaryReader reader = reply.GetReaderAtBodyContents();
                reader.ReadStartElement();
                string content = encoding.GetString(reader.ReadContentAsBase64());

                content = callback + "(" + content + ")";

                Message newreply = Message.CreateMessage(MessageVersion.None, "", new Writer(content));
                newreply.Properties.CopyProperties(reply.Properties);

                reply = newreply;
            }
        }

        #endregion
    }

    // Simply apply this attribute to a DataService-derived class to get
    // JSONP support in that service
    [AttributeUsage(AttributeTargets.Class)]
    public class JSONPSupportBehaviorAttribute : Attribute, IServiceBehavior
    {
        #region IServiceBehavior Members

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    ed.DispatchRuntime.MessageInspectors.Add(new JSONPSupportInspector());
                }
            }
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}