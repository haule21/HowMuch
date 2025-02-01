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
            await DisplayAlert("알림", "변경사항이 없습니다.", "확인");
            return;
        }

        if (!int.TryParse(Quantity.Text, out int quantity))
        {
            await DisplayAlert("알림", "판매수량은 숫자만 입력이 가능합니다.", "확인");
            return;
        }

        string saveDatas = Preferences.Get("CurrentStock", null);

        if (saveDatas == null)
        {
            await DisplayAlert("알림", "저장된 재고를 확인할 수 없습니다.", "확인");
            await Application.Current.MainPage.Navigation.PopAsync();
            return;
        }
        else
        {
            List<StockData> datas = JsonConvert.DeserializeObject<List<StockData>>(saveDatas);
            StockData saveData = datas.Where(data => data.RecipeKey == RecipeKey).FirstOrDefault();
            if (saveData == null)
            {
                await DisplayAlert("알림", "저장된 재고를 확인할 수 없습니다.", "확인");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            saveData.Quantity = quantity;
            Preferences.Set("CurrentStock", JsonConvert.SerializeObject(datas));
        }


        await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
        MessagingCenter.Send(this, "RefreshStockManagementPage");
        await Application.Current.MainPage.Navigation.PopAsync();

    }

    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async void btnDeleteClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("알림", "정말로 삭제하시겠습니까?", "예", "아니요"))
        {
            string saveDatas = Preferences.Get("CurrentStock", null);

            if (saveDatas == null)
            {
                await DisplayAlert("알림", "저장된 재고를 확인할 수 없습니다.", "확인");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            else
            {
                List<StockData> datas = JsonConvert.DeserializeObject<List<StockData>>(saveDatas);
                StockData saveData = datas.Where(data => data.RecipeKey == RecipeKey).FirstOrDefault();
                if (saveData == null)
                {
                    await DisplayAlert("알림", "저장된 재고를 확인할 수 없습니다.", "확인");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                datas.Remove(saveData);
                Preferences.Set("CurrentStock", JsonConvert.SerializeObject(datas));
            }

            await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
            MessagingCenter.Send(this, "RefreshStockManagementPage");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}