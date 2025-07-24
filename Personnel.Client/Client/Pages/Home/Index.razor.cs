namespace Personnel.Client.Client.Pages.Home
{
    public partial class Index
    {
        bool _loader = false;


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _loader = false;
            }
        }
    }
}
