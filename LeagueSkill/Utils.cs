using LeagueSkill.Models;
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
            return PrettyQueue(entry.QueueType) + ": "  + PrettyTier(entry.Tier) + " " + entry.Rank + ", " + entry.LeaguePoints + "LP \n" + ((float)entry.Wins/entry.Losses * 50f).ToString("0.00") + " % WR ";
        }
        public static LeagueParsedData ParseLeague(LeagueEntry entry)
        {
            var data = new LeagueParsedData();
            data.FirstLine = PrettyQueue(entry.QueueType) + ": " + PrettyTier(entry.Tier) + " " + entry.Rank;
            data.LP = entry.LeaguePoints + "LP ";
            var wr = (float)entry.Wins / entry.Losses * 50f;
            data.WinRatio = (wr).ToString("0.00") + "% WR";
            data.WinRatioColor = GetWinRatioColor(wr);
            data.WinLoss = "(" + entry.Wins + "W/" + entry.Losses + "L)";
            data.ImagePath = GetLeaguePath(entry.Tier);
            return data;
        }
        public static string GetWinRatioColor(float ratio)
        {
            if (ratio <= 46)
            {
                return "text-danger";
            }
            else if(ratio <= 54)
            {
                return "text-primary";
            }
            else
            {
                return "text-success";
            }
        }
        public static string GetLeaguePath(string tier)
        {
            switch (tier)
            {
                case "CHALLENGER":
                    return "/img/Emblem_Challenger.png";
                case "GRANDMASTER":
                    return "/img/Emblem_Grandmaster.png";
                case "MASTER":
                    return "/img/Emblem_Master.png";
                case "DIAMOND":
                    return "/img/Emblem_Diamond.png";
                case "PLATINUM":
                    return "/img/Emblem_Platinum.png";
                case "GOLD":
                    return "/img/Emblem_Gold.png";
                case "SILVER":
                    return "/img/Emblem_Silver.png";
                case "BRONZE":
                    return "/img/Emblem_Bronze.png";
                case "IRON":
                    return "/img/Emblem_Iron.png";
            }
            return "";
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
