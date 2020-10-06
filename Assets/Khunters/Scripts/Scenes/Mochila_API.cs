using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


[System.Serializable]
public class Item_Full
{
    public Personagen personagem;
    public string quantidade;
    public string id;
}

public class Mochila_Full
{
    public Item_Full[] lista;
}

public class Mochila_API : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(get_mochila());
    }
    public void To_Mapa()
    {
        SceneManager.LoadScene("Mapa", LoadSceneMode.Single);
    }

    IEnumerator get_mochila()
    {
        string token = PlayerPrefs.GetString("token", "");

        UnityWebRequest request = UnityWebRequest.Get(Base_API.basePath + "api/mochila/");
        request.SetRequestHeader("Authorization", "Token " + token);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            // SceneManager.LoadScene("Login", LoadSceneMode.Single);
        }
        else
        {
            string r_text = request.downloadHandler.text;
            r_text = "{\"lista\":" + r_text + "}";
            Mochila_Full itens = JsonUtility.FromJson<Mochila_Full>(r_text);
            
            Canvas canvas = FindObjectOfType<Canvas>();
            float h = canvas.GetComponent<RectTransform>().rect.height;
            float w = canvas.GetComponent<RectTransform>().rect.width;

            float x = w/4;
            float y = -200;

            int count = 1;

            foreach (Item_Full item in itens.lista)
            {
                
                GameObject tempGameObject = Instantiate(Resources.Load("UI/Button_Icon", typeof(GameObject))) as GameObject;
                tempGameObject.transform.SetParent(canvas.transform);
                tempGameObject.name = item.personagem.prefab + item.id;
                tempGameObject.GetComponent<Image>().sprite = Resources.Load("UI/Icons/"+item.personagem.prefab, typeof(Sprite)) as Sprite;
                tempGameObject.transform.GetChild(0).GetComponent<Text>().text = item.personagem.descricao+"\n"+item.quantidade;
                
                RectTransform tempRTform = tempGameObject.GetComponent<RectTransform>();

                if(count % 5 == 0){
                    y -= 200;
                    count = 1;
                }

                tempRTform.anchoredPosition = new Vector2((x * count)-100f, y);
                count++;
            }
        }

    }
}
