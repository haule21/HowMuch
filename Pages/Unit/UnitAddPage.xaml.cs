using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class UnitAddPage : ContentPage
{
    ObservableCollection<UnitKeyNameParam> AllUnit = new ObservableCollection<UnitKeyNameParam>();
    UnitKeyNameParam SelectedItem = new UnitKeyNameParam()
    {
        UnitKey = "None",
        UnitName = "None"
    };
    public UnitAddPage()
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
                    result.Add(new UnitKeyNameParam()
                    {
                        UnitKey = "None",
                        UnitName = "None"
                    });
                    List<UnitKeyNameParam> datas = result;
                    datas.ForEach(data => AllUnit.Add(data));

                    ParentUnit.ItemsSource = AllUnit;
                    ParentUnit.SelectedItem = datas.Where(data => data.UnitKey == "None").First();
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
            await errorController.DisplayMessage(responseMessage);
        }
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: 값 검증

        string unitName = UnitName.Text;
        if (unitName == ((UnitKeyNameParam)ParentUnit.SelectedItem).UnitName)
        {
            await DisplayAlert("오류", "상위 단위와 단위가 동일할 수 없습니다.", "확인");
            return;
        }
        string parentUnitKey = ((UnitKeyNameParam)ParentUnit.SelectedItem).UnitKey == "None" ? null : ((UnitKeyNameParam)ParentUnit.SelectedItem).UnitKey;
        if (!float.TryParse(ParentUnitRelation.Text, out float parentUnitRelation))
        {
            await DisplayAlert("값 오류", "상위 단위 값을 확인해 주세요.(숫자만 가능합니다.)", "확인");
            return;
        }
        if (!float.TryParse(Value.Text, out float value))
        {
            await DisplayAlert("값 오류", "단위 값을 확인해 주세요..(숫자만 가능합니다.)", "확인");
            return;
        }
        string desc = Desc.Text;

        UnitParam unitParam = new UnitParam()
        {
            UnitName = unitName,
            ParentUnitKey = parentUnitKey,
            ParentUnitRelation = parentUnitRelation,
            Value = value,
            Description = desc
        };

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.ADD_UNIT, unitParam));

        if (response.state)
        {
            await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
            MessagingCenter.Send(this, "RefreshUnitManagementPage");
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            LoginController errorController = new LoginController(this);
            await errorController.DisplayMessage(response.message);
        }
    }

    private void btnCancelClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}