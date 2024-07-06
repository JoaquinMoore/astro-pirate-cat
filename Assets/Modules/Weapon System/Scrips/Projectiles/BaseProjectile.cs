using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public virtual void Move() { }
    public virtual IEnumerator OnInpact() { yield return null; }
}
