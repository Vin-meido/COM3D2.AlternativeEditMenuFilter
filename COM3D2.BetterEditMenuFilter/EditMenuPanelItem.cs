using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.BetterEditMenuFilter
{
	public class EditMenuPanelItem
	{
		public GameObject gameObject;
		public SceneEdit.SMenuItem menu;

		public bool Visible
        {
			get => gameObject.activeSelf;
			set => gameObject.SetActive(value);
        }

		public string Filename
        {
			get => menu.m_strMenuFileName;
        }

		public string Name
        {
			get => menu.m_strMenuName;
        }

		public string LocalizedName
        {
			get => menu.menuNameCurrentLanguage;
		}

		public string Info
        {
			get => menu.m_strInfo;
        }

		public string LocalizedInfo
        {
			get => menu.infoTextCurrentLanguage;

		}

		public bool IsMod { get; private set; }

		public bool IsVanilla { get; private set; }

		public bool IsCompat { get; private set; }

		public EditMenuPanelItem(GameObject go, SceneEdit.SMenuItem menu)
        {
			this.gameObject = go;
			this.menu = menu;

			if(GameUty.FileSystemMod.IsExistentFile(menu.m_strMenuFileName))
            {
				IsMod = true;
				IsVanilla = false;
            } else
            {
				IsMod = false;
				IsVanilla = true;
            }

			var oldTransform = go.transform.Find("Old");
			if (IsVanilla && oldTransform && oldTransform.gameObject.activeSelf)
			{
				IsCompat = true;
			} else
            {
				IsCompat = false;
            }
		}
	}
}
