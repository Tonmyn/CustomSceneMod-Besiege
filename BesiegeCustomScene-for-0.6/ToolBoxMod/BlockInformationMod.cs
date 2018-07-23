using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BesiegeCustomScene
{
    class BlockInformationMod: MonoBehaviour
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

        public Vector3 Position { get { return position; } }
        public Vector3 Velocity { get { return velocity; } }
        public float Distance { get { return distance; } }
        public float Overload { get { return overload; } }
        public float Acceleration { get { return acceleration; } }

        Vector3 position, lastPosition;
        Vector3 velocity, lastVelocity;
        float distance;
        float overload;
        float acceleration;

        bool isFirstFram;

        void Awake()
        {
            isFirstFram = false;
            velocityUnit = VelocityUnit.kmh;
        } 

        void FixedUpdate()
        {
            if (StatMaster.levelSimulating)
            {
                if (!isFirstFram)
                {
                    isFirstFram = true;

                    LoadBlock();

                    //func_position();
                    //func_velocity();
                }
                func_position();
                func_velocity();
                func_distance();
                func_overload();
                func_acceleration();
            }
            else
            {
                if (isFirstFram)
                {
                    isFirstFram = false;

                    validBlock = false;
                    initPropertise();
                }
            }
        }

        public void initPropertise()
        {
            position = lastPosition = Vector3.zero;
            velocity = lastVelocity = Vector3.zero;
            distance = 0f;
            overload = 0f;
            acceleration = 0f;           
        }

        public void changedVelocityUnit()
        {
            //if (velocityUnit == VelocityUnit.kmh)
            //{
            //    velocityUnit = VelocityUnit.ms;
            //}
            //if (velocityUnit == VelocityUnit.ms)
            //{
            //    velocityUnit = VelocityUnit.mach;
            //}
            //if (velocityUnit == VelocityUnit.mach)
            //{
            //    velocityUnit = VelocityUnit.kmh;
            //}
            switch (velocityUnit)
            {
                case VelocityUnit.kmh: { velocityUnit = VelocityUnit.ms; } break;
                case VelocityUnit.ms: { velocityUnit = VelocityUnit.mach; } break;
                case VelocityUnit.mach: { velocityUnit = VelocityUnit.kmh; } break;
            }
                
            //initPropertise();
            velocity = Vector3.zero;

            BesiegeConsoleController.ShowMessage(velocityUnit.ToString());
        }

        void func_position()
        {
            if (validBlock)
            {
                position = targetBlock.GetComponent<Rigidbody>().position;
            }
            else
            {
                position = new Vector3(0, 0, 0);
            }
        }

        void func_velocity()
        {
            if (validBlock)
            {
                Vector3 v1 = targetBlock.GetComponent<Rigidbody>().velocity;

                velocity = getVelocity(v1, velocityUnit);
            }
            else
            {
                velocity = Vector3.zero;
            }
        }

        Vector3 getVelocity(Vector3 velocity,VelocityUnit velocityUnit)
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

        void func_distance()
        {
            if (validBlock)
            {
                //Distance = string.Format("{0:N2}", _Distance / 1000f);
                //Vector3 v2 = _Position - startingBlock.GetComponent<Rigidbody>().position;
                //_Distance += v2.magnitude;
                //_Position = startingBlock.GetComponent<Rigidbody>().position;
                Vector3 currentPosition = position;

                Vector3 deltaPosition = currentPosition - lastPosition;
                distance += deltaPosition.magnitude;
                lastPosition = currentPosition;
            }
            else
            {
                //Distance = "0";
                distance = 0f;
            }
        }

        void func_overload()
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

                Vector3 v1 = velocity;

                float timedomain = Time.fixedDeltaTime * 25f;
                if (timedomain > 0)
                {
                    overload = Vector3.Dot(getVelocity(v1, velocityUnit) - lastVelocity, transform.up) / timedomain / 38.5f + Vector3.Dot(Vector3.up, transform.up) - 1;
                }    
            }
            else
            {
                //Overload = "0";
                overload = 0;
            }
        }

        void func_acceleration()
        {
            if (validBlock)
            {
                Vector3 currentVelocity = velocity;
                Vector3 _acceleration = Vector3.zero;
                int sign = 0;

                Vector3 deltaVelocity = currentVelocity - lastVelocity;
                _acceleration = deltaVelocity / Time.fixedDeltaTime;
                sign = (Vector3.Dot(_acceleration, currentVelocity) > 0) ? 1 : -1;
                acceleration = sign * _acceleration.magnitude;
                lastVelocity = currentVelocity;
            }
            else
            {
                acceleration = 0f;
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
