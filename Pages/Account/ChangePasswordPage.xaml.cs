namespace HowMuch;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
    }

    private async void ChangePasswordClicked(object sender, EventArgs e)
    {
        await DisplayAlert("�غ���", "�غ����� ����Դϴ�. ���ݸ� ��ٷ��ּ���!", "��!");
    }
}