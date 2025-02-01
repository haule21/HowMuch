using Newtonsoft.Json;

namespace HowMuch;

public partial class AccountSettingsPage : ContentPage
{
    public AccountSettingsPage()
    {
        InitializeComponent();
    }

    private void btnChangePasswordClicked(object sender, EventArgs e)
    {

    }

    private void btnEmailClicked(object sender, EventArgs e)
    {

    }

    private async void btnLogoutClicked(object sender, EventArgs e)
    {

        if (await DisplayAlert("로그아웃", "정말로 로그아웃 하시겠습니까?", "예", "아니요"))
        {
            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.LOGOUT, null));

            if (response.state)
            {
                await Common.SaveSecureStorage("username", null);
                await Common.SaveSecureStorage("password", null);
                await Common.SaveSecureStorage("isLogged", "false");

                // MainPage로 네비게이션
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
    }

}