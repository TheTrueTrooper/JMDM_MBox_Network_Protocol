using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMotion : MonoBehaviour {

    void TestControlDevice()
    {
        XDDeviceControlManager.Instance.StartHardwareMotion();

        Invoke("TestShake", 5f);

        Invoke("TestShake", 10f);

        Invoke("TestShake", 15f);

        Invoke("TestEffect", 10f);
    }

    void TestShake()
    {
        XDDeviceControlManager.Instance.PlaySimpleLandShake();
    }

    void TestEffect()
    {
        XDDeviceControlManager.Instance.ChangeEnvironmentFX(XDEvironmentFXKind.Wind, true);
    }
}
