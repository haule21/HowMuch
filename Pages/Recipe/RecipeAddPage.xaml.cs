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
        // TODO: 값 검증
        string recipeName = RecipeName.Text;
        if (!float.TryParse(Price.Text, out float price))
        {
            await DisplayAlert("에러", "판매 가격은 숫자만 입력 가능합니다.", "확인");
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
            await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
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