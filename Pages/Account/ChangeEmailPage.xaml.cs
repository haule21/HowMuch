namespace HowMuch;

public partial class ChangeEmailPage : ContentPage
{
    public ChangeEmailPage()
    {
        InitializeComponent();
    }

    private async void ChangeEmailClicked(object sender, EventArgs e)
    {
        await DisplayAlert("준비중", "준비중인 기능입니다. 조금만 기다려주세요!", "네!");
    }

    private async void ValidateEmailClicked(object sender, EventArgs e)
    {
        await DisplayAlert("준비중", "준비중인 기능입니다. 조금만 기다려주세요!", "네!");
    }

}