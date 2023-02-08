using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;


namespace Galo
{
    [InitializeOnLoad]
    public class EasyVersion : MonoBehaviour
    {
        public TextMeshProUGUI versionText;
        public string currentVersion;

        [ContextMenu("Update Version")]
        public void UpdateVersion()
        {
            string newVersion = IncreaseVersions();
            versionText.text = currentVersion = newVersion;
        }

#if UNITY_EDITOR

        public int callbackOrder => 0;

        string IncreaseVersions()
        {
            IncreasePlatformVersion();
            return IncreaseBuild();

        }

        string IncreaseBuild()
        {
            return IncrementVersion(new[] { 0, 0, 1 });
        }

        void IncreaseMinor()
        {
            IncrementVersion(new[] { 0, 1, 0 });
        }

        void IncreaseMajor()
        {
            IncrementVersion(new[] { 1, 0, 0 });
        }

        void IncreasePlatformVersion()
        {
            PlayerSettings.Android.bundleVersionCode += 1;
        }

        string IncrementVersion(int[] version)
        {
            string[] lines = PlayerSettings.bundleVersion.Split('.');

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                bool isNumber = int.TryParse(lines[i], out int numberValue);

                if (isNumber && version.Length - 1 >= i)
                {
                    if (i > 0 && version[i] + numberValue > 9)
                    {
                        version[i - 1]++;

                        version[i] = 0;
                    }
                    else
                    {
                        version[i] += numberValue;
                    }
                }
            }

            PlayerSettings.bundleVersion = $"{version[0]}.{version[1]}.{version[2]}";
            return PlayerSettings.bundleVersion;
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            bool shouldIncrement = EditorUtility.DisplayDialog(
                "Increment Version?",
                $"Current: {PlayerSettings.bundleVersion}",
                "Yes",
                "No"
            );

            if (shouldIncrement) IncreaseVersions();
        }

#endif
    }
}
