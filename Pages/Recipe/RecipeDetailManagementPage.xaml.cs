using System.Collections.ObjectModel;

namespace HowMuch;

public partial class RecipeDetailManagementPage : ContentPage
{
    ObservableCollection<RecipeDetailData> RecipeDetail;
    string CurrentRecipeKey;
    string CurrentRecipeName;
    float? CurrentRecipePrice;

    public RecipeDetailManagementPage(RecipeData recipe)
    {
        InitializeComponent();
        Load(recipe);
    }

    private async void Load(RecipeData recipe)
    {
        RecipeDetailListView.ItemsSource = null;
        RecipeDetail = new ObservableCollection<RecipeDetailData>();
        CurrentRecipeKey = recipe.RecipeKey;
        CurrentRecipeName = recipe.RecipeName;
        CurrentRecipePrice = recipe.Price;
        RecipeName.Text = recipe.RecipeName;
        RecipeParam recipeParam = new RecipeParam()
        {
            RecipeKey = recipe.RecipeKey
        };

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_RECIPE_DETAIL_INGREDIENTS, recipeParam);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {

            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<RecipeDetailParam> result))
                {
                    if (result == null)
                    {

                    }
                    else
                    {
                        result.ForEach(data => RecipeDetail.Add(new RecipeDetailData(data)));
                        RecipeDetailListView.ItemsSource = RecipeDetail;
                    }
                }
                else
                {
                    await DisplayAlert("오류", "잘못된 응답입니다.", "확인");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }
        else
        {
            LoginController errorController = new LoginController(this);
            await errorController.DisplayMessage(responseMessage);
        }
    }
    private async void btnModifyClicked(object sender, EventArgs e)
    {
        MessagingCenter.Subscribe<RecipeModifyPage>(this, "RefreshRecipeDetailManagementPage", (addSender) => {
            Load(new RecipeData() { RecipeKey = CurrentRecipeKey, RecipeName = CurrentRecipeName });
        });

        await Navigation.PushAsync(new RecipeModifyPage(new RecipeData() { RecipeKey = CurrentRecipeKey, RecipeName = CurrentRecipeName, Price = CurrentRecipePrice }));
    }

    private async void btnAddClicked(object sender, EventArgs e)
    {
        MessagingCenter.Subscribe<RecipeDetailAddPage>(this, "RefreshRecipeDetailManagementPage", (addSender) => {
            Load(new RecipeData() { RecipeKey = CurrentRecipeKey, RecipeName = CurrentRecipeName });
        });

        await Navigation.PushAsync(new RecipeDetailAddPage(CurrentRecipeKey, CurrentRecipeName));
    }
    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        RecipeDetailData data = ((ListView)sender).SelectedItem as RecipeDetailData;
        MessagingCenter.Subscribe<RecipeDetailModifyPage>(this, "RefreshRecipeDetailManagementPage", (modifySender) => {
            Load(new RecipeData() { RecipeKey = CurrentRecipeKey, RecipeName = CurrentRecipeName });
        });

        await Navigation.PushAsync(new RecipeDetailModifyPage(data));
    }
}