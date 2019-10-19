using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform backGround;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        float bgTargetY = backGround.position.y - 0.15f;        

        Vector3 bgPosition = new Vector3(backGround.position.x, bgTargetY, backGround.position.z);
        backGround.position = Vector3.Lerp(backGround.position, bgPosition, speed * Time.deltaTime);
    }
}
