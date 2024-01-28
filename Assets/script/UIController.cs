using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    AnimatorStateInfo stateinfo;
    Animator animate;
    bool isDone=false;
    public void nextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void toScene(int num)
    {
        SceneManager.LoadScene(num);
    }
    public void Start()
    {
       
    }
    public void Update()
    {
        animate = GetComponent<Animator>();
        stateinfo = animate.GetCurrentAnimatorStateInfo(0);

        if (stateinfo.IsName("CG") && stateinfo.normalizedTime > 1.0f)
        {
            isDone = true;
            nextScene();
        }
        animate.SetBool("isFinish", isDone);

    
    }
    // Start is called before the first frame update

}
