using UnityEngine;

namespace BesiegeCustomScene
{
    public class test_script : MonoBehaviour
    {


        void Awake()
        {
            BesiegeConsoleController.ShowMessage("test script awake...");

        }

        //ModKey modKey = ModKeys.GetKey("test-key");
        //public int 生成类型 = 0;

        void Update()//每帧执行一次
        {
            if (/*modKey.IsPressed ||*/ Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("?? debug");

                BesiegeConsoleController.ShowMessage("?? show");
            }

            //Debug.Log("?? debug");

            //BesiegeConsoleController.ShowMessage("?? show");
        }
    }
}
