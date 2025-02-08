using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json;

namespace HowMuch;

public partial class RecipeModifyPage : ContentPage
{
    string OriginPrice;
    public RecipeModifyPage(RecipeData recipe)
    {
        InitializeComponent();
        RecipeKey.Text = recipe.RecipeKey;
        RecipeName.Text = recipe.RecipeName;
        ChangeRecipeName.Text = recipe.RecipeName;
        ChangePrice.Text = recipe.Price.ToString();
        OriginPrice = recipe.Price.ToString();
    }


    private async void btnSaveClicked(object sender, EventArgs e)
    {
        if (ChangeRecipeName.Text == RecipeName.Text && ChangePrice.Text == OriginPrice)
        {
            await DisplayAlert("�˸�", "��������� �����ϴ�.", "Ȯ��");
            return;
        }
        if (ChangeRecipeName.Text == "")
        {
            await DisplayAlert("�˸�", "�����Ϸ��� ������ ���� �����ϴ�.", "Ȯ��");
            return;
        }
        if (!float.TryParse(ChangePrice.Text, out float changePrice))
        {
            await DisplayAlert("�˸�", "�Ǹ� ������ ���ڸ� �Է°����մϴ�.", "Ȯ��");
            return;
        }

        if (await DisplayAlert("����", "������ ��������� �����մϱ�?", "��", "�ƴϿ�"))
        {
            RecipeParam recipeParam = new RecipeParam()
            {
                RecipeKey = RecipeKey.Text,
                RecipeName = ChangeRecipeName.Text,
                Price = changePrice
            };

            PostResponse response = JsonConvert.DeserializeObject<PostResponse>(await WebApiClient.Instance.Post(END_POINT.MODIFY_UNIT, recipeParam));

            if (response.state)
            {
                await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
                WeakReferenceMessenger.Default.Send<MessageSenderRecipe>(new MessageSenderRecipe("RecipeModify"));
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                LoginController errorController = new LoginController(this);
                await errorController.DisplayMessage(response.message);
            }
        }
        else
        {
            return;
        }
    }
    private async void btnDeleteClicked(object sender, EventArgs e)
    {

    }
    private async void btnCancelClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}