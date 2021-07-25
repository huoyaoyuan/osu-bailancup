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

        public async Task PrintScoreAsync(WinCondition winCondition)
        {
            Console.WriteLine("获取数据中...");

            var scores = new List<Score>();
            foreach (var player in players)
            {
                try
                {
                    scores.Add((await OsuApi.GetUserRecentAsync(player.user_id, mode, apiKey))[0]);
                }
                catch
                {
                    Console.WriteLine($"获取{player.username}的成绩失败");
                }
            }

            if (scores.Count == 0)
            {
                Console.WriteLine("无数据");
                return;
            }


        }
    }

    record Settings(string apiKey, int mode);

    enum WinCondition { Combo, Score, Acc }
}
