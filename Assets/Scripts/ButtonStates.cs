using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStates : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void NextState()
    {
        switch (animator.GetInteger("State")){
            case 0:
                animator.SetInteger("State", 1);
                break;
            case 1:
                animator.SetInteger("State", 2);
                break;
            case 2:
                animator.SetInteger("State", 0);
                break;
        }
    }
}
