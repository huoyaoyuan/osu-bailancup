using System;
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
            BaseAddress = new("https://osu.ppy.sh/api")
        };

        private static readonly JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

        public static async Task<Player> GetUserAsync(string username, int mode, string apiKey)
        {
            using var response = await httpClient.PostAsJsonAsync("/get_user", new
            {
                u = username,
                type = "string",
                m = mode,
                k = apiKey
            }, options);
            return await response.Content.ReadFromJsonAsync<Player>(options);
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

    public record Player(string username, int user_id);
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
        public double CalcAcc(int mode)
        {
            return mode switch
            {
                0 => (count50 * 50 + count100 * 100 + count300 * 300) / ((count50 + count100 + count300 + countmiss) * 300.0),
                1 => (count100 * 50 + count300 * 100) / ((count100 + count300 + countmiss) * 100.0),
                2 => (count50 + count100 + count300) / (double)(count50 + count100 + count300 + countmiss + countkatu + countmiss),
                3 => (count50 * 50 + count100 * 100 + countkatu * 200 + count300 * 300 + countgeki * 300) / ((count50 + count100 + count300 + countmiss + countkatu + countgeki + countmiss) * 300.0),
                _ => throw new Exception("WTF mode?")
            };
        }

        public bool IsFail => rank == "F";
    }
}
