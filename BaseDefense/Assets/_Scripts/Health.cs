using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Events
    public delegate void Death(GameObject sender);
    /// <summary>
    /// Triggers just before death.
    /// </summary>
    public event Death OnDeath;
    /// <summary>
    /// Triggers if the component is killed by the DestroySelf( ) function.
    /// </summary>
    public event Death OnSelfDestruct;

    public delegate void HealthCHange(int newHealth);
    /// <summary>
    /// Triggered on HealthChange.
    /// </summary>
    public event HealthCHange OnHealthCHange;
    /// <summary>
    /// Triggered on MaxHealthChange.
    /// </summary>
    public event HealthCHange OnMaxHealthCHange;
    #endregion
    #region ShowInEditor
    [SerializeField] HealthData data;
	#endregion
	#region HideInEditor
	private int healthPoint;
	public int HealthPoint
	{
		get { return healthPoint; }
		set
		{
            if (value >= data.maxHealthPoints)
            {
                healthPoint = data.maxHealthPoints;
            }
            else if (value <= 0)
            {
                healthPoint = 0;
                if (dead == false)
                {
                    dead = true;
                    Die();
                }
            }
            else
            {
                healthPoint = value;
            }
            OnHealthCHange?.Invoke(healthPoint);
        }
	}
    private int maxHealthPoint;
    public int MaxHealthPoint
    {
        get
        {
            return maxHealthPoint;
        }
        set
        {
            maxHealthPoint = value;
            OnMaxHealthCHange?.Invoke(value);
        }
    }
	float flashProgress;
	protected bool dead = false;
	Coroutine flash;
    #endregion

    #region UnityFunctions
    private void Start()
    {
        MaxHealthPoint = data.maxHealthPoints;
        HealthPoint = data.maxHealthPoints;
    }
    #endregion
    #region HealthFunctions
    void ResetHealth()
	{
		HealthPoint = data.maxHealthPoints;
		dead = false;
	}
    /// <summary>
    /// Increases the health of the component by heal, but it will never exceed the maxHealth.
    /// Triggers the green flash if it's enabled.
    /// </summary>
    /// <param name="heal"></param>
    public void GainHealth(int heal)
	{
		HealthPoint += heal;
		if (data.flashOnHealthChange && heal > 0 && flash == null )
		{
			flash = StartCoroutine(FlashGreen());
		}
		if (HealthPoint > data.maxHealthPoints)
		{
			HealthPoint = data.maxHealthPoints;
		}
	}
    public void GainHealthOverTime(int heal, int times, float frequency)
    {
        StartCoroutine(HealOverTime(heal, times, frequency));
    }
    /// <summary>
    /// Decreases the health by the damage, if it's drop to zero the component will die.
    /// Plays the hitsound.
    /// Triggers the red flash if it's enabled.
    /// </summary>
    /// <param name="damage"></param>
    /// 
    public void TakeDamage(int damage)
    {
        if(dead)
        {
            return;
        }
        if (data.flashOnHealthChange && damage > 0 && flash == null )
        {
            flash = StartCoroutine(FlashRed());
        }
        if (data.hitAudio != null)
        {
            AudioManager.Instance.PlaySound(data.hitAudio);
        }
        HealthPoint -= damage;
    }
    public void TakeDamgeOverTime(int damage, int times, float frequency)
    {
        StartCoroutine(DamageOverTime(damage, times, frequency));
    }
    /// <summary>
    /// Kills the component with sound and effects.
    /// </summary>
    public void Die()
    {
        if(data.deathAnim != null)
        {
            var deathAnim = ObjectPool.Instance.Spawn(ObjectPool.Types.explosion, transform.position);
            Explosion exp = deathAnim.GetComponent<Explosion>();
            exp.data = data.deathAnim;
            exp.Initialize();
        }

        AudioManager.Instance.PlaySound(data.deathAudio);
        OnDeath?.Invoke(gameObject);

        Destroy(gameObject);   
    }
    /// <summary>
    /// Destroys the component without any sound or effect.
    /// </summary>
    public void DestroySelf()
    {
        OnSelfDestruct?.Invoke(gameObject);
        Destroy(gameObject);
    }
    #endregion
    #region Routines
    private IEnumerator DamageOverTime(int damage, int times, float frequency)
    {
        for (int i = 0; i < times; i++)
        {
            yield return new WaitForSeconds(frequency);
            TakeDamage(damage);
        }
    }
    private IEnumerator HealOverTime(int heal, int times, float frequency)
    {
        for (int i = 0; i < times; i++)
        {
            yield return new WaitForSeconds(frequency);
            GainHealth(heal);
        }
    }
    private IEnumerator FlashRed()
	{
		flashProgress = 1;
		while (flashProgress > 0)
		{
			GetComponent<SpriteRenderer>().color = new Color(1, 1 * flashProgress, 1 * flashProgress);

			flashProgress -= 1/ data.flashTime /2;
			yield return new WaitForSeconds(Time.deltaTime * (1 / Time.timeScale));
		}

		while (flashProgress <= 1)
		{
			GetComponent<SpriteRenderer>().color = new Color(1, 1 * flashProgress, 1 * flashProgress);

			flashProgress += 1 / data.flashTime / 2;
			yield return new WaitForSeconds(Time.deltaTime * (1 / Time.timeScale));
		}
		flash = null;
	}
	private IEnumerator FlashGreen()
	{
		flashProgress = 1;
		while (flashProgress > 0)
		{
			GetComponent<SpriteRenderer>().color = new Color(1 * flashProgress, 1 , 1 * flashProgress);

			flashProgress -= 1 / data.flashTime / 2;
			yield return new WaitForSeconds(Time.deltaTime * (1 / Time.timeScale));
		}

		while (flashProgress <= 1)
		{
			GetComponent<SpriteRenderer>().color = new Color(1 * flashProgress, 1 , 1 * flashProgress);

			flashProgress += 1 / data.flashTime / 2;
			yield return new WaitForSeconds(Time.deltaTime * (1 / Time.timeScale));
		}
		flash = null;
	}
    #endregion
}
