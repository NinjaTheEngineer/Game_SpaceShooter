using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public Animator animator;

    void Start(){
        animator = GameObject.Find("MainCamera").GetComponent<Animator>();
    }
    
    public void CamShake(){
        animator.SetTrigger("shake");
    }
}
