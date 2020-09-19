using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// // Imports do Mapbox
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityStandardAssets.Characters.ThirdPerson;

[System.Serializable]
public class Personagen
{
    public int id;
    public string descricao;
    public string prefab;
    public string tamanho;
    public int objeto_er;
    public string latitude;
    public string longitude;
}
[System.Serializable]
public class Jogador
{
    public int id;
    public string avatar;
    public string animacao;
    public string latitude;
    public string longitude;
}

[System.Serializable]
public class ListaPersonagens
{
    public Personagen[] personagens;
}

[System.Serializable]
public class ListaJogadores
{
    public Jogador[] jogadores;
}
public class Mapa_API : MonoBehaviour
{

    public int interval_request = 1;
    // [SerializeField]
    public AbstractMap _map;
    public Text mensagem;
    public GameObject imagen_gps;
    private GameObject player;
    private AbstractLocationProvider _locationProvider = null;
    private GameObject tempGameObject;
    void Start()
    {
        imagen_gps.SetActive(true);
        //Buscando a localização atual do player
        _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        StartCoroutine(InvokePlayer());
        StartCoroutine(Buscar_personagens_proximos());
    }
    IEnumerator Buscar_personagens_proximos()
    {
        yield return new WaitForSeconds(interval_request);
        Location currLoc = _locationProvider.CurrentLocation;
        Vector2d localizacao_atual = currLoc.LatitudeLongitude;
        string token = PlayerPrefs.GetString("token", "");
        if (localizacao_atual.x > 1 || localizacao_atual.x < -1)
        {
            imagen_gps.SetActive(false);
            mensagem.text = "Olá Mundo!";
            // mensagem.text = localizacao_atual.x.ToString() + " - " + localizacao_atual.y.ToString();
        }
        else
        {
            imagen_gps.SetActive(true);
            mensagem.text = "Procurando sinal de GPS...";
        }

        UnityWebRequest request = UnityWebRequest.Get(Base_API.basePath + "api/personagens");
        request.SetRequestHeader("Authorization", "Token " + token);
        request.SetRequestHeader("Latitude", localizacao_atual.x.ToString());
        request.SetRequestHeader("Longitude", localizacao_atual.y.ToString());
        request.SetRequestHeader("Avatar", PlayerPrefs.GetString("avatar", ""));

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            // Debug.Log (request.error);
            SceneManager.LoadScene("Login", LoadSceneMode.Single);
        }
        else
        {
            string r_text = request.downloadHandler.text;
            ListaPersonagens lista_personagens = JsonUtility.FromJson<ListaPersonagens>(r_text);
            ListaJogadores lista_jogadores = JsonUtility.FromJson<ListaJogadores>(r_text);
            // Personagen personagen = JsonUtility.FromJson<Personagen>(r_text);

            Gerencia_personagens(lista_personagens);
            Gerencia_jogadores(lista_jogadores);

            GameObject[] listaApagar = GameObject.FindGameObjectsWithTag("Apagar");
            foreach (GameObject ob in listaApagar)
            {
                Destroy(ob);
            }

        }
        StartCoroutine(Buscar_personagens_proximos()); //Recursão do metodo
    }
    void Gerencia_personagens(ListaPersonagens lista_personagens)
    {
        GameObject[] list_Personagem = GameObject.FindGameObjectsWithTag("Personagem");
        GameObject[] list_Item = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject ob in list_Personagem) { ob.tag = "Apagar"; }
        foreach (GameObject ob in list_Item) { ob.tag = "Apagar"; }

        foreach (Personagen i in lista_personagens.personagens)
        {
            float x = float.Parse(i.tamanho);
            if (x >= 10)
            {
                x = x / 10;
            }

            try
            {
                Plotar_objeto_no_mapa(i.id.ToString(), "Personagem", "Animais/" + i.prefab, i.latitude + ", " + i.longitude, x);
            }
            catch (System.Exception)
            {
                // Debug.Log("Prefab Animais/" + i.prefab + "Não encontrado");
            }

            try
            {
                Plotar_objeto_no_mapa(i.prefab, "Item", "Itens/" + i.prefab, i.latitude + ", " + i.longitude, x);
            }
            catch (System.Exception)
            {
                // Debug.Log("Prefab Itens/" + i.prefab + "Não encontrado");
            }
        }

    }
    void Gerencia_jogadores(ListaJogadores lista_jogadores)
    {
        foreach (Jogador i in lista_jogadores.jogadores)
        {
            float x = 3f;
            try
            {
                GameObject instance_check = Instantiate(Resources.Load("Personagens/Prefabs/Players/" + i.avatar, typeof(GameObject))) as GameObject;
                Destroy(instance_check);
            }
            catch (System.Exception)
            {
                i.avatar = "AJ";
            }
            Plotar_objeto_no_mapa("player_" + i.id, "Personagem", "Players/" + i.avatar, i.latitude + ", " + i.longitude, x);
        }
    }
    void Plotar_objeto_no_mapa(string name, string tag, string prefab, string coords, float tamanho) //Instancia um prefab na cena
    {

        Vector2d localization = Conversions.StringToLatLon(s: coords);
        try
        {
            tempGameObject = GameObject.Find(name);
            tempGameObject.gameObject.tag = tag;
            try
            {
                AICharacterControl myAICharacterControl = tempGameObject.gameObject.GetComponent<AICharacterControl>();
                GameObject alvo = Instantiate(Resources.Load("Personagens/Prefabs/Itens/Alvo", typeof(GameObject))) as GameObject;
                alvo.gameObject.tag = "Personagem";
                alvo.transform.localPosition = _map.GeoToWorldPosition(localization, false);
                myAICharacterControl.target = alvo.transform;
            }
            catch (System.Exception)
            {
                // Debug.Log()
            }
        }
        catch (System.Exception)
        {
            tempGameObject = Instantiate(Resources.Load("Personagens/Prefabs/" + prefab, typeof(GameObject))) as GameObject;
            tempGameObject.transform.localPosition = _map.GeoToWorldPosition(localization, false);
            tempGameObject.name = name;
            tempGameObject.gameObject.tag = tag;
            tempGameObject.gameObject.transform.localScale = new Vector3(tamanho, tamanho, tamanho);
        }

    }
    IEnumerator InvokePlayer()
    {
        string token = PlayerPrefs.GetString("token", "");
        UnityWebRequest request = UnityWebRequest.Get(Base_API.basePath + "api/me");
        request.SetRequestHeader("Authorization", "Token " + token);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            // Debug.Log (request.error);
            SceneManager.LoadScene("Login", LoadSceneMode.Single);
        }
        else
        {
            string r_text = request.downloadHandler.text;
            Jogador jogador = JsonUtility.FromJson<Jogador>(r_text);

            string avatar = jogador.avatar;
            GameObject[] old_player = GameObject.FindGameObjectsWithTag("Player");
            try
            {
                player = Instantiate(Resources.Load("Personagens/Prefabs/Players/" + avatar, typeof(GameObject))) as GameObject;
                player.name = avatar;
            }
            catch (System.Exception)
            {
                player = Instantiate(Resources.Load("Personagens/Prefabs/Players/AJ", typeof(GameObject))) as GameObject;
                player.name = "AJ";
            }

            PlayerPrefs.SetString("avatar", player.name);

            AICharacterControl myAICharacterControl = player.gameObject.GetComponent<AICharacterControl>();
            myAICharacterControl.target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
            player.transform.position = new Vector3(0, 0, 0);

            Camera.main.transform.SetParent(player.transform);
            Camera.main.transform.position = new Vector3(0, 21f, -15f);

            Camera.main.transform.eulerAngles = new Vector3(37f, 0f, 0f);

            foreach (GameObject ob in old_player)
            {
                Destroy(ob);
            }

        }

    }


}