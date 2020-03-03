using UnityEngine;
using System.Collections;

public class ZyVersionControl : MonoBehaviour {

    static private ZyVersionControl m_instance;
    static public ZyVersionControl Getinstance() {
        if (m_instance == null) {
            m_instance = FindObjectOfType<ZyVersionControl>();
        }
        return m_instance;
    }
    public VERSION M_VERSION;


    void Awake() {
        m_instance = this;
        ZyINIClass zyini = new ZyINIClass(Application.dataPath + "/ZyData/GameManger/VersionControl.ini");
        string version = zyini.ReadValue("VersionControl", "version");
        switch (version) {
            case "English":
                M_VERSION = VERSION.EIGLISH;
                break;
            case "Chinese":
                M_VERSION = VERSION.CHINESE;
                break;
            default:
                Application.Quit();
                break;
        }

    } 
}

public enum VERSION {
    EIGLISH,
    CHINESE
}
