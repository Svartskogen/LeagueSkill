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
        public enum Server { BR1,EUN1,EUW1,JP1,KR,LA1,LA2,NA1,OC1,TR1,RU,PBE1}
        public static async Task<Summoner> GetSummonerData(RiotClient api, string username, Server server)
        {
            Summoner summoner = await api.GetSummonerBySummonerNameAsync(username, server.ToString()).ConfigureAwait(false);
            return summoner;
        }
        public static async Task<List<LeagueEntry>> GetLeagues(RiotClient api, Summoner summoner, Server server)
        {
            if(summoner != null)
            {
                List<LeagueEntry> list = await api.GetLeagueEntriesBySummonerIdAsync(summoner.Id, server.ToString()).ConfigureAwait(false);
                return list;
            }
            return null;
        }
        public static string PrintLeagueData(LeagueEntry entry)
        {
            return PrettyQueue(entry.QueueType) + ": "  + PrettyTier(entry.Tier) + " " + entry.Rank + " " + entry.LeaguePoints + "LP " + ((float)entry.Wins/entry.Losses * 50f).ToString("0.00") + " % WR ";
        }
        public static string PrettyQueue(string queue)
        {
            switch (queue)
            {
                case "RANKED_SOLO_5x5":
                    return "Solo/Duo";
                case "RANKED_FLEX_SR":
                    return "Flex";
                case "RANKED_FLEX_TT":
                    return "TFT";
            }
            return "null";
        }
        public static string PrettyTier(string tier)
        {
            var str = tier.ToString();
            str = str.First().ToString() + str.Substring(1).ToLower();
            return str;
        }
    }
}
