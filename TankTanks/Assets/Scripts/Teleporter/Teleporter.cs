using UnityEngine;
using UnityEngine.SceneManagement; 

public class EndGameOnTrigger : MonoBehaviour
{
   

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Game Over Triggered!");

            //No game over scene yet, so just log a message for now, its about speed running the game
            //SceneManager.LoadScene(gameOverScene);
            
           
        }
    }
}
