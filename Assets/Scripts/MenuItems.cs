using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;


namespace Galo
{
    public static class MenuItems
    {

        #region Main Menu Tools

        [MenuItem("GameObject/Galo Tools/Add Pause Trigger", false, 10)]
        private static void CreatePauseTrigger()
        {
            GameObject newObj = PrefabUtility.InstantiatePrefab(Resources.Load("Level Items/PauseTrigger", typeof(GameObject)), (Selection.activeGameObject != null) ? Selection.activeGameObject.transform : null) as GameObject;
            PrefabUtility.UnpackPrefabInstance(newObj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            Selection.activeGameObject = newObj;
        }

        #endregion



    }

}
#endif