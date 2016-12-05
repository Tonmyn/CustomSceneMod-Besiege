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
        void Start()
        {

        }
        void OnTriggerEnter(Collider other)
        {
            if (StatMaster.isSimulating)
            {
                if(TriggerUI.TriggerIndex== this.Index-1)TriggerUI.TriggerIndex++;
            }

        }
    }
}
