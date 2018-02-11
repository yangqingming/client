using UnityEngine;
using UnityEditor;
using UnityGameFramework.Runtime;
using Game.Runtime;
namespace Game.Editor
{
    internal static class CreateGamePrefabs
    {
        [MenuItem("Game/CreateGamePrefabs", false, 10)]
        private static void CreateGamePrefab()
        {
            GameObject game = new GameObject("[Game]");
            game.GetOrAddComponent<ClientApp>();
            GameObject Base = new GameObject("[Base]");
            Base.GetOrAddComponent<BaseComponent>();
            Base.GetOrAddComponent<EditorResourceComponent>();
            Base.transform.SetParent(game.transform, false);

            GameObject DataNode = new GameObject("DataNode");
            DataNode.GetOrAddComponent<DataNodeComponent>();
            DataNode.transform.SetParent(Base.transform, false);

            GameObject DataTable = new GameObject("DataTable");
            DataTable.GetOrAddComponent<DataTableComponent>();
            DataTable.transform.SetParent(Base.transform, false);

            GameObject Debugger = new GameObject("Debugger");
            Debugger.GetOrAddComponent<DebuggerComponent>();
            Debugger.transform.SetParent(Base.transform, false);

            GameObject Download = new GameObject("Download");
            Download.GetOrAddComponent<DownloadComponent>();
            Download.transform.SetParent(Base.transform, false);

            GameObject Entity = new GameObject("Entity");
            Entity.GetOrAddComponent<EntityComponent>();
            Entity.transform.SetParent(Base.transform, false);

            GameObject Event = new GameObject("Event");
            Event.GetOrAddComponent<EventComponent>();
            Event.transform.SetParent(Base.transform, false);

            GameObject FSM = new GameObject("FSM");
            FSM.GetOrAddComponent<FsmComponent>();
            FSM.transform.SetParent(Base.transform, false);

            GameObject Localization = new GameObject("Localization");
            Localization.GetOrAddComponent<LocalizationComponent>();
            Localization.transform.SetParent(Base.transform, false);

            GameObject Network = new GameObject("Network");
            Network.GetOrAddComponent<NetworkComponent>();
            Network.transform.SetParent(Base.transform, false);

            GameObject ObjectPool = new GameObject("ObjectPool");
            ObjectPool.GetOrAddComponent<ObjectPoolComponent>();
            ObjectPool.transform.SetParent(Base.transform, false);

            GameObject Procedure = new GameObject("Procedure");
            Procedure.GetOrAddComponent<ProcedureComponent>();
            Procedure.transform.SetParent(Base.transform, false);

            GameObject Resource = new GameObject("Resource");
            Resource.GetOrAddComponent<ResourceComponent>();
            Resource.transform.SetParent(Base.transform, false);

            GameObject Scene = new GameObject("Scene");
            Scene.GetOrAddComponent<SceneComponent>();
            Scene.transform.SetParent(Base.transform, false);

            GameObject Setting = new GameObject("Setting");
            Setting.GetOrAddComponent<SettingComponent>();
            Setting.transform.SetParent(Base.transform, false);

            GameObject Sound = new GameObject("Sound");
            Sound.GetOrAddComponent<SoundComponent>();
            Sound.transform.SetParent(Base.transform, false);

            GameObject UI = new GameObject("UI");
            UI.GetOrAddComponent<UIComponent>();
            UI.transform.SetParent(Base.transform, false);

            GameObject UIRoot = new GameObject("UIRoot");
            UIRoot.GetOrAddComponent<UIRoot>();

            GameObject WebRequest = new GameObject("WebRequest");
            WebRequest.GetOrAddComponent<WebRequestComponent>();
            WebRequest.transform.SetParent(Base.transform, false);

        }
    }
}

