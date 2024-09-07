using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    public class MelleBullet : HitBoxBullet
    {
        public override void ExternalInputVisual()
        {

            if (_particle == null)
                return;

            GameObject particlehold = Instantiate(_particle,transform.position,transform.rotation);



            if (Input.mousePosition.x > 500)
            {
                particlehold.transform.right = transform.right;
                particlehold.transform.eulerAngles = new Vector3(particlehold.transform.eulerAngles.x,180, particlehold.transform.eulerAngles.z * - 1);
            }
            else
            {
                particlehold.transform.right = transform.right * -1;
                particlehold.transform.rotation = new Quaternion(0, particlehold.transform.rotation.y, particlehold.transform.rotation.z, particlehold.transform.rotation.w);
            }
        }
    }
}