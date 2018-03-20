using Naylah.Services.Client;
using Naylah.Services.Client.MessageHandlers;

namespace Naylah.Identity.Client
{
    public static class NaylahIdentityServiceClientExtensions
    {
        //public static NaylahIdentityClient GetNaylahIdentityClient(this NaylahServiceClient serviceClient)
        //{
        //    var nyIdentityClient = new NaylahIdentityClient(
        //        serviceClient.ServiceSettings.CustomProperties[ServiceSettingsKeys.NaylahIdentityClientId],
        //        serviceClient.ServiceSettings.CustomProperties[ServiceSettingsKeys.NaylahIdentityClientSecret]
        //        );

        //    nyIdentityClient.RequestScopes = serviceClient.ServiceSettings.CustomProperties[ServiceSettingsKeys.NaylahIdentityRequestScopes];

        //    return nyIdentityClient;
        //}



        //public static void AddNaylahIdentityAuthentication(
        //    this NaylahServiceClientSettings serviceClientSettings,
        //    string clientId,
        //    string clientSecrect = "",
        //    string requestScopes = null)
        //{
        //    if (string.IsNullOrEmpty(clientId))
        //    {
        //        throw new ArgumentNullException("clientId is empty or null");
        //    }

        //    if (string.IsNullOrEmpty(clientSecrect))
        //    {
        //        throw new ArgumentNullException("clientSecrect is empty or null");
        //    }

        //    serviceClientSettings.CustomProperties[ServiceSettingsKeys.NaylahIdentityClientId] = clientId;
        //    serviceClientSettings.CustomProperties[ServiceSettingsKeys.NaylahIdentityClientSecret] = clientSecrect;
        //    serviceClientSettings.CustomProperties[ServiceSettingsKeys.NaylahIdentityRequestScopes] = requestScopes;

        //    serviceClientSettings.AddNaylahIdentityMessageHandler();
        //}


        //public static async Task UpdateCurrentUserInfo(this NaylahServiceClient serviceClient)
        //{
        //    if ((NaylahServiceUser)serviceClient.CurrentUser == null)
        //    {
        //        return;
        //    }

        //    var nyIdentityClient = serviceClient.GetNaylahIdentityClient();

        //    var userInfoResponse = await nyIdentityClient.GetUserInfo(serviceClient.CurrentUser.AccessToken);

        //    if (!userInfoResponse.IsError)
        //    {
        //        var modelClaims = userInfoResponse.Claims.Select(x => new Claim() { ClaimType = x.Item1, Value = x.Item2 });

        //        ((NaylahServiceUser)serviceClient.CurrentUser).UpdateClaims(modelClaims);
        //    }
        //    else
        //    {
        //        if (userInfoResponse.IsHttpError)
        //        {
        //            //"HTTP error: ".ConsoleGreen();
        //            //Console.WriteLine(response.HttpErrorStatusCode);
        //            //"HTTP error reason: ".ConsoleGreen();
        //            //Console.WriteLine(response.HttpErrorReason);
        //        }
        //        //else
        //        ////{
        //        ////    "Protocol error response:".ConsoleGreen();
        //        ////    Console.WriteLine(response.Json);
        //        //}
        //    }
        //}

        internal static void AddNaylahIdentityMessageHandler(this NaylahServiceClientSettings settings)
        {
            settings.AddOrReplaceMessageHandler(
                new BearerAuthenticationHeaderMessageHandler()
                );
        }
    }
}