﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Events
    public delegate void Death(GameObject sender);
    /// <summary>
    /// Triggers before just before death.
    /// </summary>
    public event Death OnDeath;

    public delegate void SelfDestruct(GameObject sender);
    /// <summary>
    /// Triggers if the component is killed by the DestroySelf( ) function.
    /// </summary>
    public event SelfDestruct OnSelfDestruct;
    #endregion
    #region ShowInEditor
    [SerializeField] HealthData data;
	#endregion
	#region HideInEditor
	private int healthPoints;
	protected int HealthPoints
	{
		get { return healthPoints; }
		set
		{
			if (value >= data.maxHealthPoints)
			{
				healthPoints = data.maxHealthPoints;
			}else if (value <= 0)
			{
				healthPoints = 0;
			}
			else
			{
				healthPoints = value;
			}
		}
	}
	float flashProgress;
	protected bool dead = false;
	Coroutine flash;
    #endregion

    #region UnityFunctions
    protected virtual void Awake()
    {
        HealthPoints = data.maxHealthPoints;
    }
    #endregion
    #region HealthFunctions
    void ResetHealth()
	{
		HealthPoints = data.maxHealthPoints;
		dead = false;
	}
    /// <summary>
    /// Increases the health of the component by heal, but it will never exceed the maxHealth.
    /// Triggers the green flash if it's enabled.
    /// </summary>
    /// <param name="heal"></param>
    public virtual void GainHealth(int heal)
	{
		HealthPoints += heal;
		if (data.flashOnHealthChange && heal > 0 && flash == null )
		{
			flash = StartCoroutine(FlashGreen());
		}
		if (HealthPoints > data.maxHealthPoints)
		{
			HealthPoints = data.maxHealthPoints;
		}
	}
    /// <summary>
    /// Decreases the health by the damage, if it's drop to zero the component will die.
    /// Plays the hitsound.
    /// Triggers the red flash if it's enabled.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attacker"></param>
    public virtual void TakeDamage(int damage, GameObject attacker)
    {
        if (data.flashOnHealthChange && damage > 0 && flash == null )
        {
            flash = StartCoroutine(FlashRed());
        }

        if (!dead)
        {
            if (HealthPoints - damage < 0)
            {
                dead = true;
                Die();
            }
            else
            {
                if (data.hitAudio != null)
                {
                    AudioManager.Instance.PlaySound(data.hitAudio);
                }
                HealthPoints -= damage;
            }
        }
    }
    /// <summary>
    /// Kills the component with sound and effects.
    /// </summary>
    public virtual void Die()
    {
        if (data.deathAnim != null)
        {
            GameObject exp = Instantiate(data.deathAnim, transform.position, Quaternion.identity);
            exp.GetComponent<Explosion>().data = data.deathAnimData;
        }
        if (data.deathAudio != null)
        {
            AudioManager.Instance.PlaySound(data.deathAudio);
        }
        if (OnDeath != null)
        {
            OnDeath(gameObject);
        }

        Destroy(gameObject);   
    }
    /// <summary>
    /// Destroys the component without any sound or effect.
    /// </summary>
    public void DestroySelf()
    {
        if (OnSelfDestruct != null)
        {
           OnSelfDestruct(gameObject);
        }
        Destroy(gameObject);
    }
    #endregion
    #region HealthEffects
    protected IEnumerator FlashRed()
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
	protected IEnumerator FlashGreen()
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
