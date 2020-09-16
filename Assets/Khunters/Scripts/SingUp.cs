using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class SingUp : MonoBehaviour
{
    [SerializeField]
    public InputField email;
    public InputField password;
    public InputField password2;
    public Text mensagem;
    void Start(){
        email.text = "";
        password.text = "";
        password2.text = "";
    }
    public void Submit(){
        if(password.text == password2.text && password.text != ""){
            StartCoroutine(HandleSubmit());
        }else{
            mensagem.text = "As senhas devem ser validas e iguais";
        }
        
    }
    IEnumerator HandleSubmit(){
        mensagem.text = "Aguarde um momento...";
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("password", password.text);
        UnityWebRequest request = UnityWebRequest.Post(Base_API.basePath + "singup/json/",form);
        yield return request.SendWebRequest ();
        if (request.isNetworkError || request.isHttpError) {
            mensagem.text = "Falha, verifique suas informações";
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
