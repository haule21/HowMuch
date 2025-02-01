namespace HowMuch;

public partial class SubscriptionSettingsPage : ContentPage
{
    public SubscriptionSettingsPage()
    {
        InitializeComponent();
    }

    private async void OnCheckSubscriptionStatusClicked(object sender, EventArgs e)
    {
        // 구독 상태 확인 로직
        await DisplayAlert("구독 상태", "현재 구독 상태: 활성", "확인");
    }

    private async void OnCancelSubscriptionClicked(object sender, EventArgs e)
    {
        // 구독 해지 로직
        await DisplayAlert("구독 해지", "구독이 해지되었습니다.", "확인");
    }
}