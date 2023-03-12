using System.Linq;
using UnityEditor;
using UnityEngine;

public static class FIndMissingScripts
{
    [MenuItem("My Menu/ Find Missing scripts in project")]
    static void FindMissingScriptsInMenuProjectItems()
    {
        string[] prefabPaths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();
        foreach(string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            foreach(Component component in prefab.GetComponentsInChildren<Component>())
            {
                 if(component == null)
                {
                    Debug.Log("Prefab Found with missinng Script" + path,prefab);
                    break;
                }
            }
        }

    }
    
}
