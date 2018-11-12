using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingPattern : ScriptableObject
{
    public abstract void Shoot(BulletData bulletData, Transform self, Vector2 barrelOffSet);
    protected void ShootBullet(BulletData bulletData, Transform self, Vector2 barrelOffSet, Quaternion rotation)
	{
        Vector2 calculatedPosition = self.TransformVector(barrelOffSet) + self.position;

		GameObject bulletInstance = ObjectPool.Instance.Spawn(ObjectPool.Types.bullet, calculatedPosition);	
        bulletInstance.transform.up = rotation * self.right;
		var bulletComponent = bulletInstance.GetComponent<Bullet>();
		bulletComponent.data = bulletData;
		bulletComponent.Initialize();
	}
}
