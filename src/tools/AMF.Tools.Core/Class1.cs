using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMF.Parser.Model;

namespace AMF.Tools.Core
{
    public class WebApiGenerator2
    {
        public WebApiGenerator2(AmfModel model)
        {
            foreach(var endpoint in model.WebApi.EndPoints)
            {
                foreach(var operation in endpoint.Operations)
                {

                }
            }
        }
    }
}
