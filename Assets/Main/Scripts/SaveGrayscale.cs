using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class SaveGrayscale : MonoBehaviour
{

    private IntPtr instance;
    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "[up_base] : ";

    [DllImport("up_base", EntryPoint = "com_tinker_up_base_create")]
    private static extern IntPtr _Create();

    [DllImport("up_base", EntryPoint = "com_tinker_up_base_echo", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static extern IntPtr _Echo(IntPtr instance, string input);

    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        instance = _Create();
        string inputText = "WHello";
        logger.Log(kTAG, "Loaded the plugin : " + Marshal.PtrToStringAnsi(_Echo(instance, inputText)));
    }
}
