using Newtonsoft.Json;

namespace HowMuch;

public partial class AccountSettingsPage : ContentPage
{
    public AccountSettingsPage()
    {
        InitializeComponent();
    }

    private async void btnChangePasswordClicked(object sender, EventArgs e)
    {
        await DisplayAlert("준비중", "준비중인 기능입니다. 조금만 기다려주세요!", "네!");
    }

    private async void btnEmailClicked(object sender, EventArgs e)
    {
        await DisplayAlert("준비중", "준비중인 기능입니다. 조금만 기다려주세요!", "네!");
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

    private async void btnDeleteAccountClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("계정 탈퇴", "정말로 계정을 삭제하시겠습니까? 관련 데이터들이 모두 삭제되고 복구할 수 없습니다.", "예", "아니요"))
        {
            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.DELETE_ACCOUNT, null));

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