using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Runtime.CompilerServices
{
    static class IsExternalInit { }
}

namespace osu_bailancup
{
    static class OsuApi
    {
        private static readonly HttpClient httpClient = new()
        {
            BaseAddress = new("https://osu.ppy.sh")
        };

        private static readonly JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

        public static async Task<ApiUser> GetUserAsync(string username, int mode, string apiKey)
        {
            using var response = await httpClient.PostAsJsonAsync("/get_user", new
            {
                u = username,
                type = "string",
                m = mode,
                k = apiKey
            }, options);
            return await response.Content.ReadFromJsonAsync<ApiUser>(options);
        }

        public static async Task<Score[]> GetUserRecentAsync(int userId, int mode, string apiKey)
        {
            using var response = await httpClient.PostAsJsonAsync("/get_user_recent", new
            {
                u = userId,
                type = "id",
                m = mode,
                limit = 1,
                k = apiKey
            }, options);
            return await response.Content.ReadFromJsonAsync<Score[]>(options);
        }
    }

    public record ApiUser(string username, int user_id);
    public record Score(
        int beatmap_id,
        int score,
        int maxcombo,
        int count50,
        int count100,
        int count300,
        int countmiss,
        int countkatu,
        int countgeki,
        string rank)
    {

    }
}
