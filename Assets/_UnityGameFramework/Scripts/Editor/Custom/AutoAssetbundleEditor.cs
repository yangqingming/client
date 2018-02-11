using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AutoAssetbundleEditor : EditorWindow
    {
        private  AssetBundleEditorController m_Controller = null;
        private List<SourceAsset> SourceAssetAllList = new List<SourceAsset>();
        [MenuItem("Game/AutoAssetbundleEditor", false, 10)]
        static void OpenAutoAssetBundleCollection()
        {
            AutoAssetbundleEditor window = GetWindow<AutoAssetbundleEditor>(true, "Auto AssetBundle Editor", true);
            window.minSize = new Vector2(500, 350);
            window.maxSize = new Vector2(500, 350);
        }

        private void OnEnable()
        {
            m_Controller = new AssetBundleEditorController();
            m_Controller.OnLoadingAssetBundle += OnLoadingAssetBundle;
            m_Controller.OnLoadingAsset += OnLoadingAsset;
            m_Controller.OnLoadCompleted += OnLoadCompleted;

            if (m_Controller.Load())
            {
                Debug.Log("Load configuration success.");
            }
            else
            {
                Debug.LogWarning("Load configuration failure.");
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));
                {
                    if (GUILayout.Button("Auto", GUILayout.Width(500), GUILayout.Height(350)))
                    {
                        ScanAllAsset(m_Controller.SourceAssetRoot);
                        AutoMake();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        Dictionary<string, int> refCount = new Dictionary<string, int>();
        void addRef(string path)
        {
            if (!refCount.ContainsKey(path))
            {
                refCount.Add(path, 1);
            }
            else
                refCount[path] += 1;
        }

        int GetRef(string path)
        {
            if (!refCount.ContainsKey(path))
                return 0;
            return refCount[path];
        }

        void AutoMake()
        {
           List<SourceAsset> sourceAssetList = SourceAssetAllList.Where(sourceAsset => m_Controller.GetAsset(sourceAsset.Guid) == null).ToList();
           refCount.Clear();
           for (int i = 0; i < sourceAssetList.Count; ++i)
           {
                string extension = Path.GetExtension(sourceAssetList[i].Path);
                if (extension.CompareTo(".prefab") == 0)
                {
                    string[] depends = AssetDatabase.GetDependencies(sourceAssetList[i].Path);
                    for (int j = 0; j < depends.Length; ++j)
                    {
                        addRef(depends[j]);
                    }
                }
           }

            for (int i = 0; i < sourceAssetList.Count; ++i)
            {
                MakeUIRule(sourceAssetList[i]);
                MakeModel(sourceAssetList[i]); 
            }

            m_Controller.Save();
        }

        void MakeModel(SourceAsset asset)
        {
            string[] ret = asset.FromRootPath.Split('/');
            if (ret.Length < 3)
                return;

            string folderName = ret[0];
            if (folderName.CompareTo("Models") != 0)
                return;
            string Extension = Path.GetExtension(asset.FromRootPath);
            if (Extension.ToLower().CompareTo(".fbx") == 0 || Extension.ToLower().CompareTo(".mat") == 0)
            {
                int count = GetRef(asset.Path);
                string AssetBundleName = "";
                if (count > 1)
                {
                    int dotIndex = asset.FromRootPath.IndexOf('.');
                    string assetBundleName = dotIndex > 0 ? asset.FromRootPath.Substring(0, dotIndex) : asset.FromRootPath;
                    string[] names = assetBundleName.Split('@');
                    AssetBundleName = names[0] + Extension;

                    if (m_Controller.AddAssetBundle(AssetBundleName, null, AssetBundleLoadType.LoadFromFile, false))
                    {
                        AssetBundle assetBundle = m_Controller.GetAssetBundle(AssetBundleName, null);
                        if (assetBundle == null)
                        {
                            Debug.LogError("添加AssetBundle错误： " + AssetBundleName);
                            return;
                        }
                        if (!m_Controller.AssignAsset(asset.Guid, assetBundle.Name, assetBundle.Variant))
                        {
                            Debug.LogWarning(string.Format("Assign asset '{0}' to AssetBundle '{1}' failure.", asset.Name, assetBundle.FullName));
                        }
                    }
                }

            }
            else if (".prefab".CompareTo(Extension) == 0)
            {
                string AssetBundleName = "";
                AssetBundleName = asset.FromRootPath;

                if (m_Controller.AddAssetBundle(AssetBundleName, null, AssetBundleLoadType.LoadFromFile, false))
                {
                    AssetBundle assetBundle = m_Controller.GetAssetBundle(AssetBundleName, null);
                    if (assetBundle == null)
                    {
                        Debug.LogError("添加AssetBundle错误： " + AssetBundleName);
                        return;
                    }
                    if (!m_Controller.AssignAsset(asset.Guid, assetBundle.Name, assetBundle.Variant))
                    {
                        Debug.LogWarning(string.Format("Assign asset '{0}' to AssetBundle '{1}' failure.", asset.Name, assetBundle.FullName));
                    }
                }
            }
        }


        void MakeUIRule(SourceAsset asset)
        {
            string[] ret = asset.FromRootPath.Split('/');
            if (ret.Length < 3)
                return;

            string folderName = ret[0];
            if (folderName.CompareTo("UI") != 0)
                return;
            string Name = Path.GetFileNameWithoutExtension(asset.Path);
            string extension = Path.GetExtension(asset.Path);
            string AssetBundleName = "";
            for (int i = 0; i < ret.Length - 1; ++i)
            {
                AssetBundleName += ret[i] + "/";
            }
            AssetBundleName += Name.Split('@')[0] + extension;

            if (m_Controller.AddAssetBundle(AssetBundleName, null, AssetBundleLoadType.LoadFromFile, false))
            {
                AssetBundle assetBundle = m_Controller.GetAssetBundle(AssetBundleName, null);
                if (assetBundle == null)
                {
                    Debug.LogError("添加AssetBundle错误： " + AssetBundleName + extension);
                    return;
                }
                if (!m_Controller.AssignAsset(asset.Guid, assetBundle.Name, assetBundle.Variant))
                {
                    Debug.LogWarning(string.Format("Assign asset '{0}' to AssetBundle '{1}' failure.", asset.Name, assetBundle.FullName));
                }
            }
        }

        void ScanAllAsset(SourceFolder sourceFolder)
        {
            SourceAsset[] assets = sourceFolder.GetAssets();

            for (int i = 0; i < assets.Length; ++i)
            {
                SourceAssetAllList.Add(assets[i]);
            }
            SourceFolder[] foldrs = sourceFolder.GetFolders();
            for (int i = 0; i < foldrs.Length; ++i)
            {
                ScanAllAsset(foldrs[i]);
            }
        }

        private void OnLoadingAssetBundle(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading AssetBundles", string.Format("Loading AssetBundles, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Assets", string.Format("Loading assets, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadCompleted()
        {
            EditorUtility.ClearProgressBar();
        }
    }
}