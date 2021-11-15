﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui
{
	public partial class WindowOverlay : IWindowOverlay, IDrawable
	{
		internal HashSet<IWindowOverlayElement> _windowElements = new HashSet<IWindowOverlayElement>();
		internal bool _disposedValue;
		bool isVisible = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowOverlay"/> class.
		/// </summary>
		/// <param name="window">The parent Window.</param>
		public WindowOverlay(IWindow window)
		{
			this.Window = window;
		}

		/// <inheritdoc/>
		public IWindow Window { get; internal set; }

		/// <inheritdoc/>
		public IReadOnlyCollection<IWindowOverlayElement> WindowElements => this._windowElements.ToList().AsReadOnly();

		/// <inheritdoc/>
		public bool IsNativeViewInitialized { get; internal set; }

		/// <inheritdoc/>
		public bool EnableDrawableTouchHandling { get; set; }

		/// <inheritdoc/>
		public bool IsVisible
		{
			get { return isVisible; }
			set
			{
				isVisible = value;
				if (this.IsNativeViewInitialized)
					this.Invalidate();
			}
		}

		/// <inheritdoc/>
		public float DPI { get; internal set; } = 1;

#pragma warning disable CS0067 // The event is never used
		/// <inheritdoc/>
		public event EventHandler<VisualDiagnosticsHitEvent>? OnTouch;
#pragma warning restore CS0067

		/// <inheritdoc/>
		public void Draw(ICanvas canvas, RectangleF dirtyRect)
		{
			if (!this.IsVisible)
				return;
			foreach (var drawable in this._windowElements)
				drawable.Draw(canvas, dirtyRect);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					this.DisposeNativeDependencies();
				}

				_disposedValue = true;
			}
		}

		/// <inheritdoc/>
		public virtual bool AddWindowElement(IWindowOverlayElement drawable)
		{
			var result = this._windowElements.Add(drawable);
			this.Invalidate();
			return result;
		}

		/// <inheritdoc/>
		public virtual bool RemoveWindowElement(IWindowOverlayElement drawable)
		{
			var result = this._windowElements.Remove(drawable);
			this.Invalidate();
			return result;
		}

		/// <inheritdoc/>
		public virtual void RemoveWindowElements()
		{
			this._windowElements.Clear();
			this.Invalidate();
		}

		/// <inheritdoc/>
		public virtual void HandleUIChange()
		{
		}

		/// <summary>
		/// Handles <see cref="OnTouch"/> event.
		/// </summary>
		/// <param name="point">Point where user has touched.</param>
		internal void OnTouchInternal(Point point)
		{
			var elements = new List<IVisualTreeElement>();
			var windowElements = new List<IWindowOverlayElement>();

			if (this.EnableDrawableTouchHandling)
			{
				windowElements.AddRange(this._windowElements.Where(n => n.IsPointInElement(point)));
			}

			if (this.DisableUITouchEventPassthrough)
			{
				var visualWindow = this.Window as IVisualTreeElement;
				if (visualWindow != null)
					elements.AddRange(visualWindow.GetVisualTreeElements(point));
			}

			this.OnTouch?.Invoke(this, new VisualDiagnosticsHitEvent(point, elements, windowElements));
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

#if NETSTANDARD || NET6

		/// <inheritdoc/>
		public bool DisableUITouchEventPassthrough { get; set; }
		/// <inheritdoc/>
		public void Invalidate()
		{
		}

		/// <inheritdoc/>
		public bool InitializeNativeLayer()
		{
			return false;
		}

		/// <summary>
		/// Disposes the native event hooks and handlers used to drive the overlay.
		/// </summary>
		private void DisposeNativeDependencies()
		{
		}
#endif
	}
}
