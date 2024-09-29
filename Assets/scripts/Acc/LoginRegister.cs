using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LoginRegister : MonoBehaviour
{
    public InputField _nick; //���
    public InputField _email; // ���� ��� ����� �����
    public InputField _pass; //���� ��� ����� ������ 
    public InputField _RewritePass; //���� ��� ���������� ����� ������ 

    private string OutPass;
    private string OutEmail;
    private string OutLogin;

    public Text ROUPinUnityObj; //������ � ������� ����������� ������ ������ �� ������� 
    public Text ROULinUnityObj; //������ � ������� ����������� ������ ����� �� ������� 

    private bool CheckOfPassPassed;//�������� �� ������
    private bool CheckOfEmailPassed;//�������� �� �����
    private bool CheckOfLoginPassed;//�������� �� �����

    private List<string> ReasonOfUnsuitablePassword;    //����� ����������� ������ ������ �� ������� 
    private List<string> ReasonOfUnsuitableNick;    //����� ����������� ������ ����� �� ������� 


    public class user //����� �������������
    {
        public string _name { get; set; }
        public string _mail { get; set; }
        public user(string name, string mail)
        {
            _name = name;
            _mail = mail;
        }
    }

    public class ourUser : user //������� ������������ 
    {
        public string _password { get; set; }
        public ourUser(string _name, string _mail, string password)
            : base(_name, _mail)
        {
            _password = password;
        }
    }



    public void Register()//�����������
    {
        ReasonOfUnsuitablePassword = new List<string>();
        OutPass = null;
        ROUPinUnityObj.text = "";
        CheckOfPassPassed = true; //���������� �������� �������� ������ �� ���������

        if (_email.text != null && _pass.text != null && _pass.text == _RewritePass.text && _nick.text != null) //�������� ��� ���� ��������� � ���� ��� ����� ������ ���������
        {
            StartCoroutine(NameEmailPassCheking()); //����� �������� ������ �� ������������, ����� � ����� �� ������������,  ��������� ��� ����������� ����� ���������� �����

            if (CheckOfPassPassed == true) //���� ������ ��������
            {
                
                
            }
            else //���� ������ �� ��������
            {
                foreach (var item in ReasonOfUnsuitablePassword)
                {
                    OutPass = OutPass + item + System.Environment.NewLine;
                }
                ROUPinUnityObj.text = OutPass; //����� ������ ������ �� ��������
            }
        }
        else
        {
            ROUPinUnityObj.text = "*���� ����� �����, ����� ���� ������ �� ���������. ������ ������ �������� �����, ���� �� ����� 6 �������� � �������� � ���� ��������� � �������� �����."; //����� ������
        }

    }

    public void Login()
    {
        if (_pass.text != null && _nick.text != null) //�������� ��� ���� ���������
        {
            StartCoroutine(NameEmailPassCheking());

            if (CheckOfPassPassed == true) //�������������� ������ 
            {


            }
            else //�������������� �� ������
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
            ReasonOfUnsuitablePassword.Add("*���� ����� ����� ���� ������ �� ���������.");
        }
    }

    IEnumerator NameEmailPassCheking() 
    {    
        //�������� ������ �� ����������� ����������� 
        bool IsLetter = true;

        if (_pass.text.Length < 6 || _pass.text.Length > 15)
        {
            ReasonOfUnsuitablePassword.Add("*���������� �������� � ������ ������ ���� �� ������ 6");
            CheckOfPassPassed = false;
        }

        string _allowedCharsInPass = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789~!?@#$%^&*_-+()[]{}></\\|\"'.,:;";
        if (_pass.text.All(c => _allowedCharsInPass.Contains(c)) == false)
        {
            ReasonOfUnsuitablePassword.Add("*������ ������ ��������� ����-�� 1 �������� � 1 ��������� �����, ����� � ������ ����� �������������� ������ ��������� �����, ����� � ��� �������: ~ ! ? @ # $ % ^ & * _ - + ( ) [ ] { } > < / \\ | \" ' . , : ;.");
            IsLetter = false;
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsLetter) == false)
        {
            ReasonOfUnsuitablePassword.Add("*������ ������ ��������� ����-�� 1 �������� � 1 ��������� �����");
            IsLetter = false;
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsDigit) == false)
        {
            ReasonOfUnsuitablePassword.Add("*������ ������ ��������� ����-�� 1 �����");
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsLower) == false && IsLetter == true)
        {
            ReasonOfUnsuitablePassword.Add("*������ ������ ��������� ����-�� 1 �������� � 1 ��������� �����");
            CheckOfPassPassed = false;
        }


        if (_pass.text.Any(char.IsUpper) == false && IsLetter == true)
        {
            ReasonOfUnsuitablePassword.Add("*������ ������ ��������� ����-�� 1 �������� � 1 ��������� �����");
            CheckOfPassPassed = false;
        }


        //�������� ����� 
        if (_email.text.Length > 50)// ��������� ����� ������
        {
            CheckOfEmailPassed = false;
        }

        
        if (_email.text.IndexOf('@') != _email.text.LastIndexOf('@')) // ��������� ������� ������ ������� @
        {
            CheckOfEmailPassed = false;
        }
        else
        {
            int atIndex = _email.text.IndexOf('@');
            if (atIndex <= 0 || atIndex >= _email.text.Length - 1) // ���������, ��� �� � ����� @ ���� ���� �� ���� ���������� ������
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



        //�������� ������ 
        if (_nick.text.Length < 3 || _nick.text.Length > 15) 
        {
            CheckOfLoginPassed = false;
            ReasonOfUnsuitableNick.Add("*����� ������ ���� ������ �� 3 �� 15 ��������.");
        }

        if (!Regex.IsMatch(_nick.text, @"^[a-zA-Z0-9]{3,15}$"))
        {
            ReasonOfUnsuitableNick.Add("*����� ����� ��������� ������ ��������� ����� � �����.");
            CheckOfLoginPassed = false;
        }


        yield return CheckOfPassPassed;
        StopCoroutine(NameEmailPassCheking()); //����������� �������� => ������� ����� � �������
    
    }
}
