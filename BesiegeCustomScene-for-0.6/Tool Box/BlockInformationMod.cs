using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class BlockInformationMod : MonoBehaviour
    {

        public enum VelocityUnit
        {
            kmh = 0,
            ms = 1,
            mach = 2,
        };

        public VelocityUnit velocityUnit;
        GameObject targetBlock;
        bool validBlock = false;

        public Vector3 Position { get; private set; }
        public Vector3 Velocity { get; private set; }
        public float Distance { get; private set; }
        public float Overload { get; private set; }
        public float Acceleration { get; private set; }

        private Vector3 lastPosition;
        private Vector3 lastVelocity;
        private bool isFirstFrame;
        private Queue<float> averageAccelerationQueue = new Queue<float>();
        readonly int queueSize = 60;

        void Awake()
        {
            isFirstFrame = false;
            velocityUnit = VelocityUnit.kmh;
        }

        void FixedUpdate()
        {
            if (StatMaster.levelSimulating)
            {
                if (!isFirstFrame)
                {
                    isFirstFrame = true;

                    LoadBlock();
                    averageAccelerationQueue.Clear();
                    //func_position();
                    //func_velocity();
                }
                FuncPosition();
                FuncVelocity();
                FuncDistance();
                FuncOverload();
                FuncAcceleration();
            }
            else
            {
                if (isFirstFrame)
                {
                    isFirstFrame = false;

                    validBlock = false;
                    InitPropertise();
                }
            }
        }

        public void InitPropertise()
        {
            Position = lastPosition = Vector3.zero;
            Velocity = lastVelocity = Vector3.zero;
            Distance = 0f;
            Overload = 0f;
            Acceleration = 0f;
        }

        public void ChangedVelocityUnit()
        {
            switch (velocityUnit)
            {
                case VelocityUnit.kmh: { velocityUnit = VelocityUnit.ms; } break;
                case VelocityUnit.ms: { velocityUnit = VelocityUnit.mach; } break;
                case VelocityUnit.mach: { velocityUnit = VelocityUnit.kmh; } break;
            }

            Velocity = Vector3.zero;

            BesiegeConsoleController.ShowMessage(velocityUnit.ToString());
        }

        void FuncPosition()
        {
            if (validBlock)
            {
                Position = targetBlock.GetComponent<Rigidbody>().position;
            }
            else
            {
                Position = new Vector3(0, 0, 0);
            }
        }

        void FuncVelocity()
        {
            if (validBlock)
            {
                Vector3 v1 = targetBlock.GetComponent<Rigidbody>().velocity;

                Velocity = GetVelocity(v1, velocityUnit);
            }
            else
            {
                Velocity = Vector3.zero;
            }
        }

        Vector3 GetVelocity(Vector3 velocity, VelocityUnit velocityUnit)
        {
            if (velocityUnit == VelocityUnit.kmh)
            {
                //V = string.Format("{0:N0}", v1.magnitude * 3.6f);
                velocity = Vector3.Scale(velocity, Vector3.one * 3.6f);
            }
            else if (velocityUnit == VelocityUnit.ms)
            {
                //V = string.Format("{0:N0}", v1.magnitude);
                //velocity = velocity;
            }
            else if (velocityUnit == VelocityUnit.mach)
            {
                //V = string.Format("{0:N2}", v1.magnitude / 340f);
                velocity = Vector3.Scale(velocity, Vector3.one / 340f);
            }
            return velocity;
        }

        void FuncDistance()
        {
            if (validBlock)
            {
                //Distance = string.Format("{0:N2}", _Distance / 1000f);
                //Vector3 v2 = _Position - startingBlock.GetComponent<Rigidbody>().position;
                //_Distance += v2.magnitude;
                //_Position = startingBlock.GetComponent<Rigidbody>().position;
                Vector3 currentPosition = Position;

                Vector3 deltaPosition = currentPosition - lastPosition;
                Distance += deltaPosition.magnitude;
                lastPosition = currentPosition;
            }
            else
            {
                //Distance = "0";
                Distance = 0f;
            }
        }

        void FuncOverload()
        {
            if (validBlock)
            {
                //Vector3 v1 = startingBlock.GetComponent<Rigidbody>().velocity;
                //float _overload = 0;
                //float timedomain = Time.fixedDeltaTime * 25f;
                //if (timedomain > 0) _overload =
                //           Vector3.Dot((_V - v1), base.transform.up) / timedomain / 38.5f + Vector3.Dot(Vector3.up, base.transform.up) - 1;
                //_V = v1;
                //Overload = string.Format("{0:N2}", _overload);

                //Vector3 v1 = Velocity;

                //float timedomain = Time.fixedDeltaTime * 25f;
                //if (timedomain > 0)
                //{
                //    Overload = Vector3.Dot(getVelocity(v1, velocityUnit) - lastVelocity, transform.up) / timedomain / 38.5f + Vector3.Dot(Vector3.up, transform.up) - 1;
                //}

                float acceleration = Acceleration;

                if (velocityUnit == VelocityUnit.kmh)
                {
                    //V = string.Format("{0:N0}", v1.magnitude * 3.6f);
                    acceleration = acceleration / 3.6f;
                }
                else if (velocityUnit == VelocityUnit.mach)
                {
                    //V = string.Format("{0:N2}", v1.magnitude / 340f);
                    acceleration = acceleration * 340f;
                }


                if (averageAccelerationQueue.Count == queueSize)
                {
                    averageAccelerationQueue.Dequeue();
                }
                averageAccelerationQueue.Enqueue(acceleration);

                Overload = averageAccelerationQueue.Average() / Physics.gravity.magnitude;
            }
            else
            {
                //Overload = "0";
                Overload = 0;
            }
        }

        void FuncAcceleration()
        {
            if (validBlock)
            {
                Vector3 currentVelocity = Velocity;
                Vector3 _acceleration = Vector3.zero;
                int sign = 0;

                Vector3 deltaVelocity = currentVelocity - lastVelocity;
                _acceleration = deltaVelocity / Time.fixedDeltaTime;
                sign = (Vector3.Dot(_acceleration, currentVelocity) > 0) ? 1 : -1;
                Acceleration = sign * _acceleration.magnitude;
                lastVelocity = currentVelocity;
            }
            else
            {
                Acceleration = 0f;
            }
        }

        //载入模块
        void LoadBlock()
        {
            try
            {
                targetBlock = GameObject.Find("StartingBlock");
                // Dlight = GameObject.Find("Directional light");
            }
            catch { }

            if (targetBlock == null)
            {
                targetBlock = GameObject.Find("bgeL0");
                if (targetBlock == null)
                {
                    MyBlockInfo info = UnityEngine.Object.FindObjectOfType<MyBlockInfo>();
                    if (info != null)
                    {
                        targetBlock = info.gameObject;
                    }
                    //if (targetBlock == null)
                    //{
                    //    validBlock = false; 
                    //}
                    //else
                    //{
                    //    validBlock = true;
                    //}

                    validBlock = (targetBlock == null ? false : true);
                }
                else
                {
                    validBlock = true;
                }
            }
            else
            {
                validBlock = true;
            }

#if DEBUG 
            ConsoleController.ShowMessage(targetBlock.name);
#endif
        }



    }
}
