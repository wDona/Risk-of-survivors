using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyMeleePrefab;
    public GameObject enemyRangedPrefab;

    [Header("Area de spawn")]
    public Transform centroEstadio;
    public float radioSpawn = 200f;
    public float alturaSpawn = 115f;

    [Header("Configuracion oleadas")]
    public int enemigosPrimeraOleada = 15;
    public int incrementoPorOleada = 5;
    public int totalOleadas = 10;
    public float tiempoEntreOleadas = 5f;
    public float tiempoEntreSpawns = 0.5f;

    [Header("UI (opcional)")]
    public TextMeshProUGUI txtOleada;
    public TextMeshProUGUI txtEnemigos;

    private int oleadaActual = 0;
    private int enemigosVivos = 0;
    private bool spawning = false;

    void Start()
    {
        StartCoroutine(IniciarOleadas());
    }

    IEnumerator IniciarOleadas()
    {
        yield return new WaitForSeconds(2f);

        while (oleadaActual < totalOleadas)
        {
            oleadaActual++;
            int cantidad = enemigosPrimeraOleada + (oleadaActual - 1) * incrementoPorOleada;
            float ratioRanged = Mathf.Lerp(0f, 0.5f, (float)(oleadaActual - 1) / (totalOleadas - 1));

            ActualizarUIoleada();

            yield return StartCoroutine(SpawnOleada(cantidad, ratioRanged));

            // Espera a que mueran todos
            yield return new WaitUntil(() => enemigosVivos <= 0);

            if (oleadaActual < totalOleadas)
            {
                Debug.Log("Siguiente oleada en " + tiempoEntreOleadas + "s");
                yield return new WaitForSeconds(tiempoEntreOleadas);
            }
        }

        Debug.Log("¡Todas las oleadas completadas!");
    }

    IEnumerator SpawnOleada(int cantidad, float ratioRanged)
    {
        spawning = true;

        for (int i = 0; i < cantidad; i++)
        {
            Vector2 rnd = Random.insideUnitCircle * radioSpawn;
            Vector3 pos = new Vector3(
                centroEstadio.position.x + rnd.x,
                alturaSpawn,
                centroEstadio.position.z + rnd.y
            );
        
            GameObject prefab = Random.value < ratioRanged ? enemyRangedPrefab : enemyMeleePrefab;
            GameObject enemigo = Instantiate(prefab, pos, Quaternion.identity);
            enemigosVivos++;

            Enemy e = enemigo.GetComponent<Enemy>();
            if (e != null)
                StartCoroutine(EsperarMuerte(enemigo));

            ActualizarUIenemigos();
            yield return new WaitForSeconds(tiempoEntreSpawns);
        }

        spawning = false;
    }
    
    public void EnemyMuerto()
    {
        enemigosVivos--;
        Debug.Log("Enemigo muerto, quedan: " + enemigosVivos);
        ActualizarUIenemigos();
    }

    IEnumerator EsperarMuerte(GameObject enemigo)
    {
        yield return new WaitUntil(() => {
            Debug.Log("Spawning: " + spawning + " Vivos: " + enemigosVivos);
            return !spawning && enemigosVivos <= 0;
        });
        Debug.Log("Iniciando oleada " + (oleadaActual + 1));
        enemigosVivos--;
        Debug.Log("Enemigo muerto, quedan: " + enemigosVivos);
        ActualizarUIenemigos();
    }

    void ActualizarUIoleada()
    {
        if (txtOleada) txtOleada.text = "Oleada " + oleadaActual + " / " + totalOleadas;
    }

    void ActualizarUIenemigos()
    {
        if (txtEnemigos) txtEnemigos.text = "Enemigos: " + enemigosVivos;
    }
}