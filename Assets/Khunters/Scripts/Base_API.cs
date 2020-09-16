using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Base_API : MonoBehaviour
{
    // public static string basePath = "https://khunters-testes.herokuapp.com/";
    public static string basePath = "https://khunters.herokuapp.com/";
    // public static string basePath = "http://192.168.100.5:8000/";
    void Start()
    {
        StartCoroutine(Check_token());
    }
    IEnumerator Check_token()
    {
        yield return new WaitForSeconds (5);
        string token = PlayerPrefs.GetString("token", "");
        bool redirect = false;
        string scene = SceneManager.GetActiveScene().name;
        string[] restricted_scenes = new string[] {"Mapa", "Vuforia"};
        for (int i = 0; i < restricted_scenes.Length -1 ; i++)
        {
            if(restricted_scenes[i] == scene){
                redirect = true;
            }
        }

        if(token == "" && redirect){
            SceneManager.LoadScene ("Login", LoadSceneMode.Single);
        }
    }
}
