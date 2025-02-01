using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class RecipeDetailModifyPage : ContentPage
{
    string RecipeDetailKey;
    string OriginIngredientKey = null;
    string OriginUnitKey = null;
    string OriginMaterialUsage = null;
    string MaterialUsage = null;
    string RecipeKey;
    private class RecipeDetailModify
    {
        public RecipeDetailData RecipeDetailData { get; set; }
        public ObservableCollection<UnitKeyNameParam> Unit { get; set; }
        public ObservableCollection<IngredientKeyParam> Ingredient { get; set; }
        public IngredientKeyParam SelectedItemIngredient { get; set; }
        public UnitKeyNameParam SelectedItemUnit { get; set; }
    }
    ObservableCollection<RecipeDetailModify> recipeDetailModify = new ObservableCollection<RecipeDetailModify>();
    ObservableCollection<UnitKeyNameParam> IngredientUnit = new ObservableCollection<UnitKeyNameParam>();
    ObservableCollection<IngredientKeyParam> Ingredient = new ObservableCollection<IngredientKeyParam>();
    public RecipeDetailModifyPage(RecipeDetailData recipeDetailData)
    {
        InitializeComponent();
        Load(recipeDetailData);
    }

    private async void Load(RecipeDetailData recipeDetailData)
    {
        RecipeKey = recipeDetailData.RecipeKey;
        RecipeDetailKey = recipeDetailData.RecipeDetailKey;
        OriginIngredientKey = recipeDetailData.IngredientKey;
        OriginMaterialUsage = recipeDetailData.MaterialUsage.ToString();
        OriginUnitKey = recipeDetailData.UnitKey;
        MaterialUsage = recipeDetailData.MaterialUsage.ToString();

        RecipeDetailModify recipeDetailModify = new RecipeDetailModify()
        {
            RecipeDetailData = new RecipeDetailData()
            {
                RecipeName = recipeDetailData.RecipeName,
                IngredientName = recipeDetailData.IngredientName,
                MaterialUsage = recipeDetailData.MaterialUsage,
                UnitName = recipeDetailData.UnitName
            },
            Unit = null,
            Ingredient = null,
            SelectedItemIngredient = null,
            SelectedItemUnit = null
        };

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_INGREDIENT_SOURCE, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {
                await DisplayAlert("����", "��� ���� �߰��� �ּ���.", "Ȯ��");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<IngredientParam> result))
                {
                    result.ForEach(data => this.Ingredient.Add(new IngredientKeyParam(data)));
                    recipeDetailModify.Ingredient = this.Ingredient;
                }
                else
                {
                    await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
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
        recipeDetailModify.SelectedItemIngredient = Ingredient.Where(data => data.IngredientKey == OriginIngredientKey).First();
        responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_UNIT_NAME_BY_INGREDEINT_UNIT_KEY, recipeDetailModify.SelectedItemIngredient);
        if (Common.TryParseJson(responseMessage, out GetResponse response2))
        {
            if (response.result == null)
            {
                await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                if (Common.TryParseJson(response2.result.ToString(), out List<UnitKeyNameParam> result))
                {
                    result.ForEach(data => IngredientUnit.Add(data));
                    recipeDetailModify.Unit = this.IngredientUnit;
                }
                else
                {
                    await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
                    await Application.Current.MainPage.Navigation.PopAsync();
                    return;
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
            return;
        }
        recipeDetailModify.SelectedItemUnit = IngredientUnit.Where(data => data.UnitKey == OriginUnitKey).First();
        RecipeName.Text = recipeDetailData.RecipeName;

        this.recipeDetailModify.Add(recipeDetailModify);
        BindableLayout.SetItemsSource(RecipeDetailLayout, this.recipeDetailModify);
    }

    private async void btnDeleteClicked(object sender, EventArgs e)
    {

    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: �� ����
        RecipeDetailModify modify = recipeDetailModify.First();
        if (modify.SelectedItemIngredient.IngredientKey == OriginIngredientKey && modify.SelectedItemUnit.UnitKey == OriginUnitKey && MaterialUsage == OriginMaterialUsage)
        {
            await DisplayAlert("�� ����", " ��������� �����ϴ�.", "Ȯ��");
            return;
        }

        if (!float.TryParse(MaterialUsage, out float materialUsage))
        {
            await DisplayAlert("�� ����", "��� ��뷮�� Ȯ�����ּ���.(���ڸ� �����մϴ�.)", "Ȯ��");
            return;
        }

        if (await DisplayAlert("����", "������ �����ұ��?", "��", "�ƴϿ�"))
        {
            SourceRecipeParam sourceRecipeParam = new SourceRecipeParam()
            {
                SourceKey = this.RecipeKey,
                SourceRecipeKey = this.RecipeDetailKey,
                IngredientKey = modify.SelectedItemIngredient.IngredientKey,
                MaterialUsage = materialUsage,
                UnitKey = modify.SelectedItemIngredient.UnitKey
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.MODIFY_SOURCE_RECIPE, sourceRecipeParam));

            if (response.state)
            {
                await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
                MessagingCenter.Send(this, "RefreshRecipeDetailManagementPage");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
    }
    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
    private async void textMaterialUsageChanged(object sender, EventArgs e)
    {
        MaterialUsage = (sender as Entry).Text;
    }
    private async void pickerIngredientChanged(object sender, EventArgs e)
    {
        IngredientKeyParam selectedItem = (IngredientKeyParam)(sender as Picker).SelectedItem;

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_UNIT_NAME_BY_INGREDEINT_UNIT_KEY, selectedItem);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (response.result == null)
            {
                await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                if (Common.TryParseJson(response.result.ToString(), out List<UnitKeyNameParam> result))
                {
                    int currentCount = IngredientUnit.Count;
                    result.ForEach(data => IngredientUnit.Add(data));
                    while (currentCount > 0)
                    {
                        IngredientUnit.RemoveAt(0);
                        currentCount--;
                    }
                    RecipeDetailModify modify = recipeDetailModify.First();
                    modify.SelectedItemUnit = IngredientUnit.First();
                }
                else
                {
                    await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
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
}