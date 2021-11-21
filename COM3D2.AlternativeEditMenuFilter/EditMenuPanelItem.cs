using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.AlternativeEditMenuFilter
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


		private bool? _existentFile;

		private bool IsExistentFile
        {
			get
            {
				if (_existentFile == null)
                {
					_existentFile = GameUty.FileSystemMod.IsExistentFile(menu.m_strMenuFileName);
				}

				return (bool)_existentFile;
			}
        }

		public bool IsMod => IsExistentFile;

		public bool IsVanilla => !IsExistentFile;


		private bool? _isCompat;

		public bool IsCompat { 
			get
            {
				if(_isCompat == null)
                {
					_isCompat = IsVanilla && (gameObject.transform.Find("Old")?.gameObject.activeSelf ?? false);
				}

				return (bool)_isCompat;
			}
		}

		public EditMenuPanelItem(GameObject go, SceneEdit.SMenuItem menu)
        {
			this.gameObject = go;
			this.menu = menu;
		}
	}
}
