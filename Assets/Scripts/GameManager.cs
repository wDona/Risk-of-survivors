using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("XP")]
    public int gmp = 0;

    [Header("Niveles de habilidades")]
    public int nivelVida = 0;
    public int nivelDano = 0;
    public int nivelSalto = 0;
    public int nivelVelocidad = 0;

    [Header("Costes por nivel")]
    public int costeVida = 100;
    public int costeDano = 120;
    public int costeSalto = 150;
    public int costeVelocidad = 100;

    [Header("Niveles maximos")]
    public int maxVida = 5;
    public int maxDano = 5;
    public int maxSalto = 3;
    public int maxVelocidad = 5;

    // Stats calculados
    public float VidaMaxima => 100 + nivelVida * 50;
    public float DanoBonus => nivelDano * 15;
    public int   SaltosExtra => nivelSalto;
    public float VelocidadBonus => nivelVelocidad * 0.5f;

    // Desbloqueos
    public bool DanoDesbloqueado => nivelVida > 0;
    public bool SaltoDesbloqueado => nivelVida > 0;
    public bool VelocidadDesbloqueada => nivelDano > 0 || nivelSalto > 0;

    // ----------------------------------------------------------------

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ----------------------------------------------------------------
    // Metodos de habilidades

    public bool ComprarVida()
    {
        if (nivelVida >= maxVida || gmp < costeVida) return false;
        gmp -= costeVida;
        nivelVida++;
        return true;
    }

    public bool ComprarDano()
    {
        if (!DanoDesbloqueado || nivelDano >= maxDano || gmp < costeDano) return false;
        gmp -= costeDano;
        nivelDano++;
        return true;
    }

    public bool ComprarSalto()
    {
        if (!SaltoDesbloqueado || nivelSalto >= maxSalto || gmp < costeSalto) return false;
        gmp -= costeSalto;
        nivelSalto++;
        return true;
    }

    public bool ComprarVelocidad()
    {
        if (!VelocidadDesbloqueada || nivelVelocidad >= maxVelocidad || gmp < costeVelocidad) return false;
        gmp -= costeVelocidad;
        nivelVelocidad++;
        return true;
    }

    public void AnadirGMP(int cantidad)
    {
        gmp += cantidad;
    }

    public void ResetearHabilidades()
    {
        gmp += nivelVida * costeVida;
        gmp += nivelDano * costeDano;
        gmp += nivelSalto * costeSalto;
        gmp += nivelVelocidad * costeVelocidad;

        nivelVida = 0;
        nivelDano = 0;
        nivelSalto = 0;
        nivelVelocidad = 0;
    }
}