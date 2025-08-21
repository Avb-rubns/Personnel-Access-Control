namespace Rubns.Infrastructure.Services
{
    internal class PlaywrightFetcher : IWebPageFetcher
    {
        public Task<string> GetBodyHtmlAsync(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetFullHtmlAsync(string url)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

            // Obtener HTML ya renderizado
            var content = await page.ContentAsync();
            return content;
        }

        public async Task<string> GetHeadHtmlAsync(string url)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

            // Obtener el contenido del <head> solamente
            var headHtml = await page.EvalOnSelectorAsync<string>("head", "head => head.innerHTML");

            return headHtml;
        }
    }
}
