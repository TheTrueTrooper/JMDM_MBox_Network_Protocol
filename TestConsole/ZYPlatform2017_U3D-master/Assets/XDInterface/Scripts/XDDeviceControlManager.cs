using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZhuoYuan.XDControl;

public class XDDeviceControlManager : MonoBehaviour
{
    public static XDDeviceControlManager Instance;

    public XDControlMode m_ControlMode = XDControlMode.None;

    private bool m_IsStartHardwearMotion;
    private float m_XDDeviceGameTime;
    private float m_MotionScalefactor = 1;
    private float m_ShakeScalefactor = 1;
    private float Pitch, Roll, Yaw, Surge, Sway, Heave, Speed, ShakeType, ShakeCycle, ShakeAmplitute;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            enabled = false;
        }
    }

    void Start()
    {
        GetComponent<Animator>().enabled = false;
        ZyINIClass zyini = new ZyINIClass(Application.dataPath + "/ZyData/GameManger/VersionControl.ini");
        string version = zyini.ReadValue("HardwareMotion", "MotionScale");
        float.TryParse(version.Trim(), out m_MotionScalefactor);
        string shakeScake = zyini.ReadValue("HardwareShake", "ShakeScale");
        float.TryParse(shakeScake.Trim(), out m_ShakeScalefactor);
    }

    public void StartHardwareMotion()
    {
        GetComponent<Animator>().enabled = true;
        m_IsStartHardwearMotion = true;
    }

    void Update()
    {
        m_XDDeviceGameTime += Time.deltaTime;
        if (!m_IsStartHardwearMotion)
        {
            return;
        }
        if (XDDeviceControl.IsXDPlayerStart)
        {
            Vector3 RotationEulerAngle = transform.rotation.eulerAngles;
            XDDeviceControl.UpdateHardwareMotion(m_XDDeviceGameTime, ConvertValue(RotationEulerAngle.x), ConvertValue(RotationEulerAngle.z), ConvertValue(RotationEulerAngle.y), 0, 0, 0, ShakeType, ShakeCycle, ShakeAmplitute * m_ShakeScalefactor, 0);
        }
    }

    void OnDestroy()
    {
        StopXDDeviceShake();
        StopAllEvironmentFX();
        m_IsStartHardwearMotion = false;
    }

    /// <summary>
    /// Shake XDDevice
    /// </summary>
    /// <param name="shakeType">hardwatr shake type, range(0, 6), interger only, 0 is no shake, 1 is front lean to back lean shake, 2 is left lean to right lean, 3 is left turn to right turn shake, 4 is back move to front move shake, 5 is left move to right move shake, 6 is low move to high move shake</param>
    /// <param name="shakeCycle">shake cycle, range(1, 4), multiply 100ms is the time reach the shake most value </param>
    /// <param name="shakeAmplitute">strength of shake, rang(-1.0, 1.0), signe is the start direction, positive represents shake start is right, up, back, negtive represents start is left, down, forward</param>
    public void PlayXDDeviceShake(float shakeType, float shakeCycle, float shakeAmplitute)
    {
        ShakeType = shakeType;
        ShakeCycle = shakeCycle;
        ShakeAmplitute = shakeAmplitute;
    }

    /// <summary>
    /// stop XDDeviceShake
    /// </summary>
    public void StopXDDeviceShake()
    {
        ShakeType = 0;
    }

    /// <summary>
    /// Shake XDDevice
    /// </summary>
    /// <param name="shakeType">hardwatr shake type, range(0, 6), interger only, 0 is no shake, 1 is front lean to back lean shake, 2 is left lean to right lean, 3 is left turn to right turn shake, 4 is back move to front move shake, 5 is left move to right move shake, 6 is low move to high move shake</param>
    /// <param name="shakeCycle">shake cycle, range(1, 4), multiply 100ms is the time reach the shake most value </param>
    /// <param name="shakeAmplitute">strength of shake, rang(-1.0, 1.0), signe is the start direction, positive represents shake start is right, up, back, negtive represents start is left, down, forward</param>
    /// <param name="duration">shake during a time duration</param>
    public void PlayShakeForAWhile(float shakeType, float shakeCycle, float shakeAmplitute, float duration)
    {
        StartCoroutine(PlayXDDeviceShakeForAWhile(shakeType, shakeCycle, shakeAmplitute, duration));
    }

    /// <summary>
    /// play simple fall land shake
    /// </summary>
    public void PlaySimpleLandShake()
    {
        PlayShakeForAWhile(6, 2, 0.2f, 0.2f);
    }

    public void ChangeEnvironmentFX(XDEvironmentFXKind kind, bool IsOpen)
    {
        XDDeviceControl.UpdateEnvironmentEffect(IsOpen ? 1 : 0, (int)kind);
    }

    private float ConvertValue(float inFloat)
    {
        inFloat = inFloat > 90 ? inFloat - 360 : inFloat;
        inFloat = inFloat < -90 ? inFloat + 360 : inFloat;

        return inFloat * m_MotionScalefactor;
    }

    private IEnumerator PlayXDDeviceShakeForAWhile(float shakeType, float shakeCycle, float shakeAmplitute, float duration)
    {
        PlayXDDeviceShake(shakeType, shakeCycle, shakeAmplitute);

        yield return new WaitForSeconds(duration);

        PlayXDDeviceShake(0, shakeCycle, shakeAmplitute);
    }

    public void StopAllEvironmentFX()
    {
        ChangeEnvironmentFX(XDEvironmentFXKind.Wind, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Rain, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Snow, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Electric, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Gas, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.GasBubble, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Fog, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.BeatLeg, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.BeatBack, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.BeatHip, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Perfume, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Fire, false);
        ChangeEnvironmentFX(XDEvironmentFXKind.Ghost, false);
    }
}

public enum XDEvironmentFXKind
{
    Wind = 0,
    Rain,
    Snow,
    Electric,
    Gas,
    GasBubble,
    Fog,
    BeatLeg,
    BeatBack,
    BeatHip,
    Perfume,
    Fire,
    Ghost,
}

public enum XDControlMode
{
    None,
    AnimationMode,
    GamePlayMode,
}
