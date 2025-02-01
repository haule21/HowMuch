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

        // 로그인 검증 (예시로 간단한 조건 사용)
        if (!response.state)
        {
            ErrorLabel.Text = response.message;
            ErrorLabel.IsVisible = true;
        }
        else if (response.state)
        {
            // 로그인 성공 시, 사용자 정보 저장
            await Common.SaveSecureStorage("username", userId);  // 사용자 이름 저장
            await Common.SaveSecureStorage("password", password);
            await Common.SaveSecureStorage("isLogged", "true");

            ErrorLabel.IsVisible = false;
            await DisplayAlert("로그인 성공", "메인 페이지로 이동합니다.", "확인");

            // MainPage로 네비게이션
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
                // 로그인 실패 시
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