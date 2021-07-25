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
    }

    record Settings(string apiKey, int mode);
}
