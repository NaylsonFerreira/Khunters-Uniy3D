using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class vuforia : MonoBehaviour
{
     public Transform groundPlaneStage;
     public Text mensagem;
     private GameObject personagem;
    void Start()
    {
        mensagem.text = "Aponte a câmera para uma superfície plana e aguarde o quadrado no centro da tela";
        personagem = GameObject.FindGameObjectWithTag("Personagem");
        personagem.transform.SetParent(groundPlaneStage);
    }
    IEnumerator Capturar(){
        string token = PlayerPrefs.GetString("token", "");
        string id_capturar = PlayerPrefs.GetString("id_capturar", "");
        UnityWebRequest request = UnityWebRequest.Delete (Base_API.basePath + "api/objeto_er_map/"+id_capturar+"/");
        request.SetRequestHeader ("Authorization", "Token " + token);
        yield return request.SendWebRequest ();
        if (request.isNetworkError || request.isHttpError) {
            mensagem.text = "Não foi possivel apagar";
            Debug.Log (request.error);
        } else {
            mensagem.text = "Capturado com sucesso";
            PlayerPrefs.SetString("id_capturar", "");
        }
    }
    public void FoundPlane(){
        mensagem.text = "Toque na tela";
    }
    public void NotFoundPlane(){
        mensagem.text = "Aponte a câmera para uma superfície plana e aguarde o quadrado no centro da tela";
    }
    public void voltar(){
        SceneManager.LoadScene("Mapa", LoadSceneMode.Single);
    }
    public void Open_app(){
        StartCoroutine (Capturar ());
        try
        {
        string bundleId = "net.gcompris.full"; // your target bundle id
		AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

		//if the app is installed, no errors. Else, doesn't get past next line
		AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage",bundleId);

        
            ca.Call("startActivity",launchIntent);

            up.Dispose();
            ca.Dispose();
            packageManager.Dispose();
            launchIntent.Dispose();
        }
        catch (System.Exception)
        {
            Application.OpenURL("https://play.google.com/store/apps/details?id=net.gcompris.full");
            // Debug.LogException(e, this);
        }				
    }
}
