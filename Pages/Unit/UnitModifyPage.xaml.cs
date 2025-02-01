using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class UnitModifyPage : ContentPage
{
    public string UnitKey;
    public string UnitName;
    public string ParentUnitKey;
    public string ParentUnitRelation;
    public string Value;
    public string Desc;
    private class UnitModify
    {
        public UnitData UnitData { get; set; }
        public ObservableCollection<UnitKeyNameParam> AllUnit { get; set; }
        public UnitKeyNameParam SelectedItem { get; set; }
    }
    ObservableCollection<UnitModify> Unit = new ObservableCollection<UnitModify>();
    public UnitModifyPage(UnitData unitData)
    {
        InitializeComponent();
        Load(unitData);
    }

    private async void Load(UnitData unitData)
    {
        UnitKey = unitData.UnitKey;
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
                    UnitName = unitData.UnitName;
                    List<UnitKeyNameParam> datas = result;

                    if (datas.Where(data => data.UnitKey == unitData.UnitKey).Any())
                    {
                        datas.Remove(datas.Where(data => data.UnitKey == unitData.UnitKey).First());
                    }

                    UnitModify unitModify = new UnitModify()
                    {
                        UnitData = unitData,
                        AllUnit = new ObservableCollection<UnitKeyNameParam>(),
                        SelectedItem = null
                    };
                    datas.ForEach(data => unitModify.AllUnit.Add(data));
                    if (unitModify.AllUnit.Where(data => data.UnitKey == unitData.ParentUnitKey).Any())
                    {
                        ParentUnitKey = unitData.ParentUnitKey;
                        unitModify.SelectedItem = unitModify.AllUnit.Where(data => data.UnitKey == unitData.ParentUnitKey).First();
                    }
                    else
                    {
                        ParentUnitKey = "None";
                        unitModify.SelectedItem = unitModify.AllUnit.Where(data => data.UnitKey == "None").First();
                    }

                    Unit.Add(unitModify);
                    BindableLayout.SetItemsSource(UnitLayout, Unit);
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
        UnitModify moidfy = Unit.First();
        string unitName = moidfy.UnitData.UnitName;
        string parentUnitKey = moidfy.UnitData.ParentUnitKey == "None" ? null : moidfy.UnitData.ParentUnitKey;

        if (!float.TryParse(moidfy.UnitData.ParentUnitRelation, out float parentUnitRelation))
        {
            await DisplayAlert("값 오류", "상위 단위 값을 확인하여주세요.(숫자만 가능합니다.)", "확인");
            return;
        }

        float? value = moidfy.UnitData.Value;
        string desc = moidfy.UnitData.Desc;

        UnitParam urlParam = new UnitParam()
        {
            UnitKey = this.UnitKey,
            UnitName = unitName,
            ParentUnitKey = parentUnitKey,
            ParentUnitRelation = parentUnitRelation,
            Value = value,
            Description = desc
        };

        PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.MODIFY_UNIT, urlParam));

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

    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}