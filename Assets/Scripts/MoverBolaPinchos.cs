using UnityEngine;

public class MoverBolaPinchos : MonoBehaviour
{
    public Transform[] waypoints;
    public float velocidad = 300f;
    public float radioWaypoint = 50;
    public Transform spawnBola;

    private Rigidbody rb;
    private int indexActual;
    private bool esperandoSalir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.position = spawnBola.position;
        indexActual = waypoints.Length - 1;
        esperandoSalir = false;
    }

    void FixedUpdate()
    {
        if (waypoints.Length == 0) return;

        Transform objetivo = waypoints[indexActual];
        Vector3 posFlat = new Vector3(objetivo.position.x, transform.position.y, objetivo.position.z);
        Vector3 direccion = (posFlat - transform.position).normalized;
        float distancia = Vector3.Distance(transform.position, posFlat);
        
        Vector3 pos = transform.position;
        pos.y = spawnBola.position.y; // usa Y del spawn como referencia
        transform.position = pos;

        // siempre empuja
        Vector3 fuerzaFlat = new Vector3(direccion.x, 0, direccion.z);
        rb.AddForce(fuerzaFlat * velocidad, ForceMode.Acceleration);

        // llegó al punto
        if (distancia < radioWaypoint && !esperandoSalir)
        {
            esperandoSalir = true;
            bool esMedio = indexActual == waypoints.Length - 1;
            indexActual = esMedio ? Random.Range(0, waypoints.Length - 1) : waypoints.Length - 1;
        }

        // espera a alejarse antes de detectar siguiente
        if (esperandoSalir && distancia > radioWaypoint * 2f)
            esperandoSalir = false;
        
        float velMax = 50f;
        
        Vector3 velFlat = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // evita que las colisiones la muevan
        Vector3 velDeseada = fuerzaFlat.normalized * velFlat.magnitude;
        rb.velocity = new Vector3(velDeseada.x, 0f, velDeseada.z);
        
        if (velFlat.magnitude > velMax)
        {
            velFlat = velFlat.normalized * velMax;
            rb.velocity = new Vector3(velFlat.x, 0f, velFlat.z);
        }
        
    }
}