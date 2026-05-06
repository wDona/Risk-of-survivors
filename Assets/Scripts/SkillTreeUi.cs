using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SkillTreeUI : MonoBehaviour
{
    [Header("XP")]
    [SerializeField] private TextMeshProUGUI txtXP;
    [SerializeField] private Image barraXP;

    [Header("Botones comprar")]
    [SerializeField] private Button btnVida;
    [SerializeField] private Button btnDano;
    [SerializeField] private Button btnSalto;
    [SerializeField] private Button btnVelocidad;

    [Header("Textos coste")]
    [SerializeField] private TextMeshProUGUI txtCosteVida;
    [SerializeField] private TextMeshProUGUI txtCosteDano;
    [SerializeField] private TextMeshProUGUI txtCosteSalto;
    [SerializeField] private TextMeshProUGUI txtCosteVelocidad;

    [Header("Indicadores de nivel (imagenes)")]
    [SerializeField] private Image[] puntosVida;
    [SerializeField] private Image[] puntosDano;
    [SerializeField] private Image[] puntosSalto;
    [SerializeField] private Image[] puntosVelocidad;

    [Header("Overlays de bloqueo")]
    [SerializeField] private GameObject lockDano;
    [SerializeField] private GameObject lockSalto;
    [SerializeField] private GameObject lockVelocidad;

    [Header("Textos stats")]
    [SerializeField] private TextMeshProUGUI txtStatVida;
    [SerializeField] private TextMeshProUGUI txtStatDano;
    [SerializeField] private TextMeshProUGUI txtStatSalto;
    [SerializeField] private TextMeshProUGUI txtStatVel;

    [Header("Colores")]
    [SerializeField] private Color colorActivo = new Color(0.86f, 0.12f, 0.12f);
    [SerializeField] private Color colorInactivo = new Color(0.2f, 0.2f, 0.2f);

    [Header("XP maximo pa la barra")]
    [SerializeField] private int xpMax = 1000;

    [Header("Navegacion")]
    [SerializeField] private string menuSceneName = "menuPrincipal";

    // ----------------------------------------------------------------

    void Start()
    {
        btnVida.onClick.AddListener(OnComprarVida);
        btnDano.onClick.AddListener(OnComprarDano);
        btnSalto.onClick.AddListener(OnComprarSalto);
        btnVelocidad.onClick.AddListener(OnComprarVelocidad);
        
        Button btnVolver = GameObject.Find("BtnVolver")?.GetComponent<Button>();
        if (btnVolver) btnVolver.onClick.AddListener(OnVolver);

        Actualizar();
    }

    // ----------------------------------------------------------------
    // Compras

    void OnComprarVida()
    {
        GameManager.Instance.ComprarVida();
        Actualizar();
    }

    void OnComprarDano()
    {
        GameManager.Instance.ComprarDano();
        Actualizar();
    }

    void OnComprarSalto()
    {
        GameManager.Instance.ComprarSalto();
        Actualizar();
    }

    void OnComprarVelocidad()
    {
        GameManager.Instance.ComprarVelocidad();
        Actualizar();
    }

    public void OnResetear()
    {
        GameManager.Instance.ResetearHabilidades();
        Actualizar();
    }

    public void OnVolver()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    // ----------------------------------------------------------------
    // Actualizar UI

    void Actualizar()
    {
        var gm = GameManager.Instance;

        // XP
        if (txtXP) txtXP.text = gm.gmp + " XP";
        if (barraXP) barraXP.fillAmount = Mathf.Clamp01((float)gm.gmp / xpMax);

        // Bloqueos
        if (lockDano) lockDano.SetActive(!gm.DanoDesbloqueado);
        if (lockSalto) lockSalto.SetActive(!gm.SaltoDesbloqueado);
        if (lockVelocidad) lockVelocidad.SetActive(!gm.VelocidadDesbloqueada);

        // Puntos de nivel
        ActualizarPuntos(puntosVida, gm.nivelVida);
        ActualizarPuntos(puntosDano, gm.nivelDano);
        ActualizarPuntos(puntosSalto, gm.nivelSalto);
        ActualizarPuntos(puntosVelocidad, gm.nivelVelocidad);

        // Textos coste
        ActualizarCoste(txtCosteVida, gm.nivelVida, gm.maxVida, gm.costeVida, gm.gmp);
        ActualizarCoste(txtCosteDano, gm.nivelDano, gm.maxDano, gm.costeDano, gm.gmp);
        ActualizarCoste(txtCosteSalto, gm.nivelSalto, gm.maxSalto, gm.costeSalto, gm.gmp);
        ActualizarCoste(txtCosteVelocidad, gm.nivelVelocidad, gm.maxVelocidad, gm.costeVelocidad, gm.gmp);

        // Botones interactivos
        btnVida.interactable = gm.nivelVida < gm.maxVida && gm.gmp >= gm.costeVida;
        btnDano.interactable = gm.DanoDesbloqueado && gm.nivelDano < gm.maxDano && gm.gmp >= gm.costeDano;
        btnSalto.interactable = gm.SaltoDesbloqueado && gm.nivelSalto < gm.maxSalto && gm.gmp >= gm.costeSalto;
        btnVelocidad.interactable = gm.VelocidadDesbloqueada && gm.nivelVelocidad < gm.maxVelocidad && gm.gmp >= gm.costeVelocidad;

        // Stats
        if (txtStatVida) txtStatVida.text = gm.VidaMaxima.ToString();
        if (txtStatDano) txtStatDano.text = (10 + gm.DanoBonus).ToString();
        if (txtStatSalto) txtStatSalto.text = (1 + gm.SaltosExtra).ToString();
        if (txtStatVel) txtStatVel.text = (5 + gm.VelocidadBonus).ToString("F1");
    }

    void ActualizarPuntos(Image[] puntos, int nivel)
    {
        if (puntos == null) return;
        for (int i = 0; i < puntos.Length; i++)
            if (puntos[i]) puntos[i].color = i < nivel ? colorActivo : colorInactivo;
    }

    void ActualizarCoste(TextMeshProUGUI txt, int nivel, int max, int coste, int xp)
    {
        if (!txt) return;
        if (nivel >= max) { txt.text = "MAX"; return; }
        txt.text = coste + " XP";
        txt.color = xp >= coste ? colorActivo : Color.gray;
    }
}