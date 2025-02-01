namespace HowMuch;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}
    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item != null)
        {
            string selectedItem = e.Item.ToString();

            // 항목에 따라 네비게이션
            if (selectedItem == "계정 설정")
            {
                await Navigation.PushAsync(new AccountSettingsPage());
            }
            else if (selectedItem == "구독 관리")
            {
                await Navigation.PushAsync(new SubscriptionSettingsPage());
            }
        }
    }
}