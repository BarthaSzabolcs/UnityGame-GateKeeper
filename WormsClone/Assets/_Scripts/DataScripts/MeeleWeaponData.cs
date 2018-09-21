using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/MeeleWeapon")]
public class MeeleWeaponData : WeaponData
{
    #region ShowInEditor
    [Header("MeeleWeapon Settings:")]
    public MeeleAttack meeleAttack;
    public List<string> taggedToDamage;
    public int damage;

    public Vector2 damageTriggerSize;
    public Vector2 damageTriggerOffSet;
    public float swingDegrees;
    #endregion
    #region HideInEditor
    Rigidbody2D self;
    BoxCollider2D damageTrigger;
    PlayerController wielder;
    Weapon weapon;
    #endregion

    public void Initialize(Rigidbody2D self, BoxCollider2D damageTrigger, PlayerController wielder)
    {
        this.self = self;
        this.damageTrigger = damageTrigger;
        weapon = self.GetComponent<Weapon>();
        this.wielder = wielder;
    }

    public IEnumerator AttackRoutine()
    {
        wielder.canAim = false;
        damageTrigger.enabled = true;

        bool isLookingRight = wielder.transform.position.x > wielder.target.x;

        for (var i = 0; i < meeleAttack.positiveLoops; i++)
        {
            meeleAttack.PositivePhase(self, isLookingRight);
            yield return new WaitForSeconds(0.016f);
        }
        for (var i = 0; i < meeleAttack.negativeLoops; i++)
        {
            meeleAttack.NegativePhase(self, isLookingRight);
            yield return new WaitForSeconds(0.016f);
        }
       
        damageTrigger.enabled = false;
        weapon.meeleRoutine = null;
        wielder.canAim = true;
    }

    public override void Attack()
    {
        Debug.Log("Attack not implemented");
    }
}
