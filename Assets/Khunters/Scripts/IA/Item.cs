using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class Item : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Destroy(gameObject);
            Coletar(gameObject);
        }
    }
    public static void Coletar(GameObject gObj)
    {
        Debug.Log(gObj);
        string token = PlayerPrefs.GetString("token", "");

        WWWForm form = new WWWForm();
        form.AddField("objeto_er_map", gObj.name);
        form.AddField("quantidade", 1);

        UnityWebRequest request = UnityWebRequest.Post(Base_API.basePath + "api/mochila/", form);
        request.SetRequestHeader("Authorization", "Token " + token);
        request.SendWebRequest();
    }
}
