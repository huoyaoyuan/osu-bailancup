using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace osu_bailancup
{
    class CupManager
    {
        private readonly string apiKey;
        private readonly int mode;
        private readonly List<Player> players = new();

        public CupManager(Settings settings)
        {
            apiKey = settings.apiKey;
            mode = settings.mode;
        }

        public async Task AddPlayerAsync(string username)
        {
            if (players.Any(x => x.username == username))
            {
                Console.WriteLine("列表中已有该选手");
                return;
            }
            var player = await OsuApi.GetUserAsync(username, mode, apiKey);
            players.Add(player);
        }

        public void RemovePlayer(string username)
        {
            if (players.RemoveAll(x => x.username == username) == 0)
                Console.WriteLine("列表中不存在该选手");

            Console.WriteLine("当前选手列表：" + string.Join(",", players.Select(x => x.username)));
        }
    }

    record Settings(string apiKey, int mode);
}
