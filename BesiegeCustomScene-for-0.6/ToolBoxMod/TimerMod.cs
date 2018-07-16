using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class TimerMod : MonoBehaviour
    {

        
        string _currentTime;

        DateTime _startTime;

        public bool TimeSwitch { get; set; }

        public string CurrentSystemTime { get { return DateTime.Now.ToString("HH:mm:ss"); } }

        public string CurrentTimerTime { get { return _currentTime; }  set { _currentTime = value; } }

        bool isTicking;

        void Start()
        {
            isTicking = false;
            _currentTime = "00:00:00";
            _startTime = DateTime.Now;
        }

        void Update()
        {

            if (TimeSwitch)
            {
                if (!isTicking)
                {
                    isTicking = true;
                    _startTime = DateTime.Now;
                }

                TimeSpan span = DateTime.Now - _startTime;
                DateTime n = new DateTime(span.Ticks);
                _currentTime = n.ToString("mm:ss:ff");
            }
            else
            {
                
            }
        }


        public void Retime()
        {
            TimeSwitch = false;
            _currentTime = "00:00:00";
            isTicking = false;
        }

    }
}
