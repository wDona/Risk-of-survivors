using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransicionEscenas : MonoBehaviour
{
    public string nombreEscena;

    [Header("Transicion (opcional)")]
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private float transitionTime = 0.5f;
    
    // ----------------------------------------------------------------
    // Botones

    public void OnTransicionEscena()
    {
        CargarEscena(nombreEscena);
    }

    public void OnSalir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // ----------------------------------------------------------------
    // Carga de escena (con transicion opcional)

    private void CargarEscena(string nombreEscena)
    {
        if (transitionAnimator != null)
            StartCoroutine(CargarConTransicion(nombreEscena));
        else
            SceneManager.LoadScene(nombreEscena);
    }

    private IEnumerator CargarConTransicion(string nombreEscena)
    {
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nombreEscena);
    }
}