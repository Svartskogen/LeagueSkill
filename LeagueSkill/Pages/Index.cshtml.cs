using LeagueSkill.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty(SupportsGet = true)]
        public string Region { get; set; }
        public List<SelectListItem> RegionsList { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value = "BR1", Text = "Brazil"},
            new SelectListItem{Value = "EUN1", Text = "Europe Nordic and East"},
            new SelectListItem{Value = "EUW1", Text = "Europe West"},
            new SelectListItem{Value = "JP1", Text = "Japan"},
            new SelectListItem{Value = "KR", Text = "Korea"},
            new SelectListItem{Value = "LA1", Text = "Latin America North"},
            new SelectListItem{Value = "LA2", Text = "Latin America South"},
            new SelectListItem{Value = "NA1", Text = "North America"},
            new SelectListItem{Value = "OC1", Text = "Oceania"},
            new SelectListItem{Value = "TR1", Text = "Turkey"},
            new SelectListItem{Value = "RU", Text = "Russia"},
            new SelectListItem{Value = "PBE1", Text = "PBE"},
        };
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
                    ApiKey = "RGAPI-7647d118-e233-4cf3-b3db-4ab38cefa973"
                });
                SummonerInfo = Utils.GetSummonerData(api, Profile.Username,
                    ParseRegion()).Result;
                Leagues = Utils.GetLeagues(api, SummonerInfo,
                    ParseRegion()).Result;
                DataLoaded = SummonerInfo != null;
            }
            return Page();
        }
        private Utils.Server ParseRegion()
        {
            return (Utils.Server)Enum.Parse(typeof(Utils.Server), Region);
        }
    }
}
