using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Game.Runtime
{
    public class ProcedureEditorMode : ProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            int id = ClientApp.UI.OpenUIForm(AssetUtility.GetUIPath("Basics"), "UI");
            ClientApp.UI.CloseUIForm(id);
            id = ClientApp.UI.OpenUIForm(AssetUtility.GetUIPath("Basics"), "UI");
            Debug.Log("进入编辑器资源模式");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }
    }
}
