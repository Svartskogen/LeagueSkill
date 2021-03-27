using LeagueSkill.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RiotNet;
using RiotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeagueSkill.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]

        public ProfileQuery Profile { get; set; }
        public bool ValidSearch { get; private set; } //true si es valido el parametro de busqueda
        public bool DataLoaded { get; private set; } //true si encontro el invocador

        //Model
        public Summoner SummonerInfo { get; set; }
        public List<LeagueEntry> Leagues { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            ValidSearch = false;
            DataLoaded = false;
        }
        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(Profile.Username))
            {
                ValidSearch = true;
                //buscar perfil
                var api = new RiotClient(new RiotClientSettings
                {
                    ApiKey = "RGAPI-a6614a66-8fd2-48b3-ad30-a7137d3b7d57"
                });
                SummonerInfo = Utils.GetSummonerData(api, Profile.Username).Result;
                Leagues = Utils.GetLeagues(api, SummonerInfo).Result;

                DataLoaded = SummonerInfo != null;
            }
            return Page();
        } 
    }
}
