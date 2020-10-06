using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class Profile : MonoBehaviour
{
    private bool loading = false;
    [SerializeField]
    public Text mensagem;
    private void HandleCamera(float x){
        float atual = Camera.main.transform.position.x;
        float max = 10f;
        float min = 0f;
        x = x + atual;
        if (min > x || x > max)
        {
            x = min;
        }
        Camera.main.transform.position = new Vector3(x,1.3f,-6f);
    }    public void Proximo(){
        HandleCamera(5f);
    }
    public void Anterior(){
        HandleCamera(-5f);                
    }
    public void To_Mapa(){
        SceneManager.LoadScene ("Mapa", LoadSceneMode.Single);
    }
    public void Submit(){
        if(loading == false){
            loading = true;
            StartCoroutine(HandleSubmit());
        }else{
            mensagem.text = "Salvando...";
        }        
    }
    IEnumerator HandleSubmit(){
        mensagem.text = "Aguarde um momento...";
        float atual = Camera.main.transform.position.x;
        string avatar = "";
        switch (atual)
        {
            case 0: avatar = "AJ"; break;
            case 5: avatar = "Amy"; break;
            default: avatar = "AJ"; break;
        }

        string token = PlayerPrefs.GetString("token", "");
        UnityWebRequest request_me = UnityWebRequest.Get (Base_API.basePath + "api/me");
        request_me.SetRequestHeader ("Authorization", "Token " + token);
        yield return request_me.SendWebRequest ();
        if (request_me.isNetworkError || request_me.isHttpError) {
            Debug.Log (request_me.error);
            SceneManager.LoadScene ("Login", LoadSceneMode.Single);
        } 
        string r_text = request_me.downloadHandler.text;
        Jogador jogador = JsonUtility.FromJson<Jogador>(r_text);

        jogador.avatar = avatar;
        string jsonString = JsonUtility.ToJson(jogador);

        UnityWebRequest request_up_avatar = UnityWebRequest.Put(Base_API.basePath + "api/jogador/"+jogador.id.ToString()+"/",jsonString);
        request_up_avatar.SetRequestHeader ("Authorization", "Token " + token);
        request_up_avatar.SetRequestHeader("Content-Type", "application/json");
        request_up_avatar.SetRequestHeader("Accept", "application/json");

        yield return request_up_avatar.SendWebRequest ();
        if (request_up_avatar.isNetworkError || request_up_avatar.isHttpError) {
            Debug.Log (request_me.error);
            SceneManager.LoadScene ("Login", LoadSceneMode.Single);
        } else {
            r_text = request_up_avatar.downloadHandler.text;
            Jogador jogador_updated = JsonUtility.FromJson<Jogador>(r_text);
            PlayerPrefs.SetString("avatar", jogador_updated.avatar);
            Debug.Log("avatar atualizado com sucesso");
            User_API.To_Mapa();
            
        }

        loading = false;
        mensagem.text = "Confirmar";
    }
}
