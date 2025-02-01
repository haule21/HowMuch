using Newtonsoft.Json;

namespace HowMuch;

public partial class SourceAddPage : ContentPage
{
    public SourceAddPage()
    {
        InitializeComponent();
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: �� ����
        string sourceName = SourceName.Text;

        if (!float.TryParse(SourceAmount.Text, out float sourceAmount))
        {
            await DisplayAlert("�� ����", "�ҽ� ���� Ȯ���� �ּ���.", "Ȯ��");
            return;
        }

        SourceParam sourceParam = new SourceParam()
        {
            SourceName = sourceName,
            Amount = sourceAmount,
            PricePerUnit = null
        };

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.ADD_SOURCE, sourceParam));

        if (response.state)
        {
            await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
            MessagingCenter.Send(this, "RefreshSourceManagementPage");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            LoginController errorController = new LoginController(this);
            await errorController.DisplayMessage(response.message);
        }
    }

    private void btnCancelClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}