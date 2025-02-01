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
            await DisplayAlert("�Է� ����", "�̸����� �Է����ּ���.", "Ȯ��");
            return;
        }

        if (!Common.EmailRegex(email))
        {
            await DisplayAlert("�Է� ����", "�̸����� ����� Ȯ�����ּ���.", "Ȯ��");
            return;
        }
        if (string.IsNullOrEmpty(userId))
        {
            await DisplayAlert("�Է� ����", "���̵� �Է����ּ���.", "Ȯ��");
            return;
        }
        //if (!Common.IdRegex(userId))
        //{
        //    await DisplayAlert("�Է� ����", "��", "Ȯ��");
        //    return;
        //}

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.FIND_PASSWORD, new FindPasswordParam() { UserId = userId, Email = email }));

        if (response.state)
        {
            await DisplayAlert("�˸�", "�̸��Ϸ� �ӽ� ��й�ȣ�� �߱��߽��ϴ�.", "Ȯ��");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            LoginController errorController = new LoginController(this);
            await errorController.DisplayMessage(response.message);
        }


    }
}