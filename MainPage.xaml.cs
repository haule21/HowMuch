namespace HowMuch
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Load();
        }

        private async void Load()
        {

        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            {
                if (result)
                {
                    await Navigation.PushAsync(new SettingsPage());
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }


        private async void btnIngredientsManagement_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("btnIngredientsManagement_Clicked");
            //if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            //{
            //    if (result)
            //    {
            //        if (Boolean.TryParse(await SecureStorage.GetAsync("subscription"), out bool subscription))
            //        {
            //            if (subscription)
            //            {
            //                await Navigation.PushAsync(new IngredientManagementPage());
            //                return;
            //            }
            //            else
            //            {
            //                await Navigation.PushAsync(new SubscriptionManagementPage());
            //            }
            //        }
            //        else
            //        {
            //            await Navigation.PushAsync(new SubscriptionManagementPage()); 
            //        }
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new LoginPage());
            //    }
            //}
            //else
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}
            if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            {
                if (result)
                {
                    await Navigation.PushAsync(new IngredientManagementPage());
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }
        private async void btnRecipeManagement_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("btnRecipeManagement_Clicked");

            //if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            //{
            //    if (result)
            //    {
            //        if (Boolean.TryParse(await SecureStorage.GetAsync("subscription"), out bool subscription))
            //        {
            //            if (subscription)
            //            {
            //                await Navigation.PushAsync(new RecipeManagementPage());
            //                return;
            //            }
            //            else
            //            {
            //                await Navigation.PushAsync(new SubscriptionManagementPage());
            //            }
            //        }
            //        else
            //        {
            //            await Navigation.PushAsync(new SubscriptionManagementPage());
            //        }
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new LoginPage());
            //    }
            //}
            //else
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}

            if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            {
                if (result)
                {
                    await Navigation.PushAsync(new RecipeManagementPage());
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }
        private async void btnSourceManagement_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("btnSourceManagement_Clicked");
            //if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            //{
            //    if (result)
            //    {
            //        if (Boolean.TryParse(await SecureStorage.GetAsync("subscription"), out bool subscription))
            //        {
            //            if (subscription)
            //            {
            //                await Navigation.PushAsync(new SourceManagementPage());
            //                return;
            //            }
            //            else
            //            {
            //                await Navigation.PushAsync(new SubscriptionManagementPage());
            //            }
            //        }
            //        else
            //        {
            //            await Navigation.PushAsync(new SubscriptionManagementPage());
            //        }
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new LoginPage());
            //    }
            //}
            //else
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}   
            if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            {
                if (result)
                {
                    await Navigation.PushAsync(new SourceManagementPage());
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }

        private async void btnStockManagement_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("btnStockManagement_Clicked");

            //if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            //{
            //    if (result)
            //    {
            //        if (Boolean.TryParse(await SecureStorage.GetAsync("subscription"), out bool subscription))
            //        {
            //            if (subscription)
            //            {
            //                await Navigation.PushAsync(new StockManagementPage());
            //                return;
            //            }
            //            else
            //            {
            //                await Navigation.PushAsync(new SubscriptionManagementPage());
            //            }
            //        }
            //        else
            //        {
            //            await Navigation.PushAsync(new SubscriptionManagementPage());
            //        }
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new LoginPage());
            //    }
            //}
            //else
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}
            if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            {
                if (result)
                {
                    await Navigation.PushAsync(new StockManagementPage());
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }


        private async void btnUnitManagement_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("btnUnitManagement_Clicked");
            //if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            //{
            //    if (result)
            //    {
            //        if (Boolean.TryParse(await SecureStorage.GetAsync("subscription"), out bool subscription))
            //        {
            //            if (subscription)
            //            {
            //                await Navigation.PushAsync(new UnitManagementPage());
            //                return;
            //            }
            //            else
            //            {
            //                await Navigation.PushAsync(new SubscriptionManagementPage());
            //            }
            //        }
            //        else
            //        {
            //            await Navigation.PushAsync(new SubscriptionManagementPage());
            //        }
            //    }
            //    else
            //    {
            //        await Navigation.PushAsync(new LoginPage());
            //    }
            //}
            //else
            //{
            //    await Navigation.PushAsync(new LoginPage());
            //}
            if (Boolean.TryParse(await SecureStorage.GetAsync("isLogged"), out bool result))
            {
                if (result)
                {
                    await Navigation.PushAsync(new UnitManagementPage());
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
            else
            {
                await Navigation.PushAsync(new LoginPage());
            }
        }
    }

}
