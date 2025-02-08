using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace HowMuch;

public partial class SourceRecipeAddPage : ContentPage
{
    string MaterialUsage = null;
    string SourceKey;
    private class SourceRecipeAdd
    {
        public ObservableCollection<UnitKeyNameParam> Unit { get; set; }
        public ObservableCollection<IngredientKeyParam> Ingredient { get; set; }
        public IngredientKeyParam SelectedItemIngredient { get; set; }
        public UnitKeyNameParam SelectedItemUnit { get; set; }
    }
    ObservableCollection<SourceRecipeAdd> sourceRecipeAdd = new ObservableCollection<SourceRecipeAdd>();
    ObservableCollection<UnitKeyNameParam> IngredientUnit = new ObservableCollection<UnitKeyNameParam>();
    ObservableCollection<IngredientKeyParam> Ingredient = new ObservableCollection<IngredientKeyParam>();
    public SourceRecipeAddPage(string sourceKey, string sourceName)
    {
        InitializeComponent();
        Load(sourceKey, sourceName);
    }
    private async void Load(string sourceKey, string sourceName)
    {
        SourceName.Text = sourceName;
        SourceKey = sourceKey;

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_ALL_INGREDIENT, null);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (Common.TryParseJson(response.result.ToString(), out List<IngredientParam> result))
            {
                result.ForEach(data => Ingredient.Add(new IngredientKeyParam(data)));
            }
            else
            {
                await DisplayAlert("����", "��ᰡ �����ϴ�. ���� ��Ḧ �����Ͽ� �ּ���.", "Ȯ��");
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
                await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
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

        SourceRecipeAdd sourceRecipeAdd = new SourceRecipeAdd()
        {
            Unit = IngredientUnit,
            Ingredient = Ingredient,
            SelectedItemIngredient = Ingredient.First(),
            SelectedItemUnit = IngredientUnit.Where(a => a.UnitKey == Ingredient[0].UnitKey).First()
        };
        this.sourceRecipeAdd.Add(sourceRecipeAdd);
        BindableLayout.SetItemsSource(SourceRecipeLayout, this.sourceRecipeAdd);
    }

    private async void btnSaveClicked(object sender, EventArgs e)
    {
        // TODO: �� ����
        SourceRecipeAdd add = sourceRecipeAdd.First();
        if (add.SelectedItemIngredient.IngredientKey == null)
        {
            await DisplayAlert("�� ����", "��Ḧ �����Ͽ� �ּ���.", "Ȯ��");
            return;
        }

        if (add.SelectedItemUnit.UnitKey == null)
        {
            await DisplayAlert("�� ����", "��� ������ �����Ͽ� �ּ���.", "Ȯ��");
            return;
        }

        if (MaterialUsage == null)
        {
            await DisplayAlert("�� ����", "��뷮�� �Է��Ͽ� �ּ���.", "Ȯ��");
            return;
        }

        if (!float.TryParse(MaterialUsage, out float materialUsage))
        {
            await DisplayAlert("�� ����", "��뷮�� ���ڸ� �����մϴ�.", "Ȯ��");
            return;
        }

        if (await DisplayAlert("�߰�", "��Ḧ �߰��ұ��?", "��", "�ƴϿ�"))
        {
            SourceRecipeAdd sourceRecipeAdd = this.sourceRecipeAdd.First();
            SourceRecipeParam sourceRecipeParam = new SourceRecipeParam()
            {
                SourceKey = this.SourceKey,
                IngredientKey = add.SelectedItemIngredient.IngredientKey,
                MaterialUsage = materialUsage,
                UnitKey = add.SelectedItemUnit.UnitKey
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.ADD_SOURCE_RECIPE, sourceRecipeParam));

            if (response.state)
            {
                await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
                WeakReferenceMessenger.Default.Send<MessageSenderSourceRecipe>(new MessageSenderSourceRecipe("SourceRecipeAdd"));
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
        IngredientKeyParam selectedItem = (IngredientKeyParam)(sender as Picker).SelectedItem;

        string responseMessage = await WebApiClient.Instance.Get(END_POINT.GET_UNIT_NAME_BY_INGREDEINT_UNIT_KEY, selectedItem);
        if (Common.TryParseJson(responseMessage, out GetResponse response))
        {
            if (Common.TryParseJson(response.result.ToString(), out List<UnitKeyNameParam> result))
            {
                if (result == null)
                {
                    await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
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
                    SourceRecipeAdd add = sourceRecipeAdd.First();
                    add.SelectedItemUnit = IngredientUnit.First();
                }
            }
            else
            {
                await DisplayAlert("����", "��� ������ �����ϴ�.", "Ȯ��");
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