using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    public class MTrigger : MonoBehaviour
    {
        public int Index = -1;
        public bool entry = false;
        void start()
        {

        }
        void OnTriggerEnter(Collider other)
        {
            if (StatMaster.isSimulating)
            {
                entry = true;
            }
        }
    }
}
