using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class IngredientManagementPage : ContentPage
{
    ObservableCollection<IngredientData> Ingredients;
    public IngredientManagementPage()
    {
        InitializeComponent();
        Load();
        WeakReferenceMessenger.Default.Register<MessageSenderIngredient>(this, (r, m) => {
            Load();
        });
    }

    private async void Load()
    {
        IngredientView.ItemsSource = null;
        Ingredients = new ObservableCollection<IngredientData>();
        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_INGREDIENT, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {

            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<IngredientParam> result))
                {
                    if (result == null)
                    {

                    }
                    else
                    {
                        List<IngredientParam> datas = result;

                        datas.ForEach(data => Ingredients.Add(new IngredientData(data)));
                        IngredientView.ItemsSource = Ingredients;
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
    private async void btnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new IngredientAddPage());
    }

    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        IngredientData data = ((ListView)sender).SelectedItem as IngredientData;
        await Navigation.PushAsync(new IngredientModifyPage(data));
    }
}