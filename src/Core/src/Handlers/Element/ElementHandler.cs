﻿using System;

namespace Microsoft.Maui.Handlers
{
	public abstract partial class ElementHandler : IElementHandler
	{
		public static PropertyMapper<IElement, ElementHandler> ElementMapper = new()
		{
		};

		static bool HasSetDefaults;

		[HotReload.OnHotReload]
		internal static void OnHotReload()
		{
			HasSetDefaults = false;
		}

		protected PropertyMapper _mapper;
		protected readonly PropertyMapper _defaultMapper;

		protected ElementHandler(PropertyMapper mapper)
		{
			_ = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_defaultMapper = mapper;
			_mapper = _defaultMapper;
		}

		public IMauiContext? MauiContext { get; private set; }

		public IServiceProvider? Services => MauiContext?.Services;

		public object? NativeView { get; private protected set; }

		public IElement? VirtualView { get; private protected set; }

		public void SetMauiContext(IMauiContext mauiContext) =>
			MauiContext = mauiContext;

		public virtual void SetVirtualView(IElement view)
		{
			_ = view ?? throw new ArgumentNullException(nameof(view));

			if (VirtualView == view)
				return;

			if (VirtualView?.Handler != null && VirtualView.Handler != this)
				VirtualView.Handler = null;

			bool setupNativeView = VirtualView == null;

			VirtualView = view;
			NativeView ??= CreateNativeElement();

			if (VirtualView.Handler != this)
				VirtualView.Handler = this;

			if (setupNativeView)
			{
				ConnectHandler(NativeView);
			}

			if (!HasSetDefaults)
			{
				if (NativeView != null)
				{
					SetupDefaults(NativeView);
				}

				HasSetDefaults = true;
			}

			_mapper = _defaultMapper;

			if (VirtualView is IPropertyMapperView imv)
			{
				var map = imv.GetPropertyMapperOverrides();
				if (map is PropertyMapperChained chainedMapper)
				{
					chainedMapper.Chained = _defaultMapper;
					_mapper = chainedMapper;
				}
			}

			_mapper.UpdateProperties(this, VirtualView);
		}

		public virtual void UpdateValue(string property)
		{
			if (VirtualView == null)
				return;

			_mapper?.UpdateProperty(this, VirtualView, property);
		}

		private protected abstract object OnCreateNativeElement();

		object CreateNativeElement() =>
			OnCreateNativeElement();

		private protected abstract void OnSetupDefaults(object nativeView);

		void SetupDefaults(object nativeView) =>
			OnSetupDefaults(nativeView);

		private protected abstract void OnConnectHandler(object nativeView);

		void ConnectHandler(object nativeView) =>
			OnConnectHandler(nativeView);

		private protected abstract void OnDisconnectHandler(object nativeView);

		void DisconnectHandle(object nativeView)
		{
			OnDisconnectHandler(nativeView);

			if (VirtualView != null)
				VirtualView.Handler = null;

			VirtualView = null;
		}

		void IElementHandler.DisconnectHandler()
		{
			if (NativeView is object nativeView)
				DisconnectHandle(nativeView);
		}
	}
}