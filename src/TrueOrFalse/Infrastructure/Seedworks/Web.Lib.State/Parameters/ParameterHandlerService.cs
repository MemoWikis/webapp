using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Seedworks.Web.State
{
    public class ParameterHandlerService
    {
        private readonly IHttpCurrent _httpCurrent;

        private IRequest _request { get { return _httpCurrent.Request; } }
        private IResponse _response { get { return _httpCurrent.Response; } }

        public ParameterHandlerService(IHttpCurrent httpCurrent)
        {
            _httpCurrent = httpCurrent;
        }

        public void ProcessParams(ParameterHandlerList parameterHandlers)
        {
            ProcessParams(_request.QueryString, parameterHandlers);
        }

        /// <summary>
        /// Processes all given parameters.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="parameterHandlers"></param>
        public void ProcessParams(NameValueCollection queryParams, ParameterHandlerList parameterHandlers)
        {
            foreach (string itemKey in queryParams.Keys)
                if (parameterHandlers.Contains(itemKey))
                {
                    var handler = parameterHandlers.GetByName(itemKey);

                    if (handler.AppliesOnlyLocal && !ContextUtil.IsLocal)
                        continue;

                    handler.Action(queryParams[itemKey]);
                }
        }

		public void ProcessParam(ParameterHandler parameterHandler)
		{
			ProcessParam(_request.QueryString, parameterHandler);
		}

		public void ProcessParam(NameValueCollection queryParams, ParameterHandler parameterHandler)
		{
			if (parameterHandler.AppliesOnlyLocal && !ContextUtil.IsLocal)
				return;

			parameterHandler.Action(queryParams[parameterHandler.Name]);
		}


        /// <summary>
        /// True if query string contains a parameter handled by a registered <see cref="ParameterHandler">ParamaterHandler</see>.
        /// </summary>
        /// <param name="parameterHandler"></param>
        /// <returns></returns>
        public bool DoesHandlerExist(ParameterHandler parameterHandler)
        {
            return DoesHandlerExist(_request.QueryString, parameterHandler);
        }

        /// <summary>
        /// True if query string contains a parameter handled by a registered <see cref="ParameterHandler">ParamaterHandler</see>.
        /// </summary>
        /// <param name="queryParams"></param>
        /// <param name="parameterHandler"></param>
        /// <returns></returns>
        private bool DoesHandlerExist(NameValueCollection queryParams, ParameterHandler parameterHandler)
        {
            var value = queryParams.Get(parameterHandler.Name);

            return !string.IsNullOrEmpty(value);
        }
    }
}
