using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class SourceManagementPage : ContentPage
{
    ObservableCollection<SourceData> Source;
    public SourceManagementPage()
    {
        InitializeComponent();
        Load();
        WeakReferenceMessenger.Default.Register<MessageSenderSource>(this, (r, m) => {
            Load();
        });
    }
    private async void Load()
    {
        SourceView.ItemsSource = null;
        Source = new ObservableCollection<SourceData>();
        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_SOURCE, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {

            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<SourceParam> result))
                {
                    if (result == null)
                    {

                    }
                    else
                    {
                        List<SourceParam> datas = result;
                        datas.ForEach(data => Source.Add(new SourceData(data)));
                        SourceView.ItemsSource = Source;
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
        await Navigation.PushAsync(new SourceAddPage());
    }

    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        SourceData data = ((ListView)sender).SelectedItem as SourceData;
        await Navigation.PushAsync(new SourceRecipeManagementPage(data));
    }
    private async void btnRefreshCliked(object sender, EventArgs e)
    {
        Load();
    }
}