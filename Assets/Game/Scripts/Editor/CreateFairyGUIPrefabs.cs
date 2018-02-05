using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using GameFramework;
using System;
using System.Linq;
using Game.Runtime;

namespace Game.Editor
{
    class CreateFairyGUIPrefabs : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            Debug.Log("OnPreprocessTexture: " + this.assetPath);
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.mipmapEnabled = false;
                TextureImporterSettings tis = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(tis);
                tis.ApplyTextureType(TextureImporterType.Sprite);
                textureImporter.SetTextureSettings(tis);
            }
        }

        //void OnPostprocessTexture(Texture2D texture)
        //{
        //    TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        //    if (textureImporter != null)
        //    {
        //        textureImporter.textureType = TextureImporterType.Sprite;
        //        textureImporter.spriteImportMode = SpriteImportMode.Single;
        //        textureImporter.mipmapEnabled = false;
        //    }
        //}

        //////所有的资源的导入，删除，移动，都会调用此方法，注意，这个方法是static的  
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            Debug.Log("OnPostprocessAllAssets");
            foreach (string path in importedAsset)
            {
                
            }
            foreach (string path in deletedAssets)
            {
                Debug.Log("deletedAssets = " + path);
            }
            foreach (string path in movedAssets)
            {
                AssetDatabase.ImportAsset(path);
            }
            foreach (string path in movedFromAssetPaths)
            {
                Debug.Log("movedFromAssetPaths = " + path);
            }

            if (importedAsset.Length > 0)
            {
                EditorApplication.CallbackFunction func = delegate ()
                {
                    CreatePrefabs(importedAsset);
                };
                EditorApplication.delayCall += func;


            }
                
            else if (deletedAssets.Length > 0)
            {
                EditorApplication.CallbackFunction func = delegate ()
                {
                    deletePrefabs(deletedAssets);
                };
                EditorApplication.delayCall += func;
            }
                
        }

        private static string resPath = "Assets/Game/BuildResources/UI/Res/"; //输出目录
        private static string OutPrefabsPath = "Assets/Game/BuildResources/UI/Prefabs/"; //输出目录

        [MenuItem("Game/CreateFairyGUIPrefabs", false, 10)]
        private static void CreatePrefabs(string[] Paths)
        {
            if (!Directory.Exists(OutPrefabsPath))
                Directory.CreateDirectory(OutPrefabsPath);

            for (int i = 0; i < Paths.Length; ++i)
            {
                string assetpath = Paths[i];
                string extension = Path.GetExtension(assetpath);

                if (assetpath.Contains(resPath))
                {
                    string name = Path.GetFileNameWithoutExtension(assetpath);
                    string[] names = name.Split('@');
                    string prefabName = names[0];
                    string OutPrefabPathName = Utility.Path.GetCombinePath(OutPrefabsPath, prefabName + ".prefab");
                    GameObject prefab = null;
                    if (!File.Exists(OutPrefabPathName))
                    {
                        GameObject go = new GameObject();
                        go.name = prefabName;
                        Debug.Log("OutPrefabPathName: " + OutPrefabPathName);
                        prefab = PrefabUtility.CreatePrefab(OutPrefabPathName, go);
                        GameObject.DestroyImmediate(go);
                        AssetDatabase.ImportAsset(OutPrefabPathName);
                    }
                    else
                    {
                        prefab = AssetDatabase.LoadAssetAtPath<GameObject>(OutPrefabPathName);

                        if (prefab == null)
                        {
                            Debug.LogError("OutPrefabPathName: " + OutPrefabPathName);
                        }
                    }
                    prefab.GetOrAddComponent<UILogic>();
                    FairyGUIAssets assets = prefab.GetOrAddComponent<FairyGUIAssets>();
                    assets.AllBytes = assets.AllBytes.Where(a => a != null).ToList();
                    assets.AllTexture2D = assets.AllTexture2D.Where(a => a != null).ToList();
                    assets.AllAudioClip = assets.AllAudioClip.Where(a => a != null).ToList();
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

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void deletePrefabs(string[] Paths)
        {
            for (int i = 0; i < Paths.Length; ++i)
            {
                string assetPath = Paths[i];

                string name = Path.GetFileNameWithoutExtension(assetPath);
                string[] names = name.Split('@');
                string prefabName = names[0];

                if (assetPath.Contains(resPath))
                {
                    string OutPrefabPathName = Utility.Path.GetCombinePath(OutPrefabsPath, prefabName + ".prefab");
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(OutPrefabPathName);

                    if (prefab != null)
                    {
                        FairyGUIAssets assets = prefab.GetOrAddComponent<FairyGUIAssets>();
                        assets.AllBytes = assets.AllBytes.Where(a => a != null).ToList();
                        assets.AllTexture2D = assets.AllTexture2D.Where(a => a != null).ToList();
                        assets.AllAudioClip = assets.AllAudioClip.Where(a => a != null).ToList();

                        if (assets.AllBytes.Count == 0 && assets.AllTexture2D.Count == 0 && assets.AllAudioClip.Count == 0)
                        {
                            if (File.Exists(OutPrefabPathName))
                                File.Delete(OutPrefabPathName);
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}

