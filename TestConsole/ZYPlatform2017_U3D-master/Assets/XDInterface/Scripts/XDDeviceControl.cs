using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace ZhuoYuan.XDControl
{
    public class XDDeviceControl : MonoBehaviour
    {
        //- Initialize hardware motion plugin
        //- no parameter
        //- return: true-----initialize seccess, false----initialize fail
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool InitInstance();

        //- exit hardware motion plugin
        //- no parameter
        //- return: true-----exit seccess, false----exit fail
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool ExitInstance();

        //- notify xdplayer Run on other computer that game has start
        //- no parameter
        //- return: true-----notify seccess, false----notify fail
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool GameStart();

        //- notify xdplayer Run on other computer that gamme has finish
        //- no parameter
        //- return: true-----notify seccess, false----notify fail
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool GameStop();

        /// <summary>
        /// (old interface new is MotionControl)hardware motion control interface,update hardware attitude by time
        /// </summary>
        /// <param name="Time">time since start game</param>
        /// <param name="Pitch">lean angle front to back, range(-18/180PI,18/180PI), 0 is no lean, increase from front to back</param>
        /// <param name="Roll">lean angle left to right, range(-18/180PI-18,180PI), 0 is no lean, increase from left to right</param>
        /// <param name="Yaw">turn angle left to right, range(-18/180PI-18,180PI), 0 is no turning, increase from left to right, 6 dimensional or some special hardware only</param>
        /// <param name="Surge">move distance back to front, range(-100, 100), 0 is no move, increase from back position to front postion, 6 dimensional hardware only</param>
        /// <param name="Sway">move distance left to right, range(-100, 100), 0 is no move, increase from left position to right postion, 6 dimensional hardware only</param>
        /// <param name="Heave">move distance low to high, range(-100, 100), 0 is no move, increase from low position to high postion, 3 dimensional hardware or 6 dimensional hardware only</param>
        /// <param name="Speed">player move speed</param>
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool DynamicInterface(float Time, float Pitch, float Roll, float Yaw, float Surge, float Sway, float Heave, float Speed);

        /// <summary>
        /// hardware motion control interface,update hardware attitude by time
        /// </summary>
        /// <param name="Time">time since start game</param>
        /// <param name="Pitch">lean angle front to back, range(-18/180PI,18/180PI), 0 is no lean, increase from front to back</param>
        /// <param name="Roll">lean angle left to right, range(-18/180PI-18,180PI), 0 is no lean, increase from left to right</param>
        /// <param name="Yaw">turn angle left to right, range(-18/180PI-18,180PI), 0 is no turning, increase from left to right, 6 dimensional or some special hardware only</param>
        /// <param name="Surge">move distance back to front, range(-100, 100), 0 is no move, increase from back position to front postion, 6 dimensional hardware only</param>
        /// <param name="Sway">move distance left to right, range(-100, 100), 0 is no move, increase from left position to right postion, 6 dimensional hardware only</param>
        /// <param name="Heave">move distance low to high, range(-100, 100), 0 is no move, increase from low position to high postion, 3 dimensional hardware or 6 dimensional hardware only</param>
        /// <param name="ShakeType">hardwatr shake type, range(0, 6), interger only, 0 is no shake, 1 is front lean to back lean shake, 2 is left lean to right lean, 3 is left turn to right turn shake, 4 is back move to front move shake, 5 is left move to right move shake, 6 is low move to high move shake</param>
        /// <param name="Cycle">shake cycle, range(1, 4), multiply 100ms is the time reach the shake most value </param>
        /// <param name="Amplitude">strength of shake, rang(-1.0, 1.0), signe is the start direction, positive represents shake start is right, up, back, negtive represents start is left, down, forward</param>
        /// <param name="Speed">player move speed</param>
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool MotionControl(float Time, float Pitch, float Roll, float Yaw, float Surge, float Sway, float Heave, float ShakeType, float Cycle, float Amplitude, float Speed);

        /// <summary>
        /// environment FX control
        /// </summary>
        /// <param name="Status">switch on or off, 0 is off, 1 is on</param>
        /// <param name="EffectType">FX type,0 is wind, 1 is rain, 2 is snow, 3 is electric, 4 is gas, 5 is gasbubble, 6 is fog, 7 is beat on leg, 8 is beat on back, 9 is beat on hip, 10 is perfume, 11 is fire, 12 ghost</param>
        [DllImport("XDinterface", CharSet = CharSet.Auto)]
        static extern bool EnvironmentEffect(int Status, int EffectType);



        public static bool IsXDPlayerStart
        {
            get { return m_IsXDPlayerStart; }
        }
        private static bool m_initInstance = false;
        private static bool m_IsXDPlayerStart = false;
        private void Awake()
        {
            m_initInstance = InitInstance();
        }
        private void Start()
        {
            m_IsXDPlayerStart = GameStart();
        }
        private void OnDestroy()
        {
            GameStop();
            m_IsXDPlayerStart = false;
        }
        private void OnApplicationQuit()
        {
            ExitInstance();
        }


        /// <summary>
        /// hardware motion control interface,update hardware attitude by time
        /// </summary>
        /// <param name="Time">time since start game</param>
        /// <param name="Pitch">lean angle front to back, range(-18/180PI,18/180PI), 0 is no lean, increase from front to back</param>
        /// <param name="Roll">lean angle left to right, range(-18/180PI-18,180PI), 0 is no lean, increase from left to right</param>
        /// <param name="Yaw">turn angle left to right, range(-18/180PI-18,180PI), 0 is no turning, increase from left to right, 6 dimensional or some special hardware only</param>
        /// <param name="Surge">move distance back to front, range(-100, 100), 0 is no move, increase from back position to front postion, 6 dimensional hardware only</param>
        /// <param name="Sway">move distance left to right, range(-100, 100), 0 is no move, increase from left position to right postion, 6 dimensional hardware only</param>
        /// <param name="Heave">move distance low to high, range(-100, 100), 0 is no move, increase from low position to high postion, 3 dimensional hardware or 6 dimensional hardware only</param>
        /// <param name="ShakeType">hardwatr shake type, range(0, 6), interger only, 0 is no shake, 1 is front lean to back lean shake, 2 is left lean to right lean, 3 is left turn to right turn shake, 4 is back move to front move shake, 5 is left move to right move shake, 6 is low move to high move shake</param>
        /// <param name="Cycle">shake cycle, range(1, 4), multiply 100ms is the time reach the shake most value </param>
        /// <param name="Amplitude">strength of shake, rang(-1.0, 1.0), signe is the start direction, positive represents shake start is right, up, back, negtive represents start is left, down, forward</param>
        /// <param name="Speed">player move speed</param>
        public static void UpdateHardwareMotion(float Time, float Pitch, float Roll, float Yaw, float Surge, float Sway, float Heave, float ShakeType, float Cycle, float Amplitude, float Speed)
        {
            if (m_IsXDPlayerStart)
                MotionControl(Time, Pitch, Roll, Yaw, Surge, Sway, Heave, ShakeType, Cycle, Amplitude, Speed);
        }

        /// <summary>
        /// environment FX control
        /// </summary>
        /// <param name="Status">switch on or off, 0 is off, 1 is on</param>
        /// <param name="EffectType">FX type,0 is wind, 1 is rain, 2 is snow, 3 is electric, 4 is gas, 5 is gasbubble, 6 is fog, 7 is beat on leg, 8 is beat on back, 9 is beat on hip, 10 is perfume, 11 is fire, 12 ghost</param>
        public static void UpdateEnvironmentEffect(int Status, int EffectType)
        {
            if (m_IsXDPlayerStart)
                EnvironmentEffect(Status, EffectType);
        }
    }
}