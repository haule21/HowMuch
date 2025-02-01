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

            // �׸� ���� �׺���̼�
            if (selectedItem == "���� ����")
            {
                await Navigation.PushAsync(new AccountSettingsPage());
            }
            else if (selectedItem == "���� ����")
            {
                await Navigation.PushAsync(new SubscriptionSettingsPage());
            }
        }
    }
}