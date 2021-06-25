using System.Threading.Tasks;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	[TestFixture]
	public class AnimationTests : BaseTestFixture
	{
		[Test]
		//https://bugzilla.xamarin.com/show_bug.cgi?id=51424
		public async Task AnimationRepeats()
		{
			var box = new BoxView
			{
				Handler = new HandlerWithAnimationContext(),
			};
			Assume.That(box.Rotation, Is.EqualTo(0d));
			var sb = new Animation();
			var animcount = 0;
			var rot45 = new Animation(d =>
			{
				box.Rotation = d;
				if (d > 44)
					animcount++;
			}, box.Rotation, box.Rotation + 45);
			sb.Add(0, .5, rot45);
			Assume.That(box.Rotation, Is.EqualTo(0d));

			var i = 0;
			sb.Commit(box, "foo", length: 100, repeat: () => ++i < 2);

			await Task.Delay(1000);
			Assert.That(animcount, Is.EqualTo(2));
		}
	}
}
