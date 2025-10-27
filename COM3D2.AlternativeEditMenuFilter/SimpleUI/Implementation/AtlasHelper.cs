using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace COM3D2.SimpleUI.Implementation
{
    public class AtlasHelper: MonoBehaviour
    {
        readonly Dictionary<string, UIAtlas> lookup = new Dictionary<string, UIAtlas>();

        bool loadComplete = false;
        string[] pendingAtlasLoadList;

        public void Init(string[] atlasNameList)
        {
            this.pendingAtlasLoadList = atlasNameList;
        }


        public void Start()
        {
            var result = Resources.LoadAsync<UIAtlas>("");
            //result.asset
        }

        public UIAtlas GetAtlas(string name)
        {
            if (!loadComplete)
            {
                
            }
            return null;
        }






    }
}
