namespace OurWebApp.Web.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Configuration;

    public class IndexModel : PageModel
    {
        private readonly IConfiguration Config;

        // Constructor that that takes IConfiguration is called on instantiation thanks to Dependency injection
        public IndexModel(IConfiguration config)
        {
            Config = config;
        }

        public void OnGet()
        {

        }
    }
}
