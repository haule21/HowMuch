using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class RecipeDetailAddPage : ContentPage
{
    string MaterialUsage = null;
    string RecipeKey;
    private class RecipeDetailAdd
    {
        public ObservableCollection<UnitKeyNameParam> Unit { get; set; }
        public ObservableCollection<IngredientKeyParam> Ingredient { get; set; }
        public IngredientKeyParam SelectedItemIngredient { get; set; }
        public UnitKeyNameParam SelectedItemUnit { get; set; }
    }
    ObservableCollection<RecipeDetailAdd> recipeDetailAdd = new ObservableCollection<RecipeDetailAdd>();
    ObservableCollection<UnitKeyNameParam> IngredientUnit = new ObservableCollection<UnitKeyNameParam>();
    ObservableCollection<IngredientKeyParam> Ingredient = new ObservableCollection<IngredientKeyParam>();
    public RecipeDetailAddPage(string recipeKey, string recipeName)
    {
        InitializeComponent();
        Load(recipeKey, recipeName);
    }
    private async void Load(string recipeKey, string recipeName)
    {
        RecipeName.Text = recipeName;
        RecipeKey = recipeKey;

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_INGREDIENT_SOURCE, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (Common.TryParseJson(response.result.ToString(), out List<IngredientKeyParam> result))
            {
                result.ForEach(data => Ingredient.Add(data));
            }
            else
            {
                await DisplayAlert("오류", "재료가 없습니다. 먼저 재료를 생성하여 주세요.", "확인");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
        }
        else
        {
            LoginController errorController = new LoginController(this);
            if (await errorController.DisplayMessage(responseMessage))
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            return;
        }

        responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_UNIT_NAME_BY_INGREDEINT_UNIT_KEY, Ingredient.First());
        if (Common.TryParseJson(responseMessage, out GetResponse response2))
        {
            if (Common.TryParseJson(response2.result.ToString(), out List<UnitKeyNameParam> result))
            {
                result.ForEach(data => IngredientUnit.Add(data));
            }
            else
            {
                await DisplayAlert("오류", "재료 단위가 없습니다.", "확인");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
        }
        else
        {
            LoginController errorController = new LoginController(this);
            if (await errorController.DisplayMessage(responseMessage))
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            return;
        }

        RecipeDetailAdd recipeDetailAdd = new RecipeDetailAdd()
        {
            Unit = IngredientUnit,
            Ingredient = Ingredient,
            SelectedItemIngredient = Ingredient.First(),
            SelectedItemUnit = IngredientUnit.Where(a => a.UnitKey == Ingredient[0].UnitKey).First()
        };
        this.recipeDetailAdd.Add(recipeDetailAdd);
        BindableLayout.SetItemsSource(RecipeDetailLayout, this.recipeDetailAdd);
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: 값 검증
        RecipeDetailAdd add = recipeDetailAdd.First();
        if (add.SelectedItemIngredient.IngredientKey == null)
        {
            await DisplayAlert("값 오류", "재료를 선택하여 주세요.", "확인");
            return;
        }

        if (add.SelectedItemUnit.UnitKey == null)
        {
            await DisplayAlert("값 오류", "사용 단위를 선택하여 주세요.", "확인");
            return;
        }

        if (MaterialUsage == null)
        {
            await DisplayAlert("값 오류", "사용량을 입력하여 주세요.", "확인");
            return;
        }

        if (!float.TryParse(MaterialUsage, out float materialUsage))
        {
            await DisplayAlert("값 오류", "사용량은 숫자만 가능합니다.", "확인");
            return;
        }

        if (await DisplayAlert("추가", "재료를 추가할까요?", "예", "아니요"))
        {
            RecipeDetailAdd recipeDetailAdd = this.recipeDetailAdd.First();
            RecipeDetailParam recipeDetailParam = new RecipeDetailParam()
            {
                RecipeKey = this.RecipeKey,
                IngredientKey = add.SelectedItemIngredient.IngredientKey,
                MaterialUsage = materialUsage,
                UnitKey = add.SelectedItemUnit.UnitKey
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.ADD_RECIPE_DETAIL, recipeDetailParam));

            if (response.state)
            {
                await DisplayAlert("저장 성공", "이전 페이지로 이동합니다.", "확인");
                WeakReferenceMessenger.Default.Send<MessageSenderRecipeDetail>(new MessageSenderRecipeDetail("RecipeDetailAdd"));
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
    }
    private void textMaterialUsageChanged(object sender, EventArgs e)
    {
        MaterialUsage = (sender as Entry).Text;
    }
    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async void pickerIngredientChanged(object sender, EventArgs e)
    {
        // 20250130 IngredientKey 앞 2자리가 S0 이면 소스 (단위 무조건 g), I0 이면 재료 (재료 단위)
        IngredientKeyParam selectedItem = (IngredientKeyParam)(sender as Picker).SelectedItem;

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_UNIT_NAME_BY_INGREDEINT_UNIT_KEY, selectedItem);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (Common.TryParseJson(response.result.ToString(), out List<UnitKeyNameParam> result))
            {
                if (result == null)
                {
                    await DisplayAlert("오류", "재료 단위가 없습니다.", "확인");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
                }
                else
                {
                    int currentCount = IngredientUnit.Count;
                    result.ForEach(data => IngredientUnit.Add(data));
                    while (currentCount > 0)
                    {
                        IngredientUnit.RemoveAt(0);
                        currentCount--;
                    }
                    RecipeDetailAdd add = recipeDetailAdd.First();
                    add.SelectedItemUnit = IngredientUnit.First();
                }
            }
            else
            {
                await DisplayAlert("오류", "재료 단위가 없습니다.", "확인");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
        }
        else
        {
            LoginController errorController = new LoginController(this);
            if (await errorController.DisplayMessage(responseMessage))
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            return;
        }
    }
}