﻿namespace Microsoft.Maui.Handlers
{
	public partial class SearchBarHandler : ViewHandler<ISearchBar, MauiSearchBar>
	{
		protected override MauiSearchBar CreatePlatformView() => new();


		protected override void ConnectHandler(MauiSearchBar nativeView)
		{
			nativeView.Entry.TextChanged += OnTextChanged;
			nativeView.SearchButtonPressed += OnSearchButtonPressed;
			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(MauiSearchBar nativeView)
		{
			nativeView.Entry.TextChanged -= OnTextChanged;
			nativeView.SearchButtonPressed -= OnSearchButtonPressed;
			base.DisconnectHandler(nativeView);
		}

		// TODO: NET7 make this public
		internal static void MapBackground(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.UpdateBackground(searchBar);
		}

		public static void MapText(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateText(searchBar);
		}
		
		public static void MapPlaceholder(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdatePlaceholder(searchBar);
		}

		public static void MapPlaceholderColor(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdatePlaceholderColor(searchBar);
		}

		public static void MapFont(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateFont(searchBar, handler.GetRequiredService<IFontManager>());
		}

		public static void MapHorizontalTextAlignment(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateHorizontalTextAlignment(searchBar);
		}

		public static void MapVerticalTextAlignment(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler?.PlatformView?.Entry.UpdateVerticalTextAlignment(searchBar);
		}

		public static void MapTextColor(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateTextColor(searchBar);
		}

		public static void MapMaxLength(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateMaxLength(searchBar);
		}

		public static void MapIsReadOnly(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateIsReadOnly(searchBar);
		}

		public static void MapIsTextPredictionEnabled(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateIsTextPredictionEnabled(searchBar);
		}

		public static void MapKeyboard(ISearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.Entry.UpdateKeyboard(searchBar);
		}

		public static void MapCancelButtonColor(ISearchBarHandler handler, ISearchBar searchBar)
		{
		}

		public static void MapCancelButtonColor(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.PlatformView?.UpdateCancelButtonColor(searchBar);
		}

		[MissingMapper]
		public static void MapCharacterSpacing(SearchBarHandler handler, ISearchBar searchBar) { }

		void OnTextChanged(object? sender, Tizen.NUI.BaseComponents.TextField.TextChangedEventArgs e)
		{
			if (VirtualView == null || PlatformView == null)
				return;

			VirtualView.Text = PlatformView.Entry.Text;
		}


		void OnSearchButtonPressed(object? sender, System.EventArgs e)
		{
			VirtualView?.SearchButtonPressed();
		}

	}
}