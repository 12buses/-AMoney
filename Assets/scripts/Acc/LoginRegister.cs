using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;


public class LoginRegister : MonoBehaviour
{
    public InputField _nick; //ник
    public InputField _email; // поле для ввода почты
    public InputField _pass; //поле для ввода пароля 
    public InputField _RewritePass; //поле для повторного ввода пароля 

    public string OutPass;
    public static string url;

    public Text ROUPinUnityObj; //объект с текстом объясняющий почему пароль не подошёл 

    private bool CheckPassed; //подходит ли пароль

    public List<string> ReasonOfUnsuitablePassword = new List<string>();    //текст объясняющий почему пароль не подошёл 


    public class user //класс пользователей
    {
        public string _name { get; set; }
        public string _mail { get; set; }
        public user(string name, string mail)
        {
            _name = name;
            _mail = mail;
        }
    }

    public class ourUser : user //текущий пользователь 
    {
        public string _password { get; set; }
        public bool CheckStatus { get; set; }
        public string SendType { get; set; }
        public bool CheckOfSuitblePass { get; set; }
        public ourUser(string _name, string _mail, string password, string _sendType, bool _checkStatus, bool _check)
            : base(_name, _mail)
        {
            _password = password;
            CheckStatus = _checkStatus;
            SendType = _sendType;
            CheckOfSuitblePass = _check;
        }
    }


    public void Register()//регестрация
    {
        OutPass = null;
        ROUPinUnityObj.text = "";
        CheckPassed = true; //сбрасывает значение проверки пароля на надёжность

        if (_email.text != null && _pass.text != null && _pass.text == _RewritePass.text && _nick.text != null) //проверка что поля заполнены и поля для ввода пароля совподают
        {
            StartCoroutine(NameEmailPassCheking("Reg")); //вызов проверки пароля на безопасность, почты и нейма на уникальность,  следуйший код выполниться после завершения этого

            if (CheckPassed == true) //если пароль подходит
            {
                
                
            }
            else //если пароль не подходит
            {
                foreach (var item in ReasonOfUnsuitablePassword)
                {
                    OutPass = OutPass + item + System.Environment.NewLine;
                }
                ROUPinUnityObj.text = OutPass; //вывод почему пароль не подходит
            }
        }
        else
        {
            ROUPinUnityObj.text = "*Поля ввода почты, имени либо пароля не заполнены. Пароль должен содержть цифру, быть не менне 6 символов и включать в себя заглавную и строчную букву."; //вывод ошибки
        }

    }

    public void Login()
    {
        if (_pass.text != null && _nick.text != null) //проверка что поля заполнены
        {
            StartCoroutine(NameEmailPassCheking("Login"));

            if (CheckPassed == true) //аутентификация прошла 
            {


            }
            else //аутентификация не прошла
            {
                foreach (var item in ReasonOfUnsuitablePassword)
                {
                    OutPass = OutPass + item + System.Environment.NewLine;
                }
                ROUPinUnityObj.text = OutPass;
            }
        }
        else
        {
            ReasonOfUnsuitablePassword.Add("*Поля ввода имени либо пароля не заполнены.");
        }
    }

    IEnumerator NameEmailPassCheking(string SendType) 
    {

        if (SendType == "Reg")
        {
            //проверка пароля на соответсвие требованиям 
            bool IsLetter = true;

            if (_pass.text.Length < 6)
            {
                ReasonOfUnsuitablePassword.Add("*Количество символов в пароле должно быть не меньше 6");
                CheckPassed = false;
            }


            if (_pass.text.Any(char.IsLetter) == false)
            {
                ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы");
                IsLetter = false;
                CheckPassed = false;
            }


            if (_pass.text.Any(char.IsDigit) == false)
            {
                ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 цыфру");
                CheckPassed = false;
            }


            if (_pass.text.Any(char.IsLower) == false && IsLetter == true)
            {
                ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы");
                CheckPassed = false;
            }


            if (_pass.text.Any(char.IsUpper) == false && IsLetter == true)
            {
                ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы");
                CheckPassed = false;
            }
        }

        ourUser _thisUserData = new ourUser(_nick.text, _email.text, _pass.text, SendType, CheckPassed); //создание объекта нового пользователя, серилизация в json файл и передача серверу.
        _thisUserData._password = _pass.text;
        _thisUserData._mail = _email.text;
        _thisUserData._name = _nick.text;
        _thisUserData.SendType = SendType;
        _thisUserData.CheckOfSuitblePass = CheckPassed;
        string JsonUserData = JsonConvert.SerializeObject(_thisUserData);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>() { };
        form.Add(new MultipartFormDataSection("myField", JsonUserData));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form, "json"))//отправление данных для проверки уникальности ника и почты данных у сервера
        {
            //отправляем данные
            yield return webRequest.SendWebRequest();
            // Проверяем наличие ошибок
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + webRequest.error);
            }
        }


        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))//получение данных о проверке уникальности данных пользавателя 
        {
            // Отправляем запрос
            yield return webRequest.SendWebRequest();
            // Проверяем наличие ошибок
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + webRequest.error);
            }
            else
            {
                // Получаем данные
                string jsonResult = webRequest.downloadHandler.text;
                ourUser CheckStatus = JsonConvert.DeserializeObject<ourUser>(jsonResult);
                if(CheckStatus.CheckStatus != true)
                {
                    if(SendType == "Reg")
                    {
                        ReasonOfUnsuitablePassword.Add("*Ваше имя/почта уже зарегестриравано");
                    }
                    else
                    {
                        ReasonOfUnsuitablePassword.Add("*Ваше имя или пароль неверны.");
                    }
                    CheckPassed = false;
                }
            }
        }

        yield return CheckPassed;
        StopCoroutine(NameEmailPassCheking()); //заверешение проверки => переход назад к функции
    }
}
