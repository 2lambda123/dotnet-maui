﻿#if ANDROID
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Xunit;

#if ANDROID || IOS
using ShellHandler = Microsoft.Maui.Controls.Handlers.Compatibility.ShellRenderer;
#endif

namespace Microsoft.Maui.DeviceTests
{
	public partial class ShellTests : HandlerTestBase
	{
		[Fact]
		public async Task FlyoutHeaderAdaptsToMinimumHeight()
		{
			await RunShellTest(shell =>
			{
				var layout = new VerticalStackLayout()
				{
					new Label() { Text = "Flyout Header" }
				};

				layout.MinimumHeightRequest = 30;

				shell.FlyoutHeader = layout;
				shell.FlyoutHeaderBehavior = FlyoutHeaderBehavior.CollapseOnScroll;
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);
				var flyoutFrame = GetFrameRelativeToFlyout(handler, shell.FlyoutHeader as IView);

				AssertionExtensions.AssertWithMessage(() =>
					Assert.True(Math.Abs(30 - flyoutFrame.Height) < 0.2),
					$"Expected: {30} Actual: {flyoutFrame.Height}"
				);
			});
		}

		[Theory]
		[ClassData(typeof(ShellFlyoutHeaderBehaviorTestCases))]
		public async Task FlyoutHeaderMinimumHeight(FlyoutHeaderBehavior behavior)
		{
			await RunShellTest(shell =>
			{
				var layout = new VerticalStackLayout()
				{
					new Label() { Text = "Flyout Header" }
				};

				shell.FlyoutHeader = layout;
				shell.FlyoutHeaderBehavior = behavior;
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);
				var flyoutFrame = GetFrameRelativeToFlyout(handler, shell.FlyoutHeader as IView);


				if (behavior == FlyoutHeaderBehavior.CollapseOnScroll)
				{
					// 56 was pulled from the ActionBar height on Android
					// and then just used across all three platforms for
					// the min height when using collapse on scroll
					AssertionExtensions.AssertWithMessage(() =>
						Assert.True(Math.Abs(56 - flyoutFrame.Height) < 0.2),
						$"Expected: 56 Actual: {flyoutFrame.Height}"
					);
				}
				else
				{
					AssertionExtensions.AssertWithMessage(() =>
						Assert.True(flyoutFrame.Height < 56),
						$"Expected < 56 Actual: {flyoutFrame.Height}"
					);
				}
			});
		}

		// This is mainly relevant for android because android will auto offset the content
		// baed on the height of the flyout header.
		[Fact]
		public async Task FlyoutContentSetsCorrectBottomPaddingWhenMinHeightIsSetForFlyoutHeader()
		{
			await RunShellTest(shell =>
			{
				var layout = new VerticalStackLayout()
				{
					new Label() { Text = "Flyout Header" }
				};

				layout.MinimumHeightRequest = 30;
				shell.FlyoutHeader = layout;

				shell.FlyoutFooter = new Label() { Text = "Flyout Footer" };
				shell.FlyoutContent = new VerticalStackLayout() { new Label() { Text = "Flyout Content" } };
				shell.FlyoutHeaderBehavior = FlyoutHeaderBehavior.CollapseOnScroll;
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);

				var headerFrame = GetFrameRelativeToFlyout(handler, (IView)shell.FlyoutHeader);
				var contentFrame = GetFrameRelativeToFlyout(handler, (IView)shell.FlyoutContent);
				var footerFrame = GetFrameRelativeToFlyout(handler, (IView)shell.FlyoutFooter);

				// validate footer position
				Assert.Equal(headerFrame.Height + contentFrame.Height, footerFrame.Y);
			});
		}

		[Theory]
		[ClassData(typeof(ShellFlyoutHeaderBehaviorTestCases))]
		public async Task FlyoutHeaderContentAndFooterAllMeasureCorrectly(FlyoutHeaderBehavior behavior)
		{
			await RunShellTest(shell =>
			{
				shell.FlyoutHeader = new Label() { Text = "Flyout Header" };
				shell.FlyoutFooter = new Label() { Text = "Flyout Footer" };
				shell.FlyoutContent = new VerticalStackLayout() { new Label() { Text = "Flyout Content" } };
				shell.FlyoutHeaderBehavior = behavior;
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);

				var flyoutFrame = GetFlyoutFrame(handler);
				var headerFrame = GetFrameRelativeToFlyout(handler, (IView)shell.FlyoutHeader);
				var contentFrame = GetFrameRelativeToFlyout(handler, (IView)shell.FlyoutContent);
				var footerFrame = GetFrameRelativeToFlyout(handler, (IView)shell.FlyoutFooter);

				// validate header position
				Assert.Equal(0, headerFrame.X);
				Assert.Equal(0, headerFrame.Y);
				Assert.Equal(headerFrame.Width, flyoutFrame.Width);

				// validate content position
				Assert.Equal(0, contentFrame.X);
				Assert.Equal(headerFrame.Height, contentFrame.Y);
				Assert.Equal(contentFrame.Width, flyoutFrame.Width);

				// validate footer position
				Assert.Equal(0, contentFrame.X);
				Assert.Equal(headerFrame.Height + contentFrame.Height, footerFrame.Y);
				Assert.Equal(footerFrame.Width, flyoutFrame.Width);

				//All three views should measure to the height of the flyout
				Assert.Equal(headerFrame.Height + contentFrame.Height + footerFrame.Height, flyoutFrame.Height);
			});
		}

		[Fact]
		public async Task FlyoutHeaderCollapsesOnScroll()
		{
			await RunShellTest(shell =>
			{
				Enumerable.Range(0, 100)
					.ForEach(i =>
					{
						shell.FlyoutHeaderBehavior = FlyoutHeaderBehavior.CollapseOnScroll;
						shell.Items.Add(new FlyoutItem() { Title = $"FlyoutItem {i}", Items = { new ContentPage() } });
					});

				var layout = new VerticalStackLayout()
				{
					new Label()
					{
						Text = "Header Content"
					}
				};

				layout.HeightRequest = 250;

				shell.FlyoutHeader = new ScrollView()
				{
					MinimumHeightRequest = 100,
					Content = layout
				};
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);
				await Task.Delay(10);

				var initialBox = (shell.FlyoutHeader as IView).GetBoundingBox();

				AssertionExtensions.AssertWithMessage(() =>
					Assert.True(Math.Abs(250 - initialBox.Height) < 0.2),
					$"Expected: 250 Actual: {initialBox.Height}"
				);

				await ScrollFlyoutToBottom(handler);

				var scrolledBox = (shell.FlyoutHeader as IView).GetBoundingBox();

				AssertionExtensions.AssertWithMessage(() =>
					Assert.True(Math.Abs(100 - scrolledBox.Height) < 0.2),
					$"Expected: 100 Actual: {scrolledBox.Height}"
				);
			});
		}

		[Theory]
		[ClassData(typeof(ShellFlyoutTemplatePartsTestCases))]
		public async Task FlyoutCustomContentMargin(Func<Shell, object, string> shellPart)
		{
			var baselineContent = new VerticalStackLayout() { new Label() { Text = "Flyout Layout Part" } };
			Rect frameWithoutMargin = Rect.Zero;

			// determine the location of the templated content on the screen without a margin
			await RunShellTest(shell =>
			{
				_ = shellPart(shell, baselineContent);
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);
				frameWithoutMargin = GetFrameRelativeToFlyout(handler, baselineContent);
			});

			var content = new VerticalStackLayout() { new Label() { Text = "Flyout Layout Part" } };
			string partTesting = string.Empty;
			await RunShellTest(shell =>
			{
				content.Margin = new Thickness(20, 30, 0, 30);
				partTesting = shellPart(shell, content);
			},
			async (shell, handler) =>
			{
				await OpenFlyout(handler);

				var frameWithMargin = GetFrameRelativeToFlyout(handler, content);
				var leftDiff = Math.Abs(Math.Abs(frameWithMargin.Left - frameWithoutMargin.Left) - 20);
				var verticalDiff = Math.Abs(Math.Abs(frameWithMargin.Top - frameWithoutMargin.Top) - 30);

				AssertionExtensions.AssertWithMessage(() =>
					Assert.True(leftDiff < 0.2),
					$"{partTesting} Left Margin Incorrect. Frame w/ margin: {frameWithMargin}. Frame w/o marin : {frameWithoutMargin}"
				);

				AssertionExtensions.AssertWithMessage(() =>
					Assert.True(verticalDiff < 0.2),
					$"{partTesting} Top Margin Incorrect. Frame w/ margin: {frameWithMargin}. Frame w/o marin : {frameWithoutMargin}"
				);
			});
		}

		async Task RunShellTest(Action<Shell> action, Func<Shell, ShellHandler, Task> testAction)
		{
			SetupBuilder();
			var shell = await CreateShellAsync((shell) =>
			{
				action(shell);
				if (shell.Items.Count == 0)
					shell.CurrentItem = new FlyoutItem() { Items = { new ContentPage() } };
			});

			await CreateHandlerAndAddToWindow<ShellHandler>(shell, async (handler) =>
			{
				await OnNavigatedToAsync(shell.CurrentPage);
				await testAction(shell, handler);
			});
		}
	}
}
#endif