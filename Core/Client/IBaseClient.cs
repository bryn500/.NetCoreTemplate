using System.Net.Http;

namespace $safeprojectname$.Core.Client
{
    public interface IBaseClient
    {
        HttpClient GetHttpClient();
    }
}