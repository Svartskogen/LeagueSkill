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
        //Model de input
        [BindProperty(SupportsGet = true)]
        public ProfileQuery ProfileQuery { get; set; }        

        public bool ValidSearch { get; private set; } //true si es valido el parametro de busqueda
        public bool DataLoaded { get; private set; } //true si encontro el invocador

        //Model de output
        public Summoner SummonerInfo { get; set; }
        public List<LeagueEntry> Leagues { get; set; }
        public List<LeagueParsedData> LeaguesParsedData { get; set; }
        

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
        //Ejecutado apenas cargo la pagina
        //Si viene con informacion, busca ese perfil, si no es que entro por primera vez
        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(ProfileQuery.Username))
            {
                ValidSearch = true;

                //Cargo el output model.
                var api = new RiotClient(new RiotClientSettings
                {
                    ApiKey = "RGAPI-22edc51d-5a44-4be4-b5c7-c893f7448580"
                });
                SummonerInfo = Utils.GetSummonerData(api, ProfileQuery.Username,
                    ParseRegion()).Result;
                Leagues = Utils.GetLeagues(api, SummonerInfo,
                    ParseRegion()).Result; //si no se encontro invocador esto devuelve null, por eso el nullcheck de abajo
                LeaguesParsedData = new List<LeagueParsedData>();
                if(Leagues != null) 
                {
                    foreach (LeagueEntry league in Leagues)
                    {
                        LeaguesParsedData.Add(Utils.ParseLeague(league));
                    }
                }
                DataLoaded = SummonerInfo != null;
            }
            else
            {
                ValidSearch = false;
                DataLoaded = false;
            }
        }
        //Ejecutado apenas doy a submit, basicamente valida y recarga la pagina con informacion para ser procesada en OnGet.
        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(ProfileQuery.Username))
            {
                ValidSearch = true;

                return RedirectToPage("/Index", new { ProfileQuery.Username, ProfileQuery.Region });
            }
            return Page();
        }
        private Utils.Server ParseRegion()
        {
            return (Utils.Server)Enum.Parse(typeof(Utils.Server), ProfileQuery.Region);
        }
    }
}
