using Newtonsoft.Json;

namespace HowMuch;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void onLoginButtonClicked(object sender, EventArgs e)
    {
        string userId = UserIdEntry.Text;
        string password = PasswordEntry.Text;
        LoginParam param = new LoginParam()
        {
            UserId = userId,
            Password = password
        };

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.LOGIN, param));

        // �α��� ���� (���÷� ������ ���� ���)
        if (!response.state)
        {
            ErrorLabel.Text = response.message;
            ErrorLabel.IsVisible = true;
        }
        else if (response.state)
        {
            // �α��� ���� ��, ����� ���� ����
            await Common.SaveSecureStorage("username", userId);  // ����� �̸� ����
            await Common.SaveSecureStorage("password", password);
            await Common.SaveSecureStorage("isLogged", "true");

            ErrorLabel.IsVisible = false;
            await DisplayAlert("�α��� ����", "���� �������� �̵��մϴ�.", "Ȯ��");

            // MainPage�� �׺���̼�
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }
        else
        {
            if (Common.TryParseJson(response.message, out ErrorResponse errorResponse))
            {
                ErrorLabel.Text = errorResponse.error;
            }
            else
            {
                // �α��� ���� ��
                ErrorLabel.Text = response.message;
            }
            ErrorLabel.IsVisible = true;
        }
    }

    private async void RegisterTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async void FindPasswordTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PasswordFindPage());
    }
}