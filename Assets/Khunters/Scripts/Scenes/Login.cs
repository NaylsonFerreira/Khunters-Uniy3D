using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Token {
    public string token;
}
public class Login : MonoBehaviour
{
    [SerializeField]
    public InputField email;
    public InputField password;
    public Text mensagem;
    void Start(){
        email.text =  "";
        password.text = "";
    }
    public void Submit(){
        StartCoroutine(HandleSubmit());
    }
    public void ResetPassword(){
        Application.OpenURL(Base_API.basePath + "password_reset/");
    }
    IEnumerator HandleSubmit(){
        mensagem.text = "Aguarde um momento...";
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("password", password.text);
        UnityWebRequest request = UnityWebRequest.Post(Base_API.basePath + "login/json/",form);
        yield return request.SendWebRequest ();
        if (request.isNetworkError || request.isHttpError) {
            mensagem.text = "Credenciais inválidas";
            Debug.Log (request.error);
        } else {
            string r_text = request.downloadHandler.text;
            Token token = JsonUtility.FromJson<Token>(r_text);
            PlayerPrefs.SetString("token", token.token);
            User_API.To_Mapa();
            Debug.Log(token.token);
        }
    }
}
