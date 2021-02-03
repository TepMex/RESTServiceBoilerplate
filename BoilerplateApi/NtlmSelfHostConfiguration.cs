using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using System.Web.Http.SelfHost.Channels;

namespace BoilerplateApi
{
    public class NtlmSelfHostConfiguration : HttpSelfHostConfiguration
    {
        public NtlmSelfHostConfiguration(string baseAddress)
            : base(baseAddress)
        { }

        public NtlmSelfHostConfiguration(Uri baseAddress)
            : base(baseAddress)
        { }

        protected override BindingParameterCollection OnConfigureBinding(HttpBinding httpBinding)
        {
            httpBinding.ConfigureTransportBindingElement = element =>
            {
                element.AuthenticationScheme = System.Net.AuthenticationSchemes.Anonymous | System.Net.AuthenticationSchemes.Ntlm;
            };
            return base.OnConfigureBinding(httpBinding);
        }
    }
}
