using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class IngredientAddPage : ContentPage
{
    ObservableCollection<UnitKeyNameParam> AllUnit = new ObservableCollection<UnitKeyNameParam>();
    UnitKeyNameParam SelectedItem;
    public IngredientAddPage()
    {
        InitializeComponent();
        Load();
    }

    private async void Load()
    {
        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_UNIT_NAME, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (Common.TryParseJson(response.result.ToString(), out List<UnitKeyNameParam> result))
            {
                if (result == null)
                {
                    await DisplayAlert("오류", "잘못된 응답입니다.", "확인");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    List<UnitKeyNameParam> datas = result;
                    datas.ForEach(data => AllUnit.Add(data));
                    SelectedItem = datas.Where(data => data.UnitName == "g").First();
                    UnitName.ItemsSource = AllUnit;
                    UnitName.SelectedItem = SelectedItem;
                }
            }
            else
            {
                await DisplayAlert("오류", "잘못된 응답입니다.", "확인");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }
        else
        {
            LoginController errorController = new LoginController(this);
            await errorController.DisplayMessage(response.message);
        }
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: 값 검증
        string ingredientName = IngredientName.Text;
        if (!float.TryParse(UnitValue.Text, out float unitValue))
        {
            await DisplayAlert("값 오류", "단위 값을 확인해 주세요.(숫자만 가능합니다.)", "확인");
            return;
        }
        if (!float.TryParse(Price.Text, out float price))
        {
            await DisplayAlert("값 오류", "구입가격을 확인해 주세요.(숫자만 가능합니다.)", "확인");
            return;
        }


        IngredientParam ingredientParam = new IngredientParam()
        {
            IngredientName = ingredientName,
            UnitValue = unitValue,
            UnitKey = SelectedItem.UnitKey,
            Price = price
        };

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.ADD_INGREDIENT, ingredientParam));

        if (response.state)
        {
            await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
            WeakReferenceMessenger.Default.Send<MessageSenderIngredient>(new MessageSenderIngredient("IngredientAdd"));
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            if (Common.TryParseJson(response.message, out ErrorResponse errorResponse))
            {
                if (errorResponse.error.Contains("Forbidden"))
                {
                    await DisplayAlert("만료", "로그인 시간이 만료되었습니다. 로그인 화면으로 이동합니다.", "확인");
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                    return;
                }
                else
                {
                    await DisplayAlert("오류", errorResponse.error, "확인");
                }
            }
            else
            {
                await DisplayAlert("오류", response.message, "확인");
            }
        }
    }

    private void btnCancelClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}