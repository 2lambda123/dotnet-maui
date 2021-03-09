using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using AndroidX.Core.Content;
using AColor = Android.Graphics.Color;
using ARect = Android.Graphics.Rect;
using AView = Android.Views.View;

namespace Microsoft.Maui.Controls.Compatibility.Platform.Android
{
	public static class ViewExtensions
	{
		static readonly int s_apiLevel;

		static ViewExtensions()
		{
			s_apiLevel = (int)Forms.SdkInt;
		}

		public static void RemoveFromParent(this AView view)
		{
			if (view == null)
				return;
			if (view.Parent == null)
				return;
			((ViewGroup)view.Parent).RemoveView(view);
		}

		public static void SetBackground(this AView view, Drawable drawable)
		{
			if (s_apiLevel < 16)
			{
#pragma warning disable 618 // Using older method for compatibility with API 15
				view.SetBackgroundDrawable(drawable);
#pragma warning restore 618
			}
			else
			{
				view.Background = drawable;
			}

		}

		public static void SetWindowBackground(this AView view)
		{
			Context context = view.Context;
			using (var background = new TypedValue())
			{
				if (context.Theme.ResolveAttribute(global::Android.Resource.Attribute.WindowBackground, background, true))
				{
					string type = context.Resources.GetResourceTypeName(background.ResourceId).ToLower();
					switch (type)
					{
						case "color":
							var color = new AColor(ContextCompat.GetColor(context, background.ResourceId));
							view.SetBackgroundColor(color);
							break;
						case "drawable":
							using (Drawable drawable = ContextCompat.GetDrawable(context, background.ResourceId))
								view.SetBackground(drawable);
							break;
					}
				}
			}
		}

		public static void EnsureId(this AView view)
		{
			if (view.IsDisposed())
			{
				return;
			}

			if (view.Id == AView.NoId)
			{
				view.Id = AppCompat.Platform.GenerateViewId();
			}
		}

		public static bool GetClipToOutline(this AView view)
		{
			if (view.IsDisposed() || !Forms.IsLollipopOrNewer)
				return false;

			return view.ClipToOutline;
		}

		public static void SetClipToOutline(this AView view, bool value)
		{
			if (view.IsDisposed() || !Forms.IsLollipopOrNewer)
				return;

			view.ClipToOutline = value;
		}

		public static bool SetElevation(this AView view, float value)
		{
			if (view.IsDisposed() || !Forms.IsLollipopOrNewer)
				return false;

			view.Elevation = value;
			return true;
		}

		internal static void MaybeRequestLayout(this AView view)
		{
			var isInLayout = false;
			if ((int)Build.VERSION.SdkInt >= 18)
				isInLayout = view.IsInLayout;

			if (!isInLayout && !view.IsLayoutRequested)
				view.RequestLayout();
		}

		internal static T GetParentOfType<T>(this IViewParent view)
			where T : class
		{
			if (view is T t)
				return t;

			while (view != null)
			{
				T parent = view.Parent as T;
				if (parent != null)
					return parent;

				view = view.Parent;
			}

			return default(T);
		}

		internal static T GetParentOfType<T>(this AView view)
			where T : class
		{
			T t = view as T;
			if (view != null)
				return t;

			return view.Parent.GetParentOfType<T>();
		}

		internal static T FindParentOfType<T>(this VisualElement element)
		{
			var navPage = element.GetParentsPath()
				.OfType<T>()
				.FirstOrDefault();
			return navPage;
		}

		internal static IEnumerable<Element> GetParentsPath(this VisualElement self)
		{
			Element current = self;

			while (!Application.IsApplicationOrNull(current.RealParent))
			{
				current = current.RealParent;
				yield return current;
			}
		}
	}
}
