namespace HowMuch;

public partial class ChangePasswordPage : ContentPage
{
    public ChangePasswordPage()
    {
        InitializeComponent();
    }

    private async void ChangePasswordClicked(object sender, EventArgs e)
    {
        await DisplayAlert("준비중", "준비중인 기능입니다. 조금만 기다려주세요!", "네!");
    }
}