using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Newtonsoft.Json;

public class ServerSpeaking : MonoBehaviour
{

    public InputField _nick; //���
    public InputField _email; // ���� ��� ����� �����
    public InputField _pass; //���� ��� ����� ������ 
    public InputField _RewritePass; //���� ��� ���������� ����� ������ 

    public string OutPass;
    public static string url;

    public Text ROUPinUnityObj; //������ � ������� ����������� ������ ������ �� ������� 

    private bool CheckPassed; //�������� �� ������

    public List<string> ReasonOfUnsuitablePassword = new List<string>();    //����� ����������� ������ ������ �� ������� 


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

    public void start()
    {

    }
    /*
    IEnumerator NameEmailPassCheking(string SendType)
    {
        ourUser _thisUserData = new ourUser(_nick.text, _email.text, _pass.text, SendType, CheckPassed); //�������� ������� ������ ������������, ����������� � json ���� � �������� �������.
        _thisUserData._password = _pass.text;
        _thisUserData._mail = _email.text;
        _thisUserData._name = _nick.text;
        _thisUserData.SendType = SendType;
        _thisUserData.CheckOfSuitblePass = CheckPassed;
        string JsonUserData = JsonConvert.SerializeObject(_thisUserData);
        List<IMultipartFormSection> form = new List<IMultipartFormSection>() { };
        form.Add(new MultipartFormDataSection("myField", JsonUserData));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form, "json"))//����������� ������ ��� �������� ������������ ���� � ����� ������ � �������
        {
            //���������� ������
            yield return webRequest.SendWebRequest();
            // ��������� ������� ������
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("������: " + webRequest.error);
            }
        }


        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))//��������� ������ � �������� ������������ ������ ������������ 
        {
            // ���������� ������
            yield return webRequest.SendWebRequest();
            // ��������� ������� ������
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("������: " + webRequest.error);
            }
            else
            {
                // �������� ������
                string jsonResult = webRequest.downloadHandler.text;
                ourUser CheckStatus = JsonConvert.DeserializeObject<ourUser>(jsonResult);
                if (CheckStatus.CheckStatus != true)
                {
                    if (SendType == "Reg")
                    {
                        ReasonOfUnsuitablePassword.Add("*���� ���/����� ��� ����������������");
                    }
                    else
                    {
                        ReasonOfUnsuitablePassword.Add("*���� ��� ��� ������ �������.");
                    }
                    CheckPassed = false;
                }
            }
        }
        
        yield return CheckPassed;
        StopCoroutine(NameEmailPassCheking()); //����������� �������� => ������� ����� � �������
    }
    */
}
