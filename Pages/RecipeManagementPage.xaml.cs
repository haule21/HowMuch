using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class RecipeManagementPage : ContentPage
{
    ObservableCollection<RecipeData> Recipe;
    public RecipeManagementPage()
    {
        InitializeComponent();
        Load();
        WeakReferenceMessenger.Default.Register<MessageSenderRecipe>(this, (r, m) => {
            Load();
        });
    }
    private async void Load()
    {
        RecipeView.ItemsSource = null;
        Recipe = new ObservableCollection<RecipeData>();
        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_RECIPE, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {

            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<RecipeParam> result))
                {
                    if (result == null)
                    {

                    }
                    else
                    {
                        List<RecipeParam> datas = result;
                        datas.ForEach(data => Recipe.Add(new RecipeData(data)));
                        RecipeView.ItemsSource = Recipe;
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
            if (await errorController.DisplayMessage(responseMessage))
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

    }
    private async void btnAddCliked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecipeAddPage());
    }

    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        RecipeData data = ((ListView)sender).SelectedItem as RecipeData;
        await Navigation.PushAsync(new RecipeDetailManagementPage(data));
    }
    private async void btnRefreshCliked(object sender, EventArgs e)
    {
        Load();
    }
}