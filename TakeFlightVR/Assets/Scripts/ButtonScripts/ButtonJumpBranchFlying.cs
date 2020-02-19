using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonJumpBranchFlying: MonoBehaviour
{
    public Button buttonJumpBranchFlying;
    // Use this for initialization
    void Start()
    {
        Button btn= buttonJumpBranchFlying.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick)
    }

    // Update is called once per frame
    void TaskOnClick()
    {
        // triggers animation
        Debug.log("branch jump to flying animation")
    }
}
