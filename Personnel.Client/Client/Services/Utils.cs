namespace Personnel.Client.Client.Services
{
    public class Utils(NavigationManager navigationManager)
    {
        private readonly NavigationManager _navigationManager = navigationManager;
        public string PathURL(string url)
        {
            string result = string.Empty;

            var uri = url.Split("qr");
            result = uri[1];


            return result;
        }
    }
}
