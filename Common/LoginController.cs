using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HowMuch
{
    public class LoginController
    {
        ContentPage page;
        public LoginController(ContentPage page)
        {
            this.page = page;
        }

        public async Task<bool> DisplayMessage(string message)
        {
            if (Common.TryParseJson(message, out ErrorResponse errorResponse))
            {
                if (errorResponse.error.Contains("Forbidden"))
                {
                    await page.DisplayAlert("만료", "로그인 시간이 만료되었습니다. 로그인 화면으로 이동합니다.", "확인");
                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    return false;
                }
                else
                {
                    await page.DisplayAlert("오류", errorResponse.error, "확인");
                }
            }
            else
            {
                await page.DisplayAlert("오류", message, "확인");
            }

            return true;
        }
    }
}
