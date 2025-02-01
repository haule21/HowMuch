using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class RegisterPage : ContentPage
{
    string CurrentEmail = null;
    bool Visible = false;
    bool Enabled = true;
    public class visible
    {
        public string email { get; set; }
        public string emailVerifyCode { get; set; }
    }

    ObservableCollection<visible> visibleDatas = new ObservableCollection<visible>();
    bool Validate = false;
    public RegisterPage()
    {
        InitializeComponent();
        visibleDatas.Add(new visible()
        {
            email = null,
            emailVerifyCode = null
        });
        Visible = false;
        Enabled = true;

        BindableLayout.SetItemsSource(registerLayout, this.visibleDatas);
    }

    private async void onSignUpButtonClicked(object sender, EventArgs e)
    {

        // TODO: Regex
        string userId = UserIdEntry.Text;
        if (userId == null || !Common.IdRegex(userId))
        {
            await DisplayAlert("알림", "아이디는 최소 4자리, 최대 15자리 입니다. (특수문자 불가)", "확인");
            return;
        }
        string password = PasswordEntry.Text;
        if (password == null || !Common.PwRegex(password))
        {
            await DisplayAlert("알림", "최소 8자리 및 특수문자 하나를 포함해야 합니다.(!@#$%^)", "확인");
            return;
        }

        string name = NameEntry.Text;
        if (name == null || !Common.NameRegex(name))
        {
            await DisplayAlert("알림", "이름은 한글, 숫자, 알파벳만 가능합니다.", "확인");
            return;
        }

        string email = CurrentEmail;
        if (email == null || !Common.EmailRegex(email))
        {
            await DisplayAlert("알림", "이메일을 양식이 잘못되었습니다.", "확인");
            return;
        }

        if (Validate)
        {
            RegisterParam param = new RegisterParam()
            {
                UserId = userId,
                Password = password,
                Name = name,
                Email = email
            };
            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.REGISTER, param));

            if (response.state)
            {
                await DisplayAlert("회원가입 성공", response.message, "확인");
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PushAsync(new MainPage());
                });
            }
            else
            {
                await DisplayAlert("회원가입 실패", response.message, "확인");
            }
        }
        else
        {
            await DisplayAlert("알림", "이메일 검증을 완료해주세요.", "확인");
            return;
        }

    }

    private async void EmailButtonClicked(object sender, EventArgs e)
    {
        visible visibleData = visibleDatas.First();

        if (CurrentEmail != null)
        {
            await DisplayAlert("알림", "다시 검증할 수 없습니다.", "확인");
            return;
        }

        if (visibleData.email == null || !Common.EmailRegex(visibleData.email))
        {
            await DisplayAlert("알림", "잘못된 이메일입니다.", "확인");
            return;
        }

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.VALIDATE_CHECK_MAIL, new EmailParam() { Email = visibleData.email }));

        if (response.state)
        {
            response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.SEND_VERIFY_MAIL, new EmailParam() { Email = visibleData.email }));

            if (response.state)
            {
                CurrentEmail = visibleData.email;
                Enabled = false;
                Visible = true;

                await DisplayAlert("알림", "이메일로 보낸 코드를 확인해주세요.", "확인");
                return;
            }
            else
            {
                await DisplayAlert("오류", "메일 전송에 실패했습니다. 잠시 후 다시 시도해주세요.", "확인");
                return;
            }

        }
        else
        {
            if (response.message == "Duplicate")
            {
                await DisplayAlert("중복 이메일", "다른 이메일을 사용하여 주세요.", "확인");
                return;
            }
            else
            {
                await DisplayAlert("오류", "서버 오류입니다. 관리자에게 문의하여주세요.", "확인");
                return;
            }

        }


    }

    private async void EmailValidateButtonClicked(object sender, EventArgs e)
    {
        if (Visible && !Validate)
        {
            visible visibleData = visibleDatas.First();
            string responseMessage = await WebApiClient.Instance.Get(END_POINT.CHECK_MAIL_CODE, new VerifyCodeParam() { VerifyCode = visibleData.emailVerifyCode });
            if (Common.TryParseJson(responseMessage, out GetResponse response))
            {
                if (response.state)
                {
                    await DisplayAlert("알림", "검증 완료되었습니다.", "확인");
                    Validate = true;
                }
                else
                {
                    await DisplayAlert("알림", "잘못된 코드 입니다.", "확인");
                    return;
                }
            }
            else
            {
                await DisplayAlert("알림", "잘못된 응답입니다.", "확인");
                return;
            }
        }
        else if (!Visible)
        {
            await DisplayAlert("알림", "이메일 검증 버튼을 먼저 눌러주세요.", "확인");
            return;
        }
        else
        {
            await DisplayAlert("알림", "이미 검증 완료되었습니다.", "확인");
            return;
        }
    }
}