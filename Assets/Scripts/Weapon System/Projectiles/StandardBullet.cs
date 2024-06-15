using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BaseBullet
{
    public override void Move()
    {
        _rigidBody.position += (Vector2)_rigidBody.transform.right * Time.fixedDeltaTime * _speed;
        Debug.Log("pe");
    }
}
