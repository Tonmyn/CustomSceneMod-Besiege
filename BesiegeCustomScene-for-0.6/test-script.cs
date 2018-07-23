using Modding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class test_script :MonoBehaviour
    {


        void Awake()
        {
            BesiegeConsoleController.ShowMessage("test script awake...");

        }

        //ModKey modKey = ModKeys.GetKey("test-key");
        //public int 生成类型 = 0;

        void Update()//每帧执行一次
        {
            if (StatMaster.InBuildPlayMode)//如果在模拟模式
            {
                if (/*modKey.IsPressed ||*/ Input.GetKeyDown(KeyCode.H))
                {
                    Debug.Log("?? debug");

                    BesiegeConsoleController.ShowMessage("?? show");

                    //RaycastHit hit;//提前创建射线碰撞信息储存器
                    //if (
                    //    Physics.Raycast(//当投射一个射线并碰撞到了一个碰撞器
                    //        Camera.main.ScreenPointToRay(Input.mousePosition),//方向与起点。这是默认的调用主相机的鼠标对应位置而生成的射线方向以及位置。如果使用别的射线，可以通过 Ray 射线 = new Ray(起点,方向)来自己创建一个ray。
                    //        out hit,//把碰撞信息输出到这个RaycastHit变量上
                    //        Camera.main.farClipPlane))//射线长度。其实正无穷不是必要的，使用相机的FarClipDistance就可以。
                    //{
                    //    GameObject bomb = (GameObject)Instantiate(//实例化一个Game Object
                    //        PrefabMaster.BlockPrefabs[23].gameObject,//预设列表中的第23号元素（炸弹）的Game Object 
                    //        hit.point,//生成位置。这里使用的是刚才投射的射线碰到碰撞器的时候的位置。
                    //        Camera.main.transform.rotation, this.transform);//相机的朝向。
                    //    bomb.GetComponent<ExplodeOnCollideBlock>().radius = 7;
                    //    bomb.GetComponent<ExplodeOnCollideBlock>().Explodey();//告诉炸弹的炸弹组件爆炸

                    //    //if (hit.transform.GetComponent<ExplodeOnCollideBlock>())
                    //    //{
                    //    //    hit.transform.GetComponent<ExplodeOnCollideBlock>().radius = 7000;
                    //    //    hit.transform.GetComponent<ExplodeOnCollideBlock>().Explodey();
                    //    //}
                    //}
                }
            }

            Debug.Log("?? debug");

            BesiegeConsoleController.ShowMessage("?? show");
        }
    }
}
