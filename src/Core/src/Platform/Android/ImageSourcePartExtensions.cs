﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Views;

namespace Microsoft.Maui.Platform
{
	internal static class ImageSourcePartExtensions
	{
		public static async Task<IImageSourceServiceResult?> UpdateSourceAsync(
			this IImageSourcePart image,
			View destinationContext,
			IImageSourceServiceProvider services,
			Action<Drawable?> setImage,
			CancellationToken cancellationToken = default)
		{
			image.UpdateIsLoading(false);

			var context = destinationContext.Context;
			if (context == null)
				return null;

			var destinationImageView = destinationContext as Android.Widget.ImageView;
				
			if (destinationImageView is null && setImage is null)
				return null;

			var imageSource = image.Source;
			if (imageSource == null)
				return null;

			var events = image as IImageSourcePartEvents;

			events?.LoadingStarted();
			image.UpdateIsLoading(true);

			try
			{
				var service = services.GetRequiredImageSourceService(imageSource);

				var applied = false;
				IImageSourceServiceResult? result;

				if (destinationImageView is not null)
				{
					result = await service.LoadDrawableAsync(imageSource, destinationImageView, cancellationToken);
				}
				else
				{
					result = await service.GetDrawableAsync(context, imageSource, cancellationToken);
					if (setImage is not null && result is IImageSourceServiceResult<Drawable> drawableResult)
						setImage.Invoke(drawableResult.Value);
				}

				applied = result is not null && !cancellationToken.IsCancellationRequested && destinationContext.IsAlive() && imageSource == image.Source;

				events?.LoadingCompleted(applied);

				return result;
			}
			catch (OperationCanceledException)
			{
				// no-op
				events?.LoadingCompleted(false);
			}
			catch (Exception ex)
			{
				events?.LoadingFailed(ex);
			}
			finally
			{
				// only mark as finished if we are still working on the same image
				if (imageSource == image.Source)
				{
					image.UpdateIsLoading(false);
				}
			}

			return null;
		}
	}
}