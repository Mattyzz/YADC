using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour

{
    public int i;

   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        

        if(collision.gameObject.CompareTag("Player"));
        { 
            Debug.Log("Box hit");
            SceneManager.LoadScene(i, LoadSceneMode.Single);
        }
        
    }

    

    
}