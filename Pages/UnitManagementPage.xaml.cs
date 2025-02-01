using System.Collections.ObjectModel;

namespace HowMuch;

public partial class UnitManagementPage : ContentPage
{
    ObservableCollection<UnitData> Units;
    public UnitManagementPage()
    {
        InitializeComponent();
        Load();
    }
    private async void Load()
    {
        UnitView.ItemsSource = null;
        Units = new ObservableCollection<UnitData>();
        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_UNIT, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {

            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<UnitParam> result))
                {
                    if (result == null)
                    {

                    }
                    else
                    {
                        List<UnitParam> datas = result;

                        while (datas.Count > 0)
                        {
                            UnitData unit = new UnitData(datas[0]);
                            datas.RemoveAt(0);

                            ObservableCollection<UnitData> childs = new ObservableCollection<UnitData>();
                            List<UnitParam> childParams = datas.Where(data => data.ParentUnitKey == unit.UnitKey).ToList();
                            if (childParams.Count > 0)
                            {
                                foreach (UnitParam childParam in childParams)
                                {
                                    childs.Add(new UnitData(childParam));
                                }
                                unit.Childs = childs;
                            }

                            Units.Add(unit);
                        }

                        UnitView.ItemsSource = Units;
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
    private async void btnAdd_Cliked(object sender, EventArgs e)
    {
        MessagingCenter.Subscribe<UnitAddPage>(this, "RefreshUnitManagementPage", (addSender) => {
            Load();
        });
        await Navigation.PushAsync(new UnitAddPage());
    }

    private async void tapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        UnitData data = ((ListView)sender).SelectedItem as UnitData;
        MessagingCenter.Subscribe<UnitModifyPage>(this, "RefreshUnitManagementPage", (modifySender) => {
            Load();
        });
        await Navigation.PushAsync(new UnitModifyPage(data));
    }
}