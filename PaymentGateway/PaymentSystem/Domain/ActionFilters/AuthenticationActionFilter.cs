using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PaymentSystem.Core.Domain.StateManagement;
using Microsoft.Extensions.DependencyInjection;
using PaymentSystem.Core.Helpers;
using PaymentSystem.Gateway.Helpers;

namespace PaymentSystem.Gateway.Domain.ActionFilters
{
  public class AuthenticationActionFilter: ActionFilterAttribute
  {
    private ICacheManager _cacheManager;

    /// <summary>
    /// ActionFilter which will authenticate the merchant using the merchantId and the merchantSecret params
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      _cacheManager = context.HttpContext.RequestServices.GetService<ICacheManager>();
      if (context.Controller is Controller controller)
      {
        //retrieving params from request
        var merchantId = controller.Request.Query[Core.Constants.QueryParams.MerchantId];
        var merchantSecret = controller.Request.Query[Core.Constants.QueryParams.MerchantSecret];

        //retrieving authentication settings from cache
        var authenticationSettings = _cacheManager.GetAuthenticationSettings();

        //validating if params are null
        if (string.IsNullOrWhiteSpace(merchantId))
        {
          context.Result = AuthenticationHelper.GetRedirectToRouteResult
            (authenticationSettings, Core.Constants.ErrorMessages.MandatoryMerchantId);
          return;
        }
        if (string.IsNullOrWhiteSpace(merchantSecret))
        {
          context.Result = AuthenticationHelper.GetRedirectToRouteResult
          (authenticationSettings, Core.Constants.ErrorMessages.MandatoryMerchantSecret);
          return;
        }

        //Getting merchant settings from cache
        var merchantSettings = _cacheManager.GetMerchantSettings(merchantId);
        if (merchantSettings == null)
        {
          context.Result = AuthenticationHelper.GetRedirectToRouteResult
            (authenticationSettings, Core.Constants.ErrorMessages.AuthenticationFailed);
          return;
        }

        #region MerchantSecret validation
        if (authenticationSettings.EnableHash)
        {
          //if hash is enabled in config file, then it generates the hash to make the validation
          var hashedSecret = SecurityHelper.Hash(merchantSecret, merchantSettings.MerchantKey);
          if (!string.Equals(hashedSecret, merchantSettings.MerchantHashedSecret))
          {
            context.Result = AuthenticationHelper.GetRedirectToRouteResult
              (authenticationSettings, Core.Constants.ErrorMessages.AuthenticationFailed);
            return;
          }
        }
        else
        {
          if (!string.Equals(merchantSecret.ToString(), merchantSettings.MerchantSecret))
          {
            context.Result = AuthenticationHelper.GetRedirectToRouteResult
              (authenticationSettings, Core.Constants.ErrorMessages.AuthenticationFailed);
            return;
          }
        }
        #endregion
      }
      base.OnActionExecuting(context);
    }
  }
}
