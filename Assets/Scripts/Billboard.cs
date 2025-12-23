using System;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Knight knight;
    private void LateUpdate()
    {
        if (knight.WalkDirection == Knight.WalkableDirection.Right)
            gameObject.transform.localScale = new Vector3(0.1f, 0.1f);
        else
            gameObject.transform.localScale = new Vector3(-0.1f, 0.1f);
    }
}
