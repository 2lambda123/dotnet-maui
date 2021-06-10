using System;
using System.Drawing;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using UIKit;

namespace Microsoft.Maui.Handlers
{
	public partial class SearchBarHandler : ViewHandler<ISearchBar, UISearchBar>
	{
		UIColor? _cancelButtonTextColorDefaultDisabled;
		UIColor? _cancelButtonTextColorDefaultHighlighted;
		UIColor? _cancelButtonTextColorDefaultNormal;

		UITextField? _editor;
		public UITextField? QueryEditor => _editor;

		protected override UISearchBar CreateNativeView()
		{
			var searchBar = new UISearchBar(RectangleF.Empty) { ShowsCancelButton = true, BarStyle = UIBarStyle.Default };

			_editor = searchBar.FindDescendantView<UITextField>();

			return searchBar;
		}

		protected override void ConnectHandler(UISearchBar nativeView)
		{
			nativeView.CancelButtonClicked += OnCancelClicked;
			nativeView.SearchButtonClicked += OnSearchButtonClicked;
			nativeView.TextChanged += OnTextChanged;
			nativeView.ShouldChangeTextInRange += ShouldChangeText;

			base.ConnectHandler(nativeView);
		}

		protected override void DisconnectHandler(UISearchBar nativeView)
		{
			nativeView.CancelButtonClicked -= OnCancelClicked;
			nativeView.SearchButtonClicked -= OnSearchButtonClicked;
			nativeView.TextChanged -= OnTextChanged;
			nativeView.ShouldChangeTextInRange -= ShouldChangeText;

			base.DisconnectHandler(nativeView);
    }
    
		protected override void SetupDefaults(UISearchBar nativeView)
		{
			var cancelButton = nativeView.FindDescendantView<UIButton>();

			if (cancelButton != null)
			{
				_cancelButtonTextColorDefaultNormal = cancelButton.TitleColor(UIControlState.Normal);
				_cancelButtonTextColorDefaultHighlighted = cancelButton.TitleColor(UIControlState.Highlighted);
				_cancelButtonTextColorDefaultDisabled = cancelButton.TitleColor(UIControlState.Disabled);
			}

			base.SetupDefaults(nativeView);
		}

		public static void MapText(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateText(searchBar);

			// Any text update requires that we update any attributed string formatting
			MapFormatting(handler, searchBar);
		}

		public static void MapPlaceholder(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdatePlaceholder(searchBar);
		}

		public static void MapFont(SearchBarHandler handler, ISearchBar searchBar)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.QueryEditor?.UpdateFont(searchBar, fontManager);
		}

		public static void MapHorizontalTextAlignment(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.QueryEditor?.UpdateHorizontalTextAlignment(searchBar);
		}

		public static void MapCharacterSpacing(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.QueryEditor?.UpdateCharacterSpacing(searchBar);
		}

		public static void MapFormatting(SearchBarHandler handler, ISearchBar searchBar)
		{
			// Update all of the attributed text formatting properties
			handler.QueryEditor?.UpdateCharacterSpacing(searchBar);

			// Setting any of those may have removed text alignment settings,
			// so we need to make sure those are applied, too
			handler.QueryEditor?.UpdateHorizontalTextAlignment(searchBar);

			// We also update MaxLength which depends on the text
			handler.NativeView?.UpdateMaxLength(searchBar);
		}

		[MissingMapper]
		public static void MapTextColor(IViewHandler handler, ISearchBar searchBar) { }

		[MissingMapper]
		public static void MapIsTextPredictionEnabled(IViewHandler handler, ISearchBar searchBar) { }

		public static void MapMaxLength(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateMaxLength(searchBar);
		}

		[MissingMapper]
		public static void MapIsReadOnly(IViewHandler handler, ISearchBar searchBar) { }

    public static void MapCancelButtonColor(SearchBarHandler handler, ISearchBar searchBar)
		{
			handler.NativeView?.UpdateCancelButton(searchBar, handler._cancelButtonTextColorDefaultNormal, handler._cancelButtonTextColorDefaultHighlighted, handler._cancelButtonTextColorDefaultDisabled);
    }
    
		void OnCancelClicked(object? sender, EventArgs args)
		{
			if (VirtualView != null)
				VirtualView.Text = string.Empty;

			NativeView?.ResignFirstResponder();
		}

		void OnSearchButtonClicked(object? sender, EventArgs e)
		{
			VirtualView?.SearchButtonPressed();
			NativeView?.ResignFirstResponder();
		}

		void OnTextChanged(object? sender, UISearchBarTextChangedEventArgs a)
		{
			if (VirtualView != null)
				VirtualView.Text = a.SearchText;
		}

		bool ShouldChangeText(UISearchBar searchBar, NSRange range, string text)
		{
			var newLength = searchBar?.Text?.Length + text.Length - range.Length;
			return newLength <= VirtualView?.MaxLength;
		}
	}
}