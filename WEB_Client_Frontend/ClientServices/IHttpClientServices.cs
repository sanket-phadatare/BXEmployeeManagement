namespace WEB_Client_Frontend.ClientServices
{
    public interface IHttpClientServices
    {
        Task<T> GetAsync<T>(string requestUri);

        Task<T> PostAsync<T>(string requestUri, Object content);

        Task<T> PutAsync<T>(string requestUri, Object content);

        Task<T> DeleteAsync<T>(string requestUri);

        Task<T> DeleteWithBodyAsync<T>(string requestUri, object content);  

    }
}
