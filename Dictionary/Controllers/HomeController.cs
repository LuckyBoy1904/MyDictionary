using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dictionary.Models;
using Microsoft.Extensions.Configuration;
using DAL;
using BAL;

namespace Dictionary.Controllers
{
    public class HomeController : Controller
    {
        baseDL dl = new baseDL();
        private readonly ILogger<HomeController> _logger;

        //private readonly IConfiguration configuration;
        ConnectClass cn = new ConnectClass();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public List<Vocabulary> Index()
        {
            var lst = new List<Vocabulary>();
            lst = dl.GetListVocab();
            //var str1 = configuration["ConStr"];
            return lst;
        }

        //View hien
        public IActionResult hien()
        {
            var lst = new List<Vocabulary>();
            //lst = dl.GetListVocab();
            lst = cn.GetListWord("Proc_allWord");
            ViewBag.tu = lst;
            return View();
        }
        // coment Detail
        public IActionResult Detail()
        {
            return View();
        }
        //Search
        [HttpGet]
        public IActionResult Search(string word)
        {
            var l = new Vocabulary();
            l = dl.Search(word);
            ViewData["word"] = l;
            ViewBag.tim = l;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
