using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Diagnostics;
public class launcherHost : MonoBehaviour
{
    public InputField textInput;
    public void runFile()
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.FileName = textInput.text;
        p.Start();
    }
}
