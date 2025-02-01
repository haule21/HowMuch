using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HowMuch
{
    public static class Common
    {
        public async static Task SaveSecureStorage(string key, string val)
        {
            try
            {
                Task.Run(async () => await SecureStorage.Default.SetAsync(key, val));
            }
            catch (Exception ex)
            {
                // 예외 처리
                Console.WriteLine(ex.Message);
            }
        }

        public async static Task<string> GetSecureStorage(string key)
        {
            try
            {
                string temp = Task.Run(async () => await SecureStorage.Default.GetAsync(key)).Result;
                return temp;
            }
            catch (System.NullReferenceException ex) 
            {
                // 예외 처리
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                // 예외 처리
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool TryParseJson<T>(this string @this, out T result)
        {
            bool success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }

        public static bool IdRegex(string userId)
        {
            string pattern = "^[a-zA-Z0-9]{4,15}$";
            return Regex.IsMatch(userId, pattern);
        }
        public static bool PwRegex(string password)
        {
            string pattern = "^(?=.*[!@#$%^]).{8,64}$";
            return Regex.IsMatch(password, pattern);
        }
        public static bool EmailRegex(string email)
        {
            string pattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool NameRegex(string name)
        {
            string pattern = "^[a-zA-Z0-9가-힣]+$";
            return Regex.IsMatch(name, pattern);
        }
    }
}
