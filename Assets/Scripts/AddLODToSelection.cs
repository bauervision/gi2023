using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class MenuItems : MonoBehaviour
{

    [MenuItem("Tools/Add LOD To Selection", false, 30)]
    private static void AddLOD()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("Need something selected to add LOD's to first");
            return;
        }

        // create the empty parent
        GameObject go = new GameObject(Selection.activeGameObject.name);
        //center it to the current selection
        go.transform.parent = Selection.activeGameObject.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.parent = null;
        //go.transform.localRotation = Selection.activeGameObject.transform.localRotation;
        // now reparent the mesh
        Selection.activeGameObject.transform.parent = go.transform;


        // add LOD
        go.AddComponent<LODGroup>();
        MeshRenderer[] newMR = go.GetComponentsInChildren<MeshRenderer>();
        // set the initial cull to 10%
        LOD[] initialLOD = new LOD[] { new LOD(10f / 100f, newMR) };
        go.GetComponent<LODGroup>().SetLODs(initialLOD);
    }


    [MenuItem("Tools/Add LOD To Prefab", false, 30)]
    private static void UpdateLODOnPrefab()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogError("Need something selected to add LOD's to first");
            return;
        }

        // copy the current mesh
        GameObject go = GameObject.Instantiate(Selection.activeGameObject) as GameObject;
        go.name = Selection.activeGameObject.name + "_coreMesh";

        //center it to the current selection
        go.transform.parent = Selection.activeGameObject.transform;
        // add LOD
        Selection.activeGameObject.AddComponent<LODGroup>();
        MeshRenderer[] newMR = Selection.activeGameObject.GetComponentsInChildren<MeshRenderer>();
        // set the initial cull to 10%
        LOD[] initialLOD = new LOD[] { new LOD(10f / 100f, newMR) };
        Selection.activeGameObject.GetComponent<LODGroup>().SetLODs(initialLOD);

        ClearOutComponents(Selection.activeGameObject);
    }

    private static void ClearOutComponents(GameObject selection)
    {
        if (selection.GetComponent<BoxCollider>() != null)
            DestroyImmediate(selection.GetComponent<BoxCollider>());

        if (selection.GetComponent<MeshCollider>() != null)
            DestroyImmediate(selection.GetComponent<MeshCollider>());

        if (selection.GetComponent<MeshRenderer>() != null)
            DestroyImmediate(selection.GetComponent<MeshRenderer>());

        if (selection.GetComponent<MeshFilter>() != null)
            DestroyImmediate(selection.GetComponent<MeshFilter>());

    }

    private static void ResetSelectedTransform(GameObject selectedObject)
    {
        selectedObject.transform.localPosition = Vector3.zero;
        selectedObject.transform.localEulerAngles = Vector3.zero;
        selectedObject.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
#endif