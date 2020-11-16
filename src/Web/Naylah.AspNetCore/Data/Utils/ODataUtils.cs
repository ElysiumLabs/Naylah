using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Naylah.Data.Utils
{
    public static class ODataUtils
    {
        //refrac
        public static Uri TryBuildNextLink<TModel>(IEnumerable<TModel> queryResult, int totalCount, HttpRequest request) where TModel : class, new()
        {
            try
            {
                var uri = new Uri(UriHelper.GetDisplayUrl(request));
                var qs = HttpUtility.ParseQueryString(uri.Query);
                var skip = qs.Get("$skip") ?? "0";
                if (!string.IsNullOrEmpty(skip))
                {
                    var skipInt = Convert.ToInt64(skip) + queryResult.Count();
                    if (skipInt < totalCount)
                    {
                        qs.Set("$skip", skipInt.ToString());

                        var ruri = new UriBuilder(uri);
                        ruri.Query = qs.ToString();

                        return ruri.Uri;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceWarning(ex.Message);
            }

            return null;

        }
    }


}
