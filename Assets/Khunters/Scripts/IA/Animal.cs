using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Animal : MonoBehaviour
{
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    GameObject personagem = GameObject.Find(hit.collider.gameObject.name);
                    personagem.transform.position = new Vector3(0, 0, 0);
                    personagem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    DontDestroyOnLoad(personagem);
                    PlayerPrefs.SetString("id_capturar", personagem.name);
                    SceneManager.LoadScene("Vuforia", LoadSceneMode.Single);
                }
            }
        }

    }
}
