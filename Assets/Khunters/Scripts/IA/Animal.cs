using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Animal : MonoBehaviour
{
    void Start(){
        float tamanho = transform.localScale.x;
        if (tamanho >= 10)
        {
            tamanho = tamanho / 10;
        }

        GameObject my = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        my.transform.localScale = new Vector3(tamanho, tamanho, tamanho);;
        transform.localScale = new Vector3(1f, 1f, 1f);
        
    }
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == gameObject.name)
                {                    
                    transform.position = new Vector3(0, 0, 0);
                    transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    PlayerPrefs.SetString("id_capturar", gameObject.name);
                    if (SceneManager.GetActiveScene().name != "Vuforia")
                    {
                        DontDestroyOnLoad(gameObject);
                        SceneManager.LoadScene("Vuforia", LoadSceneMode.Single);
                    }
                }
            }
        }
    }
}
