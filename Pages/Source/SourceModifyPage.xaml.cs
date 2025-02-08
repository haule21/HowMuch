using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace HowMuch;

public partial class SourceModifyPage : ContentPage
{
    string OriginSourceAmount;
    public SourceModifyPage(SourceData source)
    {
        InitializeComponent();
        OriginSourceAmount = source.Amount.ToString();
        ChangeSourceAmount.Text = source.Amount.ToString();
        SourceKey.Text = source.SourceKey;
        SourceName.Text = source.SourceName;
        ChangeSourceName.Text = source.SourceName;
    }


    private async void btnSaveClicked(object sender, EventArgs e)
    {
        if (ChangeSourceName.Text == SourceName.Text && ChangeSourceAmount.Text == OriginSourceAmount)
        {
            await DisplayAlert("�˸�", "��������� �����ϴ�.", "Ȯ��");
            return;
        }
        if (SourceName.Text == "")
        {
            await DisplayAlert("�˸�", "�����Ϸ��� �ҽ����� �����ϴ�.", "Ȯ��");
            return;
        }
        if (!float.TryParse(ChangeSourceAmount.Text, out float changeSourceAmount))
        {
            await DisplayAlert("�˸�", "�ҽ����� ���� �Է¸� �����մϴ�.", "Ȯ��");
            return;
        }

        if (await DisplayAlert("����", "�ҽ����� " + ChangeSourceName.Text + "�� �����Ͻðڽ��ϱ�?", "��", "�ƴϿ�"))
        {
            SourceParam sourceParam = new SourceParam()
            {
                SourceKey = SourceKey.Text,
                SourceName = ChangeSourceName.Text,
                Amount = changeSourceAmount
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.MODIFY_SOURCE, sourceParam));

            if (response.state)
            {
                await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
                WeakReferenceMessenger.Default.Send<MessageSenderSource>(new MessageSenderSource("SourceModify"));
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
        else
        {
            return;
        }
    }
    private async void btnDeleteClicked(object sender, EventArgs e)
    {

    }
    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}