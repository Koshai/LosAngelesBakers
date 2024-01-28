using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimationController : MonoBehaviour
{
    Animator animate;
    float hMovement_P;
    float vMovement_P;
    bool detectMove_P=false;
    bool detectElbow_P = false;
    bool detectCup_P = false;
    bool changePosition = true;
    int note = 0;
    AnimatorStateInfo stateInfo;

    // Start is called before the first frame update
    void Start()
    {
        animate = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = animate.GetCurrentAnimatorStateInfo(0);

        detectElbow_P = Input.GetKey(KeyCode.Z);
        detectCup_P = Input.GetKey(KeyCode.X);
        hMovement_P = Input.GetAxis("Horizontal");
        vMovement_P = Input.GetAxis("Vertical");

        if (hMovement_P != 0 || vMovement_P != 0)
        { detectMove_P = true; }
        else { detectMove_P = false; }
        if (note == 0 && hMovement_P > 0)
        {
            note++;
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (note == 1 && hMovement_P < 0)
        {
            note--;
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (stateInfo.IsName("elbow") && stateInfo.normalizedTime >= 1.0f)
        {
            detectElbow_P = false;
        }
        if (stateInfo.IsName("cup") && stateInfo.normalizedTime >= 1.0f)
        {
            detectCup_P = false;
        }


        animate.SetBool("isMove", detectMove_P);
        animate.SetBool("isElbow", detectElbow_P);
        animate.SetBool("isCup", detectCup_P);
        animate.SetBool("mirrorAni", changePosition);



    }
}
