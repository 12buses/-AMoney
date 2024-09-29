using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LoginRegister : MonoBehaviour
{
    public InputField _nick; //ник
    public InputField _email; // поле для ввода почты
    public InputField _pass; //поле для ввода пароля 
    public InputField _RewritePass; //поле для повторного ввода пароля 

    private string OutPass;
    private string OutEmail;
    private string OutLogin;

    public Text ROUPinUnityObj; //объект с текстом объясняющий почему пароль не подошёл 
    public Text ROULinUnityObj; //объект с текстом объясняющий почему логин не подошёл 

    private bool CheckOfPassPassed;//подходит ли пароль
    private bool CheckOfEmailPassed;//подходит ли почта
    private bool CheckOfLoginPassed;//подходит ли логин

    private List<string> ReasonOfUnsuitablePassword;    //текст объясняющий почему пароль не подошёл 
    private List<string> ReasonOfUnsuitableNick;    //текст объясняющий почему логин не подошёл 


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
        public ourUser(string _name, string _mail, string password)
            : base(_name, _mail)
        {
            _password = password;
        }
    }



    public void Register()//регестрация
    {
        ReasonOfUnsuitablePassword = new List<string>();
        OutPass = null;
        ROUPinUnityObj.text = "";
        CheckOfPassPassed = true; //сбрасывает значение проверки пароля на надёжность

        if (_email.text != null && _pass.text != null && _pass.text == _RewritePass.text && _nick.text != null) //проверка что поля заполнены и поля для ввода пароля совподают
        {
            StartCoroutine(NameEmailPassCheking()); //вызов проверки пароля на безопасность, почты и нейма на уникальность,  следуйший код выполниться после завершения этого

            if (CheckOfPassPassed == true) //если пароль подходит
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
            StartCoroutine(NameEmailPassCheking());

            if (CheckOfPassPassed == true) //аутентификация прошла 
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

    IEnumerator NameEmailPassCheking() 
    {    
        //проверка пароля на соответсвие требованиям 
        bool IsLetter = true;

        if (_pass.text.Length < 6 || _pass.text.Length > 15)
        {
            ReasonOfUnsuitablePassword.Add("*Количество символов в пароле должно быть не меньше 6");
            CheckOfPassPassed = false;
        }

        string _allowedCharsInPass = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!?@#$%^&*_-+()[]{}></\\|\"'.,:;";
        if (_pass.text.All(c => _allowedCharsInPass.Contains(c)) == false)
        {
            ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы, также в пароле могут использоваться только латинские буквы, цифры и эти символы: ~ ! ? @ # $ % ^ & * _ - + ( ) [ ] { } > < / \\ | \" ' . , : ;.");
            IsLetter = false;
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsLetter) == false)
        {
            ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы");
            IsLetter = false;
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsDigit) == false)
        {
            ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 цифру");
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsLower) == false && IsLetter == true)
        {
            ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы");
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsUpper) == false && IsLetter == true)
        {
            ReasonOfUnsuitablePassword.Add("*Пароль должен содержать хотя-бы 1 строчную и 1 заглавную буквы");
            CheckOfPassPassed = false;
        }


        //проверка почты 
        if (_email.text.Length > 50)// Проверяем длину адреса
        {
            CheckOfEmailPassed = false;
        }

        
        if (_email.text.IndexOf('@') != _email.text.LastIndexOf('@')) // Проверяем наличие одного символа @
        {
            CheckOfEmailPassed = false;
        }
        else
        {
            int atIndex = _email.text.IndexOf('@');
            if (atIndex <= 0 || atIndex >= _email.text.Length - 1) // Проверяем, что до и после @ есть хотя бы один допустимый символ
            {
                CheckOfEmailPassed = false;
            }
        }
        

        if (Regex.IsMatch(_email.text, @"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+$") != true)
        {
            CheckOfEmailPassed = false;
        }

        if (Regex.IsMatch(_email.text, @"\.\.") || Regex.IsMatch(_email.text, @"^[&][=][+][<][>][,][`]$"))
        {
            CheckOfEmailPassed = false;
        }

        if (_email.text.Contains(" "))
        {
            CheckOfEmailPassed = false;
        }



        //проверка логина 
        if (_nick.text.Length < 3 || _nick.text.Length > 15) 
        {
            CheckOfLoginPassed = false;
            ReasonOfUnsuitableNick.Add("*Логин должен быть длиной от 3 до 15 символов.");
        }

        if (!Regex.IsMatch(_nick.text, @"^[a-zA-Z0-9]{3,15}$"))
        {
            ReasonOfUnsuitableNick.Add("*Логин может содержать только латинские буквы и цифры.");
            CheckOfLoginPassed = false;
        }


        yield return CheckOfPassPassed;
        StopCoroutine(NameEmailPassCheking()); //заверешение проверки => переход назад к функции
    
    }
}
