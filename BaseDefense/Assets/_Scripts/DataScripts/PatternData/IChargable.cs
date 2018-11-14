using UnityEngine;

interface IChargable
{
    int ChargedShot(int charge, int maxCharge, BulletData chargedBullet, Transform self, Vector2 barrelOffSet);
}
