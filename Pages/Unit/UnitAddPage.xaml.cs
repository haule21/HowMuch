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
                    await DisplayAlert("����", "�߸��� �����Դϴ�.", "Ȯ��");
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
                await DisplayAlert("����", "�߸��� �����Դϴ�.", "Ȯ��");
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
        // TODO: �� ����

        string unitName = UnitName.Text;
        if (unitName == ((UnitKeyNameParam)ParentUnit.SelectedItem).UnitName)
        {
            await DisplayAlert("����", "���� ������ ������ ������ �� �����ϴ�.", "Ȯ��");
            return;
        }
        string parentUnitKey = ((UnitKeyNameParam)ParentUnit.SelectedItem).UnitKey == "None" ? null : ((UnitKeyNameParam)ParentUnit.SelectedItem).UnitKey;
        if (!float.TryParse(ParentUnitRelation.Text, out float parentUnitRelation))
        {
            await DisplayAlert("�� ����", "���� ���� ���� Ȯ���� �ּ���.(���ڸ� �����մϴ�.)", "Ȯ��");
            return;
        }
        if (!float.TryParse(Value.Text, out float value))
        {
            await DisplayAlert("�� ����", "���� ���� Ȯ���� �ּ���..(���ڸ� �����մϴ�.)", "Ȯ��");
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
            await DisplayAlert("���� ����", "���� �������� �̵��մϴ�.", "Ȯ��");
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