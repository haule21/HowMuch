using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace HowMuch;

public partial class RecipeModifyPage : ContentPage
{
    string OriginPrice;
    public RecipeModifyPage(RecipeData recipe)
    {
        InitializeComponent();
        RecipeKey.Text = recipe.RecipeKey;
        RecipeName.Text = recipe.RecipeName;
        ChangeRecipeName.Text = recipe.RecipeName;
        ChangePrice.Text = recipe.Price.ToString();
        OriginPrice = recipe.Price.ToString();
    }


    private async void btnSaveClicked(object sender, EventArgs e)
    {
        if (ChangeRecipeName.Text == RecipeName.Text && ChangePrice.Text == OriginPrice)
        {
            await DisplayAlert("알림", "변경사항이 없습니다.", "확인");
            return;
        }
        if (ChangeRecipeName.Text == "")
        {
            await DisplayAlert("알림", "변경하려는 레시피 명이 없습니다.", "확인");
            return;
        }
        if (!float.TryParse(ChangePrice.Text, out float changePrice))
        {
            await DisplayAlert("알림", "판매 가격은 숫자만 입력가능합니다.", "확인");
            return;
        }

        if (await DisplayAlert("변경", "레시피 변경사항을 저장합니까?", "예", "아니오"))
        {
            RecipeParam recipeParam = new RecipeParam()
            {
                RecipeKey = RecipeKey.Text,
                RecipeName = ChangeRecipeName.Text,
                Price = changePrice
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.MODIFY_UNIT, recipeParam));

            if (response.state)
            {
                await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
                WeakReferenceMessenger.Default.Send<MessageSenderRecipe>(new MessageSenderRecipe("RecipeModify"));
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
        else
        {
            return;
        }
    }
    private async void btnDeleteClicked(object sender, EventArgs e)
    {

    }
    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}