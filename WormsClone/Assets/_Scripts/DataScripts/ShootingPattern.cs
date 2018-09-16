using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingPattern : ScriptableObject
{
    public abstract void Shoot(GameObject bullet, BulletData bulletData,Transform self, Vector2 barrelOffSet);
    protected void ShootBullet(GameObject bullet, BulletData bulletData, Transform self, Vector2 barrelOffSet, Quaternion rotation)
    {
        Vector2 calculatedPosition = self.TransformVector(barrelOffSet) + self.position;

        var bulletInstance = Instantiate(bullet, calculatedPosition, Quaternion.identity);
        bulletInstance.transform.up = rotation * self.right;
        bulletInstance.GetComponent<Bullet>().data = bulletData;
    }
}
