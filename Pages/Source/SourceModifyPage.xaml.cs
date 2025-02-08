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
            await DisplayAlert("알림", "변경사항이 없습니다.", "확인");
            return;
        }
        if (SourceName.Text == "")
        {
            await DisplayAlert("알림", "변경하려는 소스명이 없습니다.", "확인");
            return;
        }
        if (!float.TryParse(ChangeSourceAmount.Text, out float changeSourceAmount))
        {
            await DisplayAlert("알림", "소스량은 숫자 입력만 가능합니다.", "확인");
            return;
        }

        if (await DisplayAlert("변경", "소스명을 " + ChangeSourceName.Text + "로 변경하시겠습니까?", "예", "아니오"))
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
                await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
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