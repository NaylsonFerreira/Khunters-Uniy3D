using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class User_API : MonoBehaviour
{
    
    void Start()
    {

    }
    void Update()
    {
        
    }
    
    public void To_SingUp(){
        SceneManager.LoadScene ("SingUp", LoadSceneMode.Single);
    }
    public void To_Login(){
        SceneManager.LoadScene ("Login", LoadSceneMode.Single);
    }
    public void To_Mochila(){
        SceneManager.LoadScene ("Mochila", LoadSceneMode.Single);
    }
    public static void To_Mapa(){
        SceneManager.LoadScene ("Mapa", LoadSceneMode.Single);
    }
    public void To_Profile(){
        SceneManager.LoadScene ("Profile", LoadSceneMode.Single);
    }
    public void Logout(){
        PlayerPrefs.SetString("token", "");
        PlayerPrefs.Save();
        SceneManager.LoadScene ("Login", LoadSceneMode.Single);
    }

}
