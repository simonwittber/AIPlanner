using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;



public class HTNView : EditorWindow
{
    [MenuItem("Window/HTN View")]
    static void Open()
    {
        var window = EditorWindow.GetWindow<HTNView>();
        window.Show();
    }



    void OnEnable()
    {

    }

    void OnGUI()
    {

    }
}
