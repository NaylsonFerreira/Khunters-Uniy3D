using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Check_GPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            SceneManager.LoadScene("Mapa", LoadSceneMode.Single);
        }
        else
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            StartCoroutine(Checar_GPS());
        }
    }

    IEnumerator Checar_GPS()
    {
        yield return new WaitForSeconds(5); // Verificação a cada 5 segundos
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            SceneManager.LoadScene("Mapa", LoadSceneMode.Single);
        }
        else
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        StartCoroutine(Checar_GPS());
    }
}
