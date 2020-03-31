using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDestroy : MonoBehaviour
{

    public float destroyDelay = 0f;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision col)
    {

        if (col.collider.gameObject.layer != 12 && col.collider.gameObject.tag != "Weapon")
        {
            if (col.collider.gameObject.tag == "Boundary")
                Destroy(gameObject);
            else
                Destroy(gameObject, destroyDelay);
        }
    }
}
