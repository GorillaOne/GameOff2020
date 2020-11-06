using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DG.Tweening
{
	public static class DotweenExtensions
	{
		public static async Task<T> IsComplete<T>(this T t) where T : Tween
		{
			var completionSource = new TaskCompletionSource<T>();
			t.onComplete += ()=>
			{
				bool result = completionSource.TrySetResult(t);
			};
			t.onKill += () =>
			{
				bool result = completionSource.TrySetResult(t);
			};
			return await completionSource.Task;  
		}

		public static Sequence Move(this RectTransform obj, RectTransform to, RectTransform from, float animationTime, Ease ease = Ease.Linear)
		{
			var seq = DOTween.Sequence();

			seq.AppendCallback(() =>
			{
				obj.pivot = to.pivot;
				obj.anchorMin = to.anchorMin;
				obj.anchorMax = to.anchorMax; 
			});

			var moveTween = obj.DOMove(to.position, animationTime).SetEase(ease);
			if (from != null) moveTween.From(from.position, true);
			seq.Insert(0, moveTween);

			return seq;
		}
	}
}
