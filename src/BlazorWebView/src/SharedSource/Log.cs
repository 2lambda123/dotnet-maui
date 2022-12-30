using System;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Components.WebView;

internal static partial class Log
{
	[LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Navigating to {uri}.")]
	public static partial void NavigatingToUri(this ILogger logger, Uri uri);

	[LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Failed to create WebView2 environment. This could mean that WebView2 is not installed.")]
	public static partial void FailedToCreateWebView2Environment(this ILogger logger);

	[LoggerMessage(EventId = 2, Level = LogLevel.Debug, Message = "Starting WebView2...")]
	public static partial void StartingWebView2(this ILogger logger);

	[LoggerMessage(EventId = 3, Level = LogLevel.Debug, Message = "WebView2 is started.")]
	public static partial void StartedWebView2(this ILogger logger);

	[LoggerMessage(EventId = 4, Level = LogLevel.Debug, Message = "Handling web request to URI '{requestUri}'.")]
	public static partial void HandlingWebRequest(this ILogger logger, string requestUri);

	[LoggerMessage(EventId = 5, Level = LogLevel.Debug, Message = "Response content being sent for web request to URI '{requestUri}' with HTTP status code {statusCode}.")]
	public static partial void ResponseContentBeingSent(this ILogger logger, string requestUri, int statusCode);

	[LoggerMessage(EventId = 6, Level = LogLevel.Debug, Message = "Response content was not found for web request to URI '{requestUri}'.")]
	public static partial void ReponseContentNotFound(this ILogger logger, string requestUri);

	[LoggerMessage(EventId = 7, Level = LogLevel.Debug, Message = "Navigation event for URI '{uri}' with URL loading strategy '{urlLoadingStrategy}'.")]
	public static partial void NavigationEvent(this ILogger logger, Uri uri, UrlLoadingStrategy urlLoadingStrategy);

	[LoggerMessage(EventId = 8, Level = LogLevel.Debug, Message = "Launching external browser for URI '{uri}'.")]
	public static partial void LaunchExternalBrowser(this ILogger logger, Uri uri);

	[LoggerMessage(EventId = 9, Level = LogLevel.Debug, Message = "Calling Blazor.start() in the WebView2.")]
	public static partial void CallingBlazorStart(this ILogger logger);
}
