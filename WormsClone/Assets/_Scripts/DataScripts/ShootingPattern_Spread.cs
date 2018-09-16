using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SpreadPattern")]
public class ShootingPattern_Spread : ShootingPattern
{
    [Header("Spread Settings:")]
    [SerializeField] float spread;

    public override void Shoot(GameObject bullet, Transform self)
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject bulletInstance = Instantiate(bullet, self.position, Quaternion.identity);

        bulletInstance.transform.up = Quaternion.Euler(0, 0, Random.Range(-spread, spread)) * self.up;
    }
}
