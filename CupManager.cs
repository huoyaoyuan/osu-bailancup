using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_bailancup
{
    class CupManager
    {
        private readonly string apiKey;
        private readonly int mode;

        public CupManager(string apiKey, int mode)
        {
            this.apiKey = apiKey;
            this.mode = mode;
        }
    }
}
