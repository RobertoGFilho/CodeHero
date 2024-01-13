using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace CodeHero
{
    public class MarvelApiClient
    {
        private const string ApiBaseUrl = "https://gateway.marvel.com/v1/public/";
        private const string PublicKey = "9967edc88b66730f70d85fa90134571c";
        private const string PrivateKey = "3b47e3b9b5de7de84cfaaf4f44aac6d9a1879465";

        private readonly HttpClient _httpClient;

        public MarvelApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Rootobject> GetCharactersAsync(int offset, int limit, string nameFilter = "")
        {
            string endpoint = "characters";
            string timestamp = DateTime.Now.Ticks.ToString();
            string hash = Utils.GenerateMd5Hash($"{timestamp}{PrivateKey}{PublicKey}");

            string url = $"{ApiBaseUrl}{endpoint}?apikey={PublicKey}&ts={timestamp}&hash={hash}&offset={offset}&limit={limit}";

            if (!string.IsNullOrEmpty(nameFilter))
            {
                url += $"&nameStartsWith={nameFilter}";
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Rootobject>(json);
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }
    }

    public static class Utils
    {
        public static string GenerateMd5Hash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

}



