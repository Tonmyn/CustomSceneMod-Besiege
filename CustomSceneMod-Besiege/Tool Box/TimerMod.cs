using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CustomScene
{
    public class TimerMod : MonoBehaviour
    {

        
        string _currentTime;

        DateTime _startTime;
        /// <summary>计时器开关</summary>
        public bool TimeSwitch { get; set; }
        /// <summary>当前系统时间</summary>
        public string CurrentSystemTime { get { return DateTime.Now.ToString("HH:mm:ss"); } }
        /// <summary>当前计时器时间</summary>
        public string CurrentTimerTime { get { return _currentTime; }  set { _currentTime = value; } }
        /// <summary>是否在计时</summary>
        bool isTicking;

        void Awake()
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

        /// <summary>计时器重置</summary>
        public void Retime()
        {
            TimeSwitch = false;
            _currentTime = "00:00:00";
            isTicking = false;
        }

    }
}
