using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;

namespace Microsoft.Maui.Handlers.Benchmarks
{
	class HandlersContextStub : IMauiContext
	{
		readonly IServiceProvider _services;
		readonly IMauiHandlersServiceProvider _handlersServiceProvider;
		readonly IAnimationManager _animationManager;

		public HandlersContextStub(IServiceProvider services)
		{
			_services = services;
			_handlersServiceProvider = Services.GetRequiredService<IMauiHandlersServiceProvider>();
			_animationManager = Services.GetService<IAnimationManager>() ?? new AnimationManager();
		}

		public IServiceProvider Services => _services;

		public IMauiHandlersServiceProvider Handlers => _handlersServiceProvider;

		public IAnimationManager AnimationManager => _animationManager;
	}
}
