namespace HowMuch;

public partial class SubscriptionSettingsPage : ContentPage
{
    public SubscriptionSettingsPage()
    {
        InitializeComponent();
    }

    private async void OnCheckSubscriptionStatusClicked(object sender, EventArgs e)
    {
        // ���� ���� Ȯ�� ����
        await DisplayAlert("���� ����", "���� ���� ����: Ȱ��", "Ȯ��");
    }

    private async void OnCancelSubscriptionClicked(object sender, EventArgs e)
    {
        // ���� ���� ����
        await DisplayAlert("���� ����", "������ �����Ǿ����ϴ�.", "Ȯ��");
    }
}