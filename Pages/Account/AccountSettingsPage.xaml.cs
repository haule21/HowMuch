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

        if (await DisplayAlert("�α׾ƿ�", "������ �α׾ƿ� �Ͻðڽ��ϱ�?", "��", "�ƴϿ�"))
        {
            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.LOGOUT, null));

            if (response.state)
            {
                await Common.SaveSecureStorage("username", null);
                await Common.SaveSecureStorage("password", null);
                await Common.SaveSecureStorage("isLogged", "false");

                // MainPage�� �׺���̼�
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