namespace Diploma.WebAPI.Infrastructure.Security
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUsername();
    }
}