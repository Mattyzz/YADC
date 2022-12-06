using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class loadlevel : MonoBehaviour

{
    public int iLevelToLoad;

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if(collision.gameObject.CompareTag("Player"));
        { 
            Debug.Log("Box hit");
            SceneManager.LoadScene(iLevelToLoad, LoadSceneMode.Single);
        }
        
    }

    
}
