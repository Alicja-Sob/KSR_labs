using Shared_Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wydawca_W
{

    public static class Gathering_Stats
    {
        // proby obslugi kazdego typu
        public static ConcurrentDictionary<string, int> tries_perType = new();

        // sukcesy obslugi kazdego typu
        public static ConcurrentDictionary<string, int> success_perType = new();

        // opublikowane komunikaty kazdego typu
        public static ConcurrentDictionary<string, int> published_perType = new();

        public static void CountMessage(ConcurrentDictionary<string, int> dict, string key)
        {
            dict.AddOrUpdate(key, 1, (_, old) => old + 1);
        }
    }
}