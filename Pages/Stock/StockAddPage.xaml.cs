using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class StockAddPage : ContentPage
{
    ObservableCollection<RecipeData> Recipe = new ObservableCollection<RecipeData>();
    RecipeData SelectedItem;
    public StockAddPage()
    {
        InitializeComponent();
        Load();
    }

    private async void Load()
    {
        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_RECIPE, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {
                await DisplayAlert("�˸�", "�����Ǹ� ���� ����� �ּ���.", "Ȯ��");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<RecipeParam> result))
                {
                    if (result == null)
                    {
                        await DisplayAlert("�˸�", "�����Ǹ� ���� ����� �ּ���.", "Ȯ��");
                        await Application.Current.MainPage.Navigation.PopAsync();
                        return;
                    }
                    else
                    {
                        List<RecipeParam> datas = result;
                        datas.ForEach(data => Recipe.Add(new RecipeData(data)));

                        SelectedItem = Recipe.First();
                        RecipeDatas.ItemsSource = Recipe;
                        RecipeDatas.SelectedItem = SelectedItem;
                    }
                }
                else
                {
                    await DisplayAlert("����", "�߸��� �����Դϴ�.", "Ȯ��");
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

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: �� ����
        if (!int.TryParse(Quantity.Text, out int quantity))
        {
            await DisplayAlert("�˸�", "������ ���ڸ� �Է��� �����մϴ�.", "Ȯ��");
            return;
        }

        StockData stockData = new StockData(SelectedItem, quantity);

        string saveDatas = Preferences.Get("CurrentStock", null);

        if (saveDatas == null)
        {
            List<StockData> datas = new List<StockData>();
            datas.Add(stockData);
            Preferences.Set("CurrentStock", JsonConvert.SerializeObject(datas));
        }
        else
        {
            List<StockData> datas = JsonConvert.DeserializeObject<List<StockData>>(saveDatas);
            if (datas.Where(data => data.RecipeKey == stockData.RecipeKey).Any())
            {
                await DisplayAlert("�˸�", "�ߺ� �����Ǹ� ����� �� �����ϴ�.", "Ȯ��");
                return;
            }
            datas.Add(stockData);
            Preferences.Set("CurrentStock", JsonConvert.SerializeObject(datas));
        }

        await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
        MessagingCenter.Send(this, "RefreshStockManagementPage");
        await Application.Current.MainPage.Navigation.PopAsync();

    }

    private void btnCancelClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}