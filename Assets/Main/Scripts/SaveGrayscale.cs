using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.IO;

public class SaveGrayscale : MonoBehaviour
{

    public string m_ImageStoreLocalPath = "Images";

    private IntPtr instance;
    private static ILogger logger = Debug.unityLogger;
    private static string kTAG = "[up_base] : ";

    [DllImport("up_base", EntryPoint = "com_tinker_up_base_create")]
    private static extern IntPtr _Create();

    [DllImport("up_base", EntryPoint = "com_tinker_up_base_echo", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static extern IntPtr _Echo(IntPtr instance, string input);

    [DllImport("up_base", EntryPoint = "com_tinker_up_base_save_black_and_white")]
    private static extern void _SaveBlackAndWhite(IntPtr instance, IntPtr bytes, int rows, int cols, int type);

    // Start is called before the first frame update
    void Start()
    {
        instance = _Create();
    }

    public void Initiate()
    {
        string inputText = "WHello";
        string responseText = Marshal.PtrToStringAnsi(_Echo(instance, inputText));
        logger.Log(kTAG, "Loaded the plugin : " + responseText);
    }

    public void SaveBlackAndWhite()
    {

        byte[] imageBytes = System.IO.File.ReadAllBytes(m_ImageStoreLocalPath);
        Texture2D tmpTexture = new Texture2D(1, 1);
        tmpTexture.LoadImage(imageBytes);
        Color32[] pixelData = tmpTexture.GetPixels32();

        GCHandle pixelHandle = GCHandle.Alloc(pixelData, GCHandleType.Pinned);
        IntPtr pixelPtr = pixelHandle.AddrOfPinnedObject();

         _SaveBlackAndWhite(instance, pixelPtr, 1250, 1763, 1);

        //free the pointer
        //Marshal.FreeHGlobal(pixelPtr);
    }

    private static byte[] Color32ArrayToByteArray(Color32[] colors)
    {
        int length = 4 * colors.Length;
        byte[] bytes = new byte[length];
        IntPtr ptr = Marshal.AllocHGlobal(length);
        Marshal.StructureToPtr(colors, ptr, true);
        Marshal.Copy(ptr, bytes, 0, length);
        Marshal.FreeHGlobal(ptr);
        return bytes;
    }
}
