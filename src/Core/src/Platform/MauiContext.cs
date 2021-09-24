using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;

namespace Microsoft.Maui
{
	public partial class MauiApplicationContext : IMauiApplicationContext
	{
		public MauiApplicationContext(IServiceProvider services)
		{
			Services = services ?? throw new ArgumentNullException(nameof(services));
			Handlers = Services.GetRequiredService<IMauiHandlersServiceProvider>();
		}

		public IServiceProvider Services { get; }

		public IMauiHandlersServiceProvider Handlers { get; }
	}

	public partial class MauiContext : IMauiWindowContext, IScopedMauiContext
	{
		IAnimationManager? _animationManager;

		public MauiContext(IServiceProvider services, IMauiContext? applicationContext = null)
		{
			Services = services ?? throw new ArgumentNullException(nameof(services));
			ApplicationContext = applicationContext;
			Handlers = Services.GetRequiredService<IMauiHandlersServiceProvider>();
		}

		public IServiceProvider Services { get; }

		internal IMauiContext? ApplicationContext { get; }

		public IMauiHandlersServiceProvider Handlers { get; }

		IAnimationManager IScopedMauiContext.AnimationManager =>
			_animationManager ??= Services.GetRequiredService<IAnimationManager>();
	}
}