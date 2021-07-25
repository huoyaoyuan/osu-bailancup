using System.Threading.Tasks;

namespace osu_bailancup
{
    class CupManager
    {
        private readonly string apiKey;
        private readonly int mode;

        public CupManager(Settings settings)
        {
            apiKey = settings.apiKey;
            mode = settings.mode;
        }

        public async Task AddUserAsync(string username)
        {

        }

        public async Task RemoveUserAsync(string username)
        {

        }
    }

    record Settings(string apiKey, int mode);
}
