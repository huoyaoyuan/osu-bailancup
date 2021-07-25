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

            var scores = new List<(Player player, Score score, double acc)>();
            foreach (var player in players)
            {
                try
                {
                    var score = (await OsuApi.GetUserRecentAsync(player.user_id, mode, apiKey))[0];
                    double acc = score.CalcAcc(mode);
                    scores.Add((player, score, acc));
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

            if (scores.Select(x => x.score.beatmap_id).Distinct().Count() > 1)
                Console.WriteLine("注意：获取到谱面不相同！");

            scores = scores.OrderBy(s =>
            {
                double condition = winCondition switch
                {
                    WinCondition.Combo => s.score.maxcombo,
                    WinCondition.Score => s.score.score,
                    WinCondition.Acc => s.acc,
                    _ => throw new Exception("WTF")
                };
                return s.score.IsFail ? 1 / condition : -1 / condition;
            }).ToList();

            int maxUsernameLength = Math.Max(6, scores.Max(x => x.player.username.Length));
            int maxScoreLength = Math.Max(5, (int)Math.Log10(scores.Max(x => x.score.score)) * 4 / 3 + 1);
            int maxComboLength = Math.Max(5, (int)Math.Log10(scores.Max(x => x.score.maxcombo) + 2));
            Console.WriteLine($"{"Player".PadRight(maxUsernameLength)}  {"Score".PadLeft(maxScoreLength)}  {"Combo".PadLeft(maxComboLength)}      Acc  Stat");
            Console.WriteLine("============================================================");
            foreach (var (player, score, acc) in scores)
            {
                Console.WriteLine($"{player.username.PadRight(maxUsernameLength)}  {score.score.ToString("N").PadLeft(maxScoreLength)}  {score.maxcombo.ToString().PadLeft(maxComboLength - 1)}x  {acc,6:P2}  {(score.IsFail ? "FAIL" : "ALIVE")}");
            }
        }
    }

    record Settings(string apiKey, int mode);

    enum WinCondition { Combo, Score, Acc }
}
