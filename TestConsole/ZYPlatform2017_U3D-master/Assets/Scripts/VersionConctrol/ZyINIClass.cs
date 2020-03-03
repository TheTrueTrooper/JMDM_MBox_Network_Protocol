using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class ZyINIClass {

    public string inipath;
    public ZyINIClass(string INIPath) {
        this.inipath = INIPath;
    }
    
    public void WriteValue(string Section, string Key, string Value) {
        WritePrivateProfileString(Section, Key, Value, this.inipath);
    }

    public string ReadValue(string Section, string Key) {
        StringBuilder stringBuilder = new StringBuilder(500);
        int privateProfileString = ZyINIClass.GetPrivateProfileString(Section, Key, string.Empty, stringBuilder, 500, this.inipath);
        if (privateProfileString > 0) {
            return stringBuilder.ToString();
        }
        return null;
    }

    public bool ExistINIFile() {
        return File.Exists(this.inipath);
    }

    /// <summary>
    /// 写入配置文件
    /// </summary>
    /// <param name="section">片段名</param>
    /// <param name="key">参数名</param>
    /// <param name="val">参数保存变量</param>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    [DllImport("kernel32",CharSet =CharSet.Auto)]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

    /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <param name="section">片段名</param>
    /// <param name="key">参数名</param>
    /// <param name="def">默认值</param>
    /// <param name="retVal">参数保存变量</param>
    /// <param name="size">数据大小</param>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    [DllImport("kernel32", CharSet = CharSet.Auto)]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
}
