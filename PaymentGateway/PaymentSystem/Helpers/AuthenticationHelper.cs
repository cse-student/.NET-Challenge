using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PaymentSystem.Core.Configuration;

namespace PaymentSystem.Gateway.Helpers
{
  public static class AuthenticationHelper
  {
    /// <summary>
    /// Helper function to generate RedirectToRouteResult to unauthorized request webservice handler
    /// </summary>
    /// <param name="authenticationSettings"></param>
    /// <param name="message">message to return in webservice</param>
    /// <returns></returns>
    public static RedirectToRouteResult GetRedirectToRouteResult(AuthenticationSettings authenticationSettings, string message)
    {
      return new RedirectToRouteResult(
        new RouteValueDictionary {
          { Core.Constants.RoutingKeys.Controller, authenticationSettings.Failure.Controller },
          { Core.Constants.RoutingKeys.Action, authenticationSettings.Failure.Action },
          { Core.Constants.RoutingKeys.Message, message }
        });
    }
  }
}
