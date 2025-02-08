using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class IngredientModifyPage : ContentPage
{
    string OriginIngredientName;
    string OriginUnitKey;
    float? OriginPrice;
    float? OriginUnitValue;
    private class IngredientModify
    {
        public IngredientData IngredientData { get; set; }
        public ObservableCollection<UnitKeyNameParam> AllUnit { get; set; }
        public UnitKeyNameParam SelectedItem { get; set; }
    }
    ObservableCollection<IngredientModify> Ingredient = new ObservableCollection<IngredientModify>();
    public IngredientModifyPage(IngredientData ingredientData)
    {
        InitializeComponent();
        Load(ingredientData);
    }

    private async void Load(IngredientData ingredientData)
    {
        OriginIngredientName = ingredientData.IngredientName;
        OriginUnitKey = ingredientData.UnitKey;
        OriginUnitValue = ingredientData.UnitValue;
        OriginPrice = ingredientData.Price;
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
                    string selectedItem = ingredientData.UnitName;
                    List<UnitKeyNameParam> datas = result;

                    IngredientModify ingredientModify = new IngredientModify()
                    {
                        IngredientData = ingredientData,
                        AllUnit = new ObservableCollection<UnitKeyNameParam>(),
                        SelectedItem = datas.Where(data => data.UnitKey == ingredientData.UnitKey).First()
                    };
                    datas.ForEach(data => ingredientModify.AllUnit.Add(data));
                    Ingredient.Add(ingredientModify);
                    BindableLayout.SetItemsSource(IngredientLayout, Ingredient);
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
        IngredientModify modify = Ingredient.First();
        //string ingredientName = IngredientName;
        //string unitName = UnitName;
        //float? unitValue = null;
        //if (float.TryParse(UnitValue, out float a))
        //{
        //    unitValue = a;
        //}
        //else
        //{
        //    await DisplayAlert("값 오류", "단위 값을 확인하여주세요.(숫자만 가능합니다.)", "확인");
        //}
        //float? price = null;
        //if (float.TryParse(Price, out float b))
        //{
        //    price = a;
        //}
        //else
        //{
        //    await DisplayAlert("값 오류", "단위 값을 확인하여주세요.(숫자만 가능합니다.)", "확인");
        //}

        if (modify.IngredientData.IngredientName == OriginIngredientName && modify.IngredientData.UnitKey == OriginUnitKey && modify.IngredientData.UnitValue == OriginUnitValue && modify.IngredientData.Price == OriginPrice)
        {
            await DisplayAlert("", "변경된 정보가 없습니다.", "확인");
            return;
        }

        if (modify.IngredientData.Price == 1)
        {
            if (!await DisplayAlert("확인", "가격은 1원으로 입력됩니다. 저장하시겠습니까?", "예", "아니요"))
            {
                return;
            }
        }

        if (await DisplayAlert("확인", "저장하시겠습니까?", "예", "아니요"))
        {
            IngredientParam ingredientParam = new IngredientParam()
            {
                IngredientKey = modify.IngredientData.IngredientKey,
                IngredientName = modify.IngredientData.IngredientName,
                UnitKey = modify.IngredientData.UnitKey,
                UnitValue = modify.IngredientData.UnitValue,
                Price = modify.IngredientData.Price
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.MODIFY_INGREDIENT, ingredientParam));

            if (response.state)
            {
                await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
                WeakReferenceMessenger.Default.Send<MessageSenderIngredient>(new MessageSenderIngredient("IngredientModify"));
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
    }

    private void btnCancelClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }

}