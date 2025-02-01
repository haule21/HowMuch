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
        // TODO: 값 검증
        string sourceName = SourceName.Text;

        if (!float.TryParse(SourceAmount.Text, out float sourceAmount))
        {
            await DisplayAlert("값 에러", "소스 량을 확인해 주세요.", "확인");
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
            await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
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