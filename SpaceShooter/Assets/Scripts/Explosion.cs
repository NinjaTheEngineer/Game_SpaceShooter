using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
// Destroy after 0.40secs of starting
        Destroy(this.gameObject, 0.4f);
    }
}
