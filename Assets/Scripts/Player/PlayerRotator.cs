using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private Camera playerCamera;

    [Header("Limites verticales")]
    [SerializeField] private float minVertical = -30f;
    [SerializeField] private float maxVertical = 30f;

    private bool isDragging = false;
    private float lastMouseX;
    private float lastMouseY;
    private float currentVertical = 0f;
    private Vector3 pivotPoint;

    void Start()
    {
        // Calcula el centro del personaje automaticamente usando los renderers
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            foreach (Renderer r in renderers)
                bounds.Encapsulate(r.bounds);
            pivotPoint = bounds.center;
        }
        else
        {
            pivotPoint = transform.position;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsClickingOnPlayer())
            {
                isDragging = true;
                lastMouseX = Input.mousePosition.x;
                lastMouseY = Input.mousePosition.y;
            }
        }

        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        if (isDragging)
        {
            float deltaX = Input.mousePosition.x - lastMouseX;
            float deltaY = Input.mousePosition.y - lastMouseY;

            // Rotar horizontalmente desde el centro
            transform.RotateAround(pivotPoint, Vector3.up, -deltaX * rotationSpeed);

            // Rotar verticalmente desde el centro con limites
            currentVertical -= deltaY * rotationSpeed;
            currentVertical = Mathf.Clamp(currentVertical, minVertical, maxVertical);
            transform.RotateAround(pivotPoint, transform.right, deltaY * rotationSpeed);

            lastMouseX = Input.mousePosition.x;
            lastMouseY = Input.mousePosition.y;
        }
    }

    private bool IsClickingOnPlayer()
    {
        Camera cam = playerCamera != null ? playerCamera : Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit.transform.IsChildOf(transform) || hit.transform == transform;

        return false;
    }
}