using Newtonsoft.Json;

namespace HowMuch;

public partial class AccountSettingsPage : ContentPage
{
    public AccountSettingsPage()
    {
        InitializeComponent();
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            string selectedItem = e.Item.ToString();

            // �׸� ���� �׺���̼�
            if (selectedItem == "�н����� ����")
            {
                await DisplayAlert("�غ���", "�غ����� ����Դϴ�. ���ݸ� ��ٷ��ּ���!", "��!");
            }
            else if (selectedItem == "�̸��� ����")
            {
                await DisplayAlert("�غ���", "�غ����� ����Դϴ�. ���ݸ� ��ٷ��ּ���!", "��!");
            }
            else if (selectedItem == "�α׾ƿ�")
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
            else if (selectedItem == "����Ż��")
            {
                if (await DisplayAlert("���� Ż��", "������ ������ �����Ͻðڽ��ϱ�? ���� �����͵��� ��� �����ǰ� ������ �� �����ϴ�.", "��", "�ƴϿ�"))
                {
                    PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.DELETE_ACCOUNT, null));

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
    }

}