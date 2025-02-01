using Newtonsoft.Json;
using System.Net;

namespace HowMuch
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
        protected override void OnStart()
        {
            Login();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        protected async void Login()
        {
            string userId = await Common.GetSecureStorage("userId");
            string password = await Common.GetSecureStorage("password");
            if (userId != null && password != null)
            {
                LoginParam param = new LoginParam()
                {
                    UserId = userId,
                    Password = password
                };
                PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.LOGIN, param));

                if (response.state)
                {
                    await Common.SaveSecureStorage("username", userId);
                    await Common.SaveSecureStorage("password", password);
                    await Common.SaveSecureStorage("isLogged", "true");
                }
                else
                {
                    await Common.SaveSecureStorage("isLogged", "false");
                }
            }
            else
            {
                await Common.SaveSecureStorage("isLogged", "false");
            }
        }
    }
}
