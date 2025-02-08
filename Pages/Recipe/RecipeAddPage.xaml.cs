using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace HowMuch;

public partial class RecipeAddPage : ContentPage
{
    public RecipeAddPage()
    {
        InitializeComponent();
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: �� ����
        string recipeName = RecipeName.Text;
        if (!float.TryParse(Price.Text, out float price))
        {
            await DisplayAlert("����", "�Ǹ� ������ ���ڸ� �Է� �����մϴ�.", "Ȯ��");
            return;
        }

        RecipeParam recipeParam = new RecipeParam()
        {
            RecipeName = recipeName,
            Price = price
        };

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.ADD_RECIPE, recipeParam));

        if (response.state)
        {
            await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
            WeakReferenceMessenger.Default.Send<MessageSenderRecipe>(new MessageSenderRecipe("RecipeAdd"));
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