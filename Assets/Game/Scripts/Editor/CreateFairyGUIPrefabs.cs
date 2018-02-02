using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using GameFramework;
using System;

namespace Game.Editor
{
     class CreateFairyGUIPrefabs : AssetPostprocessor
     {
        static CreateFairyGUIPrefabs()
        {
            EditorApplication.update += Update;
        }


        public static void OnPostprocessAllAssets(
           String[] importedAssets,
           String[] deletedAssets,
           String[] movedAssets,
           String[] movedFromAssetPaths)
        {
            List<string> importedKeys = new List<string>() { "Assets/Script" };
            for (int i = 0; i < importedAssets.Length; i++)
            {
                for (int j = 0; j < importedKeys.Count; j++)
                {
                    if (importedAssets[i].Contains(importedKeys[j]))
                    {
                        PlayerPrefs.SetInt("ImportScripts", 1);
                        return;
                    }
                }
            }
        }

        private static void Update()
        {
            bool importScripts = Convert.ToBoolean(PlayerPrefs.GetInt("ImportScripts", 1));
            if (importScripts && !EditorApplication.isCompiling)
            {
                OnUnityScripsCompilingCompleted();
                importScripts = false;
                PlayerPrefs.SetInt("ImportScripts", 0);
                EditorApplication.update -= Update;
            }
        }

        private static void OnUnityScripsCompilingCompleted()
        {
            Debug.Log("Unity Scrips Compiling completed.");
        }


        private static string resPath = "Game/BuildResources/UI/Res/"; //输出目录
        private static string OutPrefabsPath = "Game/BuildResources/UI/Prefabs/"; //输出目录
        [MenuItem("Game/CreateFairyGUIPrefabs", false, 10)]
        private static void CreatePrefabs()
        {
            if (!Directory.Exists(OutPrefabsPath))
                Directory.CreateDirectory(OutPrefabsPath);

            string[] files = Directory.GetFiles(Utility.Path.GetCombinePath(Application.dataPath, resPath), "*.*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < files.Length; ++i)
            {
                string file = files[i];
                string resFolder = "Assets/";
                int pos = file.IndexOf(resFolder);
                string assetpath = file.Substring(pos);
                string extension = Path.GetExtension(assetpath);

                if (extension.CompareTo(".meta") != 0)
                {
                    string name = Path.GetFileNameWithoutExtension(assetpath);
                    string[] names = name.Split('@');
                    string prefabName = names[0];
                    string OutPrefabPathName = Utility.Path.GetCombinePath("Assets/", OutPrefabsPath, prefabName + ".prefab");
                    GameObject prefab = null;

                    if (!File.Exists(OutPrefabPathName))
                    {
                        GameObject go = new GameObject();
                        go.name = prefabName;
                        Debug.Log("OutPrefabPathName: " + OutPrefabPathName);
                        prefab = PrefabUtility.CreatePrefab(OutPrefabPathName, go);
                        GameObject.DestroyImmediate(go);
                    }
                    else
                    {
                        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(OutPrefabPathName);
                    }

                    FairyGUIAssets assets = prefab.GetOrAddComponent<FairyGUIAssets>();
                    if (extension == ".bytes")
                    {
                        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetpath);
                        if (!assets.AllBytes.Contains(textAsset))
                            assets.AllBytes.Add(textAsset);
                    }
                    else if (extension == ".png")
                    {
                        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(assetpath);
                        if (!assets.AllTexture2D.Contains(texture))
                            assets.AllTexture2D.Add(texture);
                    }
                    else if (extension == ".wav")
                    {
                        AudioClip audio = AssetDatabase.LoadAssetAtPath<AudioClip>(assetpath);
                        if (!assets.AllAudioClip.Contains(audio))
                            assets.AllAudioClip.Add(audio);

                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }

}

