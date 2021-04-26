using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Hosting;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ImageSourceServiceTests : BaseImageSourceServiceTests
	{
		[Fact]
		public void CanResolveCorrectService()
		{
			var provider = CreateImageSourceServiceProvider(services =>
			{
				services.AddService<IFileImageSource, FileImageSourceService>();
			});

			var imageSource = new FileImageSourceStub();

			var service = provider.GetRequiredImageSourceService(imageSource);

			Assert.IsType<FileImageSourceService>(service);
		}

		[Fact]
		public void CanResolveCorrectServiceWhenMultiple()
		{
			var provider = CreateImageSourceServiceProvider(services =>
			{
				services.AddService<IFileImageSource, FileImageSourceService>();
				services.AddService<IUriImageSource, UriImageSourceService>();
			});

			var service = provider.GetRequiredImageSourceService(new FileImageSourceStub());
			Assert.IsType<FileImageSourceService>(service);

			service = provider.GetRequiredImageSourceService(new UriImageSourceStub());
			Assert.IsType<UriImageSourceService>(service);
		}

		[Fact]
		public void ThrowsWhenMissingService()
		{
			var provider = CreateImageSourceServiceProvider(services => { });

			var ex = Assert.Throws<InvalidOperationException>(() => provider.GetRequiredImageSourceService(new FileImageSourceStub()));

			Assert.Contains(nameof(IFileImageSource), ex.Message);
		}

		[Fact]
		public void ThrowsWhenNotASpecificImageSource()
		{
			var provider = CreateImageSourceServiceProvider(services => { });

			var ex = Assert.Throws<InvalidOperationException>(() => provider.GetRequiredImageSourceService(new InvalidImageSourceStub()));

			Assert.Contains(nameof(InvalidImageSourceStub), ex.Message);
			Assert.Contains(nameof(IImageSource), ex.Message);
		}

		private IImageSourceServiceProvider CreateImageSourceServiceProvider(Action<IImageSourceServiceCollection> configure)
		{
			var host = new AppHostBuilder()
				.UseMauiServiceProviderFactory(true)
				.ConfigureImageSourceServices(configure)
				.Build();

			var services = host.Services;

			var provider = services.GetRequiredService<IImageSourceServiceProvider>();

			return provider;
		}
	}
}