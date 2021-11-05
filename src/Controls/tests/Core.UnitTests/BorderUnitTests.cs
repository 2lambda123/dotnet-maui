#nullable enable
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Core.UnitTests
{
	[TestFixture]
	public class BorderUnitTests : BaseTestFixture
	{
		[Test]
		public void HasNoVisualChildrenWhenNoContentIsSet()
		{
			var border = new Border();

			Assert.IsEmpty(((IVisualTreeElement)border).GetVisualChildren());

			border.Content = null;

			Assert.IsEmpty(((IVisualTreeElement)border).GetVisualChildren());
		}

		[Test]
		public void TestHasVisualChildrenWhenContentIsSet()
		{
			var border = new Border();

			var label = new Label();
			border.Content = label;

			var visualTreeChildren = ((IVisualTreeElement)border).GetVisualChildren();
			Assert.IsNotEmpty(visualTreeChildren);
			Assert.AreSame(visualTreeChildren[0], label);
		}
	}
}