﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
namespace Game.Runtime
{
    public class UIRoot : MonoBehaviour
    {
        private void Awake()
        {
            Stage.inst.gameObject.transform.SetParent(gameObject.transform.parent);
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
