﻿/* Copyright (c) 2022 dr. ext (Vladimir Sigalkin) */

using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
using System.Collections.Generic;

using TextMeshPro = TMPro.TextMeshProUGUI;

namespace extDebug.Menu.UGUI
{
	public class MenuRender : IDMRender, IDMRender_Update
	{
		#region Private Vars

		private readonly StringBuilder _builder = new StringBuilder();
		
		private GameObject _menuObject;

		private Text _label_Text;

		private TextMeshPro _label_TextMeshPro;

		#endregion

		#region Constructor

		public MenuRender(GameObject menuObject, Text menuLabel) : this(menuObject) => _label_Text = menuLabel;

		public MenuRender(GameObject menuObject, TextMeshPro menuLabel) : this(menuObject) =>
			_label_TextMeshPro = menuLabel;

		#endregion

		#region IDMRender Methods

		void IDMRender.Repaint(IDMBranch itemsContainer, IReadOnlyList<DMItem> items)
		{
			const string kPrefix = " ";
			const string kPrefix_Selected = ">";
			const string kSpace = "  ";
			const char kHorizontalChar = '-';

			CalculateLengths(itemsContainer, items, kSpace.Length, 
				out var fullLength, 
				out var maxNameLength, 
				out var maxValueLength, 
				out var maxDescriptionLength);

			var order = -1;
			var lineLength = fullLength + kPrefix.Length;
			
			var pageInfo = string.Empty;
			var hasPage = itemsContainer.PageSize > 0;
			if (hasPage)
			{
				pageInfo = $"{itemsContainer.PageStart}-{itemsContainer.PageEnd} of {itemsContainer.ItemsCount}";
				lineLength = Mathf.Max(pageInfo.Length, lineLength);
				items = itemsContainer.GetPageItems();
			}
			
			var lineEmpty = new string(kHorizontalChar, lineLength);

			// header
			_builder.Clear();
			_builder.AppendFormat($"{kPrefix}<color=#{ColorUtility.ToHtmlStringRGB(itemsContainer.NameColor)}>{{0,{-fullLength}}}</color>{Environment.NewLine}", itemsContainer.Name);
			_builder.AppendLine(lineEmpty);

			// items
			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var selected = item == itemsContainer.Current;
				var prefix = selected ? kPrefix_Selected : kPrefix;
				var alpha = item.IsEnabled() ? (selected ? 1 : 0.75f) : 0.50f;

				// items separator
				if (order >= 0 && Math.Abs(order - item.Order) > 1)
					_builder.AppendLine(lineEmpty);

				order = item.Order;

				var name = item.Name;
				var value = item.Value;
				var description = item.Description;

				if (item is IDMBranch)
					name += "...";
                
                // little hack
                if (itemsContainer is DMLogs)
                {
                    alpha = 1f;
                    maxValueLength = 0;
                }
                
				_builder.AppendFormat($"{prefix}<color=#{ColorUtility.ToHtmlStringRGB(item.NameColor * alpha)}>{{0,{-maxNameLength}}}</color>", name);
				_builder.AppendFormat($"{kSpace}<color=#{ColorUtility.ToHtmlStringRGB(item.ValueColor * alpha)}>{{0,{maxValueLength}}}</color>", value);
				_builder.AppendFormat($"{kSpace}<color=#{ColorUtility.ToHtmlStringRGB(item.DescriptionColor * alpha)}>{{0,{-maxDescriptionLength}}}</color>", description);
				_builder.Append(Environment.NewLine);
			}

			if (hasPage)
			{
				_builder.AppendFormat( new string(kHorizontalChar, Mathf.Max(0, lineLength - pageInfo.Length - 1)) + " {0}", pageInfo);
			}
			else
			{
				_builder.Remove(_builder.Length - Environment.NewLine.Length, Environment.NewLine.Length);
			}

			// set value in text components
			if (_label_TextMeshPro != null)
				_label_TextMeshPro.text = _builder.ToString();
			else
				_label_Text.text = _builder.ToString();
		}

		void IDMRender_Update.Update(bool isVisible)
		{
			_menuObject.SetActive(isVisible);
		}

		#endregion

		#region Private Methods

		private MenuRender(GameObject menuObject) => _menuObject = menuObject;

		private void CalculateLengths(IDMBranch itemsContainer, IReadOnlyList<DMItem> items, int spaceLength, out int fullLength, out int maxNameLength, out int maxValueLength, out int maxDescriptionLength)
		{
			maxNameLength = 0;
			maxValueLength = 0;
			maxDescriptionLength = 0;
			fullLength = itemsContainer.Name.Length;

			for (var i = 0; i < items.Count; i++)
			{
				var item = items[i];
				var nameLength = item.Name.Length;
				var valueLength = item.Value.Length;
				var descriptionLength = item.Description.Length;

				if (item is IDMBranch)
					nameLength += 3;

				maxNameLength = Math.Max(maxNameLength, nameLength);
				maxValueLength = Math.Max(maxValueLength, valueLength);
				maxDescriptionLength = Math.Max(maxDescriptionLength, descriptionLength);
			}

			
			fullLength = Math.Max(fullLength, maxNameLength + spaceLength + maxValueLength + spaceLength + maxDescriptionLength);
		}

		#endregion
	}
}
