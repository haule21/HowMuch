using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class StockManagementPage : ContentPage
{
    ObservableCollection<StockData> Stock;
    public StockManagementPage()
    {
        InitializeComponent();
        Load();
    }
    private async void Load()
    {
        StockView.ItemsSource = null;
        Stock = new ObservableCollection<StockData>();
        string saveStockData;
        saveStockData = Preferences.Get("CurrentStock", null);

        if (saveStockData == null)
        {

        }
        else
        {
            JsonConvert.DeserializeObject<List<StockData>>(saveStockData).ForEach(data => Stock.Add(data));
            StockView.ItemsSource = Stock;
        }
    }
    private async void btnAddCliked(object sender, EventArgs e)
    {
        MessagingCenter.Subscribe<StockAddPage>(this, "RefreshStockManagementPage", (addSender) => {
            Load();
        });
        await Navigation.PushAsync(new StockAddPage());
    }

    private async void btnResetClicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("알림", "정말로 초기화하시겠습니까?", "예", "아니요"))
        {
            Preferences.Set("CurrentStock", null);
            Load();
        }
    }

    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        StockData data = ((ListView)sender).SelectedItem as StockData;
        MessagingCenter.Subscribe<StockModifyPage>(this, "RefreshStockManagementPage", (modifySender) => {
            Load();
        });
        await Navigation.PushAsync(new StockModifyPage(data));
    }
}