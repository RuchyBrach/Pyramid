using gnuciDictionary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace PyramidWeb.Pages
{
    public class Pyramid_jQueryModel : PageModel
    {
        public string WordsJson { get; set; }
        public void OnGet()
        {
            var words = EnglishDictionary.GetAllWords().ToList();
            WordsJson = JsonSerializer.Serialize(words);
        }
    }
}
