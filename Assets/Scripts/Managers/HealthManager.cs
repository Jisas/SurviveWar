using UnityEngine;
using MyBox;
using TMPro;
using UnityEngine.Events;
using SurviveWar;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [ReadOnly, SerializeField] private float currentHealth;
    [SerializeField] private bool isDummyOrTarget;

    [Header("Dead Events")]
    public UnityEvent OnDead;

    private ObjectPooler pooler;
    private TextMeshPro damageText;
    private Animation damageTextAnim;
    private PointsController pointsController;

    void Start()
    {
        pooler = FindObjectOfType<ObjectPooler>();

        if (isDummyOrTarget) pointsController = FindObjectOfType<PointsController>();
        else pointsController = null;

        currentHealth = maxHealth;
    }

    public void TakeBodyDamage(float damage, bool isTarget)
    {
        currentHealth -= damage;

        ShootMinigameManager.Instance.bodyShootAmmount++;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead(false, isTarget);
        }
    }
    public void TakeBodyDamage(float damage, Vector3 position, Vector3 offset, bool isTarget)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead(false, isTarget);
        }

        var newText = pooler.SpawnFromPool("DamageText", position + offset, Quaternion.identity) as GameObject;
        damageText = newText.GetComponent<TextMeshPro>();
        damageText.color = Color.white;
        damageText.text = damage.ToString();
        damageTextAnim = damageText.GetComponent<Animation>();
        damageTextAnim.Play();
    }

    public void TakeHeadShoot(bool isTarget)
    {
        currentHealth -= maxHealth;

        ShootMinigameManager.Instance.headShootAmmount++;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead(true, isTarget);
        }
    }
    public void TakeHeadShoot(Vector3 position, Vector3 offset, bool isTarget)
    {
        currentHealth -= maxHealth;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead(true, isTarget);
        }

        var newText = pooler.SpawnFromPool("DamageText", position + offset, Quaternion.identity) as GameObject;
        damageText = newText.GetComponent<TextMeshPro>();
        damageText.color = Color.red;
        damageText.text = "100";
        damageTextAnim = damageText.GetComponent<Animation>();
        damageTextAnim.Play();
    }

    public void TargetTakeDamage(float damage, bool isTarget)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dead(false, isTarget);
        }
    }

    public void ResetCurrentHealth() { currentHealth = maxHealth; }

    public void Dead(bool isHeadShoot, bool isTarget) 
    { 
        if ( pointsController != null )
        {
            if (isHeadShoot)
                pointsController.AddPoint(2);
            else
                pointsController.AddPoint(1);

            if (isTarget)
                ShootMinigameManager.Instance.targetsDestroyAmmount++;
            else
                ShootMinigameManager.Instance.dummysDestroyAmmount++;
        }

        OnDead.Invoke(); 
    }
}
