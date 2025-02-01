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
            await DisplayAlert("�˸�", "���̵�� �ּ� 4�ڸ�, �ִ� 15�ڸ� �Դϴ�. (Ư������ �Ұ�)", "Ȯ��");
            return;
        }
        string password = PasswordEntry.Text;
        if (password == null || !Common.PwRegex(password))
        {
            await DisplayAlert("�˸�", "�ּ� 8�ڸ� �� Ư������ �ϳ��� �����ؾ� �մϴ�.(!@#$%^)", "Ȯ��");
            return;
        }

        string name = NameEntry.Text;
        if (name == null || !Common.NameRegex(name))
        {
            await DisplayAlert("�˸�", "�̸��� �ѱ�, ����, ���ĺ��� �����մϴ�.", "Ȯ��");
            return;
        }

        string email = CurrentEmail;
        if (email == null || !Common.EmailRegex(email))
        {
            await DisplayAlert("�˸�", "�̸����� ����� �߸��Ǿ����ϴ�.", "Ȯ��");
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
                await DisplayAlert("ȸ������ ����", response.message, "Ȯ��");
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PushAsync(new MainPage());
                });
            }
            else
            {
                await DisplayAlert("ȸ������ ����", response.message, "Ȯ��");
            }
        }
        else
        {
            await DisplayAlert("�˸�", "�̸��� ������ �Ϸ����ּ���.", "Ȯ��");
            return;
        }

    }

    private async void EmailButtonClicked(object sender, EventArgs e)
    {
        visible visibleData = visibleDatas.First();

        if (CurrentEmail != null)
        {
            await DisplayAlert("�˸�", "�ٽ� ������ �� �����ϴ�.", "Ȯ��");
            return;
        }

        if (visibleData.email == null || !Common.EmailRegex(visibleData.email))
        {
            await DisplayAlert("�˸�", "�߸��� �̸����Դϴ�.", "Ȯ��");
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

                await DisplayAlert("�˸�", "�̸��Ϸ� ���� �ڵ带 Ȯ�����ּ���.", "Ȯ��");
                return;
            }
            else
            {
                await DisplayAlert("����", "���� ���ۿ� �����߽��ϴ�. ��� �� �ٽ� �õ����ּ���.", "Ȯ��");
                return;
            }

        }
        else
        {
            if (response.message == "Duplicate")
            {
                await DisplayAlert("�ߺ� �̸���", "�ٸ� �̸����� ����Ͽ� �ּ���.", "Ȯ��");
                return;
            }
            else
            {
                await DisplayAlert("����", "���� �����Դϴ�. �����ڿ��� �����Ͽ��ּ���.", "Ȯ��");
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
                    await DisplayAlert("�˸�", "���� �Ϸ�Ǿ����ϴ�.", "Ȯ��");
                    Validate = true;
                }
                else
                {
                    await DisplayAlert("�˸�", "�߸��� �ڵ� �Դϴ�.", "Ȯ��");
                    return;
                }
            }
            else
            {
                await DisplayAlert("�˸�", "�߸��� �����Դϴ�.", "Ȯ��");
                return;
            }
        }
        else if (!Visible)
        {
            await DisplayAlert("�˸�", "�̸��� ���� ��ư�� ���� �����ּ���.", "Ȯ��");
            return;
        }
        else
        {
            await DisplayAlert("�˸�", "�̹� ���� �Ϸ�Ǿ����ϴ�.", "Ȯ��");
            return;
        }
    }
}