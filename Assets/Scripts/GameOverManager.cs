using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void Reintentar()
    {
        SceneManager.LoadScene("Mapa1");
    }
    
    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
