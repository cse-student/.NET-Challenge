namespace PaymentSystem.Core.Configuration
{
  public class AuthenticationSettings
  {
    public bool EnableHash { get; set; }
    public Failure Failure { get; set; }
  }

  public class Failure
  {
  public string Controller { get; set; }
  public string Action { get; set; }
  public string Param { get; set; }
}
}
