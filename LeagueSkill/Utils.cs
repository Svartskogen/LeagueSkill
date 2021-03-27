using RiotNet;
using RiotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueSkill
{
    public static class Utils
    {
        public static async Task<Summoner> GetSummonerData(RiotClient api, string username)
        {
            Summoner summoner = await api.GetSummonerBySummonerNameAsync(username, PlatformId.LA2).ConfigureAwait(false);
            return summoner;
        }
        public static async Task<List<LeagueEntry>> GetLeagues(RiotClient api, Summoner summoner)
        {
            List<LeagueEntry> list = await api.GetLeagueEntriesBySummonerIdAsync(summoner.Id, PlatformId.LA2).ConfigureAwait(false);
            return list;
        }
        public static string GetLeagueData(LeagueEntry entry)
        {
            return entry.QueueType + " "  + entry.Tier + " " + entry.Rank + " " + entry.LeaguePoints + "LP " + ((float)entry.Wins/entry.Losses * 50f).ToString() + "%WR ";
        }
    }
}
