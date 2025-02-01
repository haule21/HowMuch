using Newtonsoft.Json;

namespace HowMuch;

public partial class StockModifyPage : ContentPage
{
    string RecipeKey;
    int OriginQuantity;
    public StockModifyPage(StockData stockData)
    {
        InitializeComponent();
        Load(stockData);
    }

    private async void Load(StockData stockData)
    {
        RecipeName.Text = stockData.RecipeName;
        RecipeKey = stockData.RecipeKey;
        OriginQuantity = stockData.Quantity;
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        if (OriginQuantity.ToString() == Quantity.Text)
        {
            await DisplayAlert("�˸�", "��������� �����ϴ�.", "Ȯ��");
            return;
        }

        if (!int.TryParse(Quantity.Text, out int quantity))
        {
            await DisplayAlert("�˸�", "�Ǹż����� ���ڸ� �Է��� �����մϴ�.", "Ȯ��");
            return;
        }

        string saveDatas = Preferences.Get("CurrentStock", null);

        if (saveDatas == null)
        {
            await DisplayAlert("�˸�", "����� ��� Ȯ���� �� �����ϴ�.", "Ȯ��");
            await Application.Current.MainPage.Navigation.PopAsync();
            return;
        }
        else
        {
            List<StockData> datas = JsonConvert.DeserializeObject<List<StockData>>(saveDatas);
            StockData saveData = datas.Where(data => data.RecipeKey == RecipeKey).FirstOrDefault();
            if (saveData == null)
            {
                await DisplayAlert("�˸�", "����� ��� Ȯ���� �� �����ϴ�.", "Ȯ��");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            saveData.Quantity = quantity;
            Preferences.Set("CurrentStock", JsonConvert.SerializeObject(datas));
        }


        await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
        MessagingCenter.Send(this, "RefreshStockManagementPage");
        await Application.Current.MainPage.Navigation.PopAsync();

    }

    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async void btnDeleteClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("�˸�", "������ �����Ͻðڽ��ϱ�?", "��", "�ƴϿ�"))
        {
            string saveDatas = Preferences.Get("CurrentStock", null);

            if (saveDatas == null)
            {
                await DisplayAlert("�˸�", "����� ��� Ȯ���� �� �����ϴ�.", "Ȯ��");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            else
            {
                List<StockData> datas = JsonConvert.DeserializeObject<List<StockData>>(saveDatas);
                StockData saveData = datas.Where(data => data.RecipeKey == RecipeKey).FirstOrDefault();
                if (saveData == null)
                {
                    await DisplayAlert("�˸�", "����� ��� Ȯ���� �� �����ϴ�.", "Ȯ��");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                datas.Remove(saveData);
                Preferences.Set("CurrentStock", JsonConvert.SerializeObject(datas));
            }

            await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
            MessagingCenter.Send(this, "RefreshStockManagementPage");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}