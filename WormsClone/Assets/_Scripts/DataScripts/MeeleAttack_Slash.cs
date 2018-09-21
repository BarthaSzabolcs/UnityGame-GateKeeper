using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/MeeleAttack_Slash")]
public class MeeleAttack_Slash : MeeleAttack
{
    #region ShowInEditor
    [SerializeField] float positiveRotation, negativeRotation;
    #endregion

    public override void NegativePhase(Rigidbody2D self, bool isLookingRight)
    {
        var rotation = isLookingRight ? -negativeRotation : negativeRotation;

        self.transform.localEulerAngles = new Vector3
        (
            self.transform.localEulerAngles.x, 
            self.transform.localEulerAngles.y, 
            self.transform.localEulerAngles.z + rotation
        );
    }
    public override void PositivePhase(Rigidbody2D self, bool isLookingRight)
    {
        var rotation = isLookingRight ? -positiveRotation : positiveRotation;

        self.transform.localEulerAngles = new Vector3
        (
             self.transform.localEulerAngles.x, 
             self.transform.localEulerAngles.y, 
             self.transform.localEulerAngles.z - rotation
        );
    }
}
