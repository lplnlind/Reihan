using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Reihan.Client.Authentication
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthHeaderHandler(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthHeaderHandler] Error setting header: {ex.Message}");
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
