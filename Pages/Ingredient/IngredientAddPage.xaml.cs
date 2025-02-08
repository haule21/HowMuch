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
                    await DisplayAlert("����", "�߸��� �����Դϴ�.", "Ȯ��");
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
                await DisplayAlert("����", "�߸��� �����Դϴ�.", "Ȯ��");
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
        // TODO: �� ����
        string ingredientName = IngredientName.Text;
        if (!float.TryParse(UnitValue.Text, out float unitValue))
        {
            await DisplayAlert("�� ����", "���� ���� Ȯ���� �ּ���.(���ڸ� �����մϴ�.)", "Ȯ��");
            return;
        }
        if (!float.TryParse(Price.Text, out float price))
        {
            await DisplayAlert("�� ����", "���԰����� Ȯ���� �ּ���.(���ڸ� �����մϴ�.)", "Ȯ��");
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
            await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
            WeakReferenceMessenger.Default.Send<MessageSenderIngredient>(new MessageSenderIngredient("IngredientAdd"));
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        else
        {
            if (Common.TryParseJson(response.message, out ErrorResponse errorResponse))
            {
                if (errorResponse.error.Contains("Forbidden"))
                {
                    await DisplayAlert("����", "�α��� �ð��� ����Ǿ����ϴ�. �α��� ȭ������ �̵��մϴ�.", "Ȯ��");
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                    return;
                }
                else
                {
                    await DisplayAlert("����", errorResponse.error, "Ȯ��");
                }
            }
            else
            {
                await DisplayAlert("����", response.message, "Ȯ��");
            }
        }
    }

    private void btnCancelClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopAsync();
    }
}