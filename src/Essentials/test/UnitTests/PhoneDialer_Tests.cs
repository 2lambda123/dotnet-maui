using Microsoft.Maui.Devices;
using Xunit;

namespace Tests
{
	public class PhoneDialer_Tests
	{
		[Fact]
		public void Dialer_Open_Fail_On_NetStandard() =>
			Assert.Throws<NotImplementedInReferenceAssemblyException>(() => PhoneDialer.Open("1234567890"));
	}
}
