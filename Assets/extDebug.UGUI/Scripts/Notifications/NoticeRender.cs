﻿/* Copyright (c) 2022 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extDebug.Notifications.UGUI
{
	public enum RenderType
	{
		Default,
		VR
	}
	
	public class NoticeRender : IDNRender
	{
		#region Public Vars
		
		public Vector2 ItemsOffset = new Vector2(10, 10);

		public float ItemSpace = 5;

		#endregion

		#region Private Vars

		private readonly RectTransform _noticeAnchor;
		
		private readonly NoticeItem _noticePrefab;

		private readonly RenderType _type;

		private Vector2 _previousSize;
		
		#endregion

		#region Public Methods

		public NoticeRender(RectTransform noticeAnchor, NoticeItem noticePrefab, RenderType type = RenderType.Default)
		{
			_noticeAnchor = noticeAnchor;
			_noticePrefab = noticePrefab;
			_type = type;
		}

		#endregion
		
		#region IDNRender Methods

		void IDNRender.SetupNotice(DNNotice notice, float currentHeight)
		{
			var noticeData = Object.Instantiate(_noticePrefab, _noticeAnchor);
			noticeData.Notice = notice;
			noticeData.Text = notice.Text;
			
			if (_type == RenderType.Default)
			{
				noticeData.Position = new Vector2(noticeData.Width, -ItemsOffset.y - currentHeight - (_previousSize.y + ItemSpace));
				noticeData.Alpha = 1f;
			}
			else if (_type == RenderType.VR)
			{
				noticeData.Position = new Vector2(-noticeData.Width * 2,  currentHeight + _previousSize.y + ItemSpace);
				noticeData.Alpha = 0f;
			}
			
			notice.Data = noticeData;
		}

		void IDNRender.RemoveNotice(DNNotice notice)
		{
			if (notice.Data is NoticeItem noticeData)
				Object.Destroy(noticeData.gameObject);
		}

		void IDNRender.Repaint(DNNotice notice, float timeLeft, ref float currentHeight)
		{
			if (notice.Data is not NoticeItem noticeData)
				return;

			var size = noticeData.Size;
			var position = noticeData.Position;

			_previousSize = size;
			
			var width = size.x;
			var height = size.y + ItemSpace;

			var targetX = 0f;
			var targetY = 0f;

			targetX = _type == RenderType.Default ? -ItemsOffset.x : 0;
			targetY = _type == RenderType.Default ? height - currentHeight - ItemsOffset.y : height + currentHeight;

			// Calculate targets
			if (timeLeft < 0.2f) targetX += width * 2;
			else if (timeLeft < 0.7f) targetX -= 50;

			if (_type == RenderType.VR)
			{
				if (timeLeft > 0.2f)
				{
					noticeData.Alpha += Time.unscaledDeltaTime * 4.5f;
				}
				else
				{
					noticeData.Alpha -= Time.unscaledDeltaTime * 4.5f;
				}
			}

			// Calculate new positions
			var speed = Time.unscaledDeltaTime * 15;
			position.x += noticeData.Velocity.x * speed;
			position.y += noticeData.Velocity.y * speed;

			// Calculate speed
			var distance = targetX - position.x;
			noticeData.Velocity.x += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeData.Velocity.x) < 0.1) noticeData.Velocity.x = 0;

			distance = targetY - position.y;
			noticeData.Velocity.y += distance * speed * 1;
			if (Mathf.Abs(distance) < 2 && Mathf.Abs(noticeData.Velocity.y) < 0.1) noticeData.Velocity.y = 0;

			var friction = 0.95f - Time.unscaledDeltaTime * 8;
			noticeData.Velocity.x *= friction;
			noticeData.Velocity.y *= friction;

			noticeData.Position = position;
			noticeData.Text = notice.Text;
			
			currentHeight -= height;
		}

		#endregion
	}
}