namespace HowMuch;

public partial class ChangeEmailPage : ContentPage
{
    public ChangeEmailPage()
    {
        InitializeComponent();
    }

    private async void ChangeEmailClicked(object sender, EventArgs e)
    {
        await DisplayAlert("�غ���", "�غ����� ����Դϴ�. ���ݸ� ��ٷ��ּ���!", "��!");
    }

    private async void ValidateEmailClicked(object sender, EventArgs e)
    {
        await DisplayAlert("�غ���", "�غ����� ����Դϴ�. ���ݸ� ��ٷ��ּ���!", "��!");
    }

}