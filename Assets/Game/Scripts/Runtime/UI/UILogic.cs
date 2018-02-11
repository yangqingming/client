using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using FairyGUI;
namespace Game.Runtime
{
    public class UILogic : UIFormLogic
    {

        private FairyGUIAssets fairyGUIAssets = null;
        private GComponent _mainView = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            fairyGUIAssets = gameObject.GetComponent<FairyGUIAssets>();

            string descName = System.IO.Path.GetFileNameWithoutExtension(this.UIForm.UIFormAssetName);

            TextAsset desc = GetUIAsset<TextAsset>(descName);

            UIPackage package = UIPackage.AddPackage(desc.bytes, descName,
                (string name, string extension, System.Type type) =>
                {
                    return GetUIAsset(name, type);
                }
            );

            _mainView = UIPackage.CreateObject(descName, "Main").asCom;
            GRoot.inst.AddChild(_mainView);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            _mainView.GetChild("bagBtn").asButton.onClick.Add(delegate (EventContext context)
            {
                GameEntry.Shutdown(ShutdownType.Restart);
                //GRoot.inst.AddChild(UIPackage.CreateObject("Bag", "BagWin").asCom);
            });
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            _mainView.Dispose();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnCover()
        {
            base.OnCover();
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        //===================================================================
        private Object GetUIAsset(string assetName, System.Type type)
        {
            if (type == typeof(TextAsset))
            {
                for (int i = 0; i < fairyGUIAssets.AllBytes.Count; ++i)
                {
                    TextAsset text = fairyGUIAssets.AllBytes[i];
                    if (text.name.CompareTo(assetName) == 0)
                    {
                        return text as Object;
                    }
                }
            }
            else if (type == typeof(Texture2D) || type == typeof(Texture))
            {
                for (int i = 0; i < fairyGUIAssets.AllTexture2D.Count; ++i)
                {
                    Texture texture = fairyGUIAssets.AllTexture2D[i];
                    if (texture.name.CompareTo(assetName) == 0)
                    {
                        return texture as Object;
                    }
                }
            }
            else if (type == typeof(AudioClip))
            {
                for (int i = 0; i < fairyGUIAssets.AllAudioClip.Count; ++i)
                {
                    AudioClip audio = fairyGUIAssets.AllAudioClip[i];
                    if (audio.name.CompareTo(assetName) == 0)
                    {
                        return audio as Object;
                    }
                }
            }

            Debug.LogWarning("没有找到该资源： " + assetName + "  Type: " + type.ToString());

            return null;
        }

        private T GetUIAsset<T>(string assetName) where T : Object
        {
            return GetUIAsset(assetName, typeof(T)) as T;
        }
    }

}
