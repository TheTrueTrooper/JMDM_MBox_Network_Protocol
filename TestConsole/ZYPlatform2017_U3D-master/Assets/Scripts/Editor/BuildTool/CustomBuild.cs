using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class CustomBuild : Editor
{

    [MenuItem("Tools/Build")]
    static void Bulid()
    {
        //System.Diagnostics.Process.Start("explorer.exe", @"E:\MyExe\7D");
        //return;
        EditorBuildSettingsScene[] levels = EditorBuildSettings.scenes;
        List<string> sceneName = new List<string>();

        for (int i = 0; i < levels.Length; ++i)
        {
            if (levels[i].enabled)
            {
                sceneName.Add(levels[i].path);
            }
        }
        if (EditorApplication.SaveScene(EditorApplication.currentScene))
        {

            string saveName = EditorUtility.SaveFilePanel("Save Scene", EditorApplication.applicationPath, ".exe", "exe");
            if (saveName != null && saveName != string.Empty)
            {
                //Debug.Log(saveName);
                BuildPipeline.BuildPlayer(sceneName.ToArray(), saveName, BuildTarget.StandaloneWindows, BuildOptions.None);
                string destDirectory = saveName.Substring(0, saveName.LastIndexOf(".", saveName.Length));
                //string exeName = saveName.Substring(saveName.LastIndexOf("/", saveName.Length), saveName.Length - saveName.LastIndexOf("/", saveName.Length) - 4);
                CopyDirectory(Application.dataPath + "/ZyData", destDirectory + "_Data" + "/ZyData");
                string openFileName = saveName.Substring(0, saveName.LastIndexOf("/", saveName.Length));
                openFileName = openFileName.Replace("/", @"\");
                System.Diagnostics.Process.Start("explorer.exe", openFileName);
            }
            else
            {
                Debug.Log("取消保存");
            }
        }
    }



    static void CopyDirectory(string sourceDirectory, string destDirectory)
    {
        if (!Directory.Exists(destDirectory))
        {
            Directory.CreateDirectory(destDirectory);
        }

        CopyFile(sourceDirectory, destDirectory);

        string[] directionNames = Directory.GetDirectories(sourceDirectory);
        foreach (string directionPath in directionNames)
        {
            string directionPathTemp = destDirectory + "\\" + directionPath.Substring(sourceDirectory.Length + 1);

            CopyDirectory(directionPath, directionPathTemp);
        }
    }

    static void CopyFile(string sourcesFile, string destFile)
    {
        string[] fileName = Directory.GetFiles(sourcesFile);
        foreach (string filePath in fileName)
        {
            string filePahTemp = destFile + "\\" + filePath.Substring(sourcesFile.Length + 1);
            if (File.Exists(filePahTemp))
            {
                File.Copy(filePath, filePahTemp, true);
            }
            else
            {
                File.Copy(filePath, filePahTemp);
            }
        }

    }
}
