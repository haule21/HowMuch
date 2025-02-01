using System.Collections.ObjectModel;

namespace HowMuch;

public partial class SourceRecipeManagementPage : ContentPage
{
    ObservableCollection<SourceRecipeData> SourceRecipe;
    string CurrentSourceKey;
    string CurrentSourceName;
    float? CurrentSourceAmount;

    public SourceRecipeManagementPage(SourceData source)
    {
        InitializeComponent();
        Load(source);
    }

    private async void Load(SourceData source)
    {
        SourceRecipeListView.ItemsSource = null;
        SourceRecipe = new ObservableCollection<SourceRecipeData>();
        CurrentSourceKey = source.SourceKey;
        CurrentSourceName = source.SourceName;
        CurrentSourceAmount = source.Amount;
        SourceName.Text = source.SourceName;
        SourceParam sourceNameParam = new SourceParam()
        {
            SourceKey = source.SourceKey,
            SourceName = source.SourceName
        };

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_SOURCE_RECIPE_INGREDIENTS, sourceNameParam);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {

            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<SourceRecipeParam> result))
                {
                    if (result == null)
                    {

                    }
                    else
                    {
                        result.ForEach(data => SourceRecipe.Add(new SourceRecipeData(data)));
                        SourceRecipeListView.ItemsSource = SourceRecipe;
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
        MessagingCenter.Subscribe<SourceModifyPage>(this, "RefreshSourceRecipeManagementPage", (addSender) => {
            Load(new SourceData() { SourceKey = CurrentSourceKey, SourceName = CurrentSourceName, PricePerUnit = null });
        });

        await Navigation.PushAsync(new SourceModifyPage(new SourceData() { SourceKey = CurrentSourceKey, SourceName = CurrentSourceName, Amount = CurrentSourceAmount }));
    }

    private async void btnAddClicked(object sender, EventArgs e)
    {
        MessagingCenter.Subscribe<SourceRecipeAddPage>(this, "RefreshSourceRecipeManagementPage", (addSender) => {
            Load(new SourceData() { SourceKey = CurrentSourceKey, SourceName = CurrentSourceName, Amount = CurrentSourceAmount });
        });

        await Navigation.PushAsync(new SourceRecipeAddPage(CurrentSourceKey, CurrentSourceName));
    }
    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        SourceRecipeData data = ((ListView)sender).SelectedItem as SourceRecipeData;
        MessagingCenter.Subscribe<SourceModifyPage>(this, "RefreshSourceRecipeManagementPage", (modifySender) => {
            Load(new SourceData() { SourceKey = CurrentSourceKey, SourceName = CurrentSourceName });
        });

        await Navigation.PushAsync(new SourceRecipeModifyPage(data));
    }
}