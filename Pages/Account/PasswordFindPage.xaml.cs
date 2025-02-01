using Newtonsoft.Json;

namespace HowMuch;

public partial class PasswordFindPage : ContentPage
{
    public PasswordFindPage()
    {
        InitializeComponent();
    }

    private async void OnResetPasswordClicked(object sender, EventArgs e)
    {
        string email = emailEntry.Text;
        string userId = userIdEntry.Text;
        if (string.IsNullOrEmpty(email))
        {
            await DisplayAlert("입력 오류", "이메일을 입력해주세요.", "확인");
            return;
        }

        if (!Common.EmailRegex(email))
        {
            await DisplayAlert("입력 오류", "이메일을 양식을 확인해주세요.", "확인");
            return;
        }
        if (string.IsNullOrEmpty(userId))
        {
            await DisplayAlert("입력 오류", "아이디를 입력해주세요.", "확인");
            return;
        }
        //if (!Common.IdRegex(userId))
        //{
        //    await DisplayAlert("입력 오류", "아", "확인");
        //    return;
        //}

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.FIND_PASSWORD, new FindPasswordParam() { UserId = userId, Email = email }));

        if (response.state)
        {
            await DisplayAlert("알림", "이메일로 임시 비밀번호를 발급했습니다.", "확인");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            LoginController errorController = new LoginController(this);
            await errorController.DisplayMessage(response.message);
        }


    }
}