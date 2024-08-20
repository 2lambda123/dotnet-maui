﻿using BenchmarkDotNet.Attributes;

namespace Microsoft.Maui.Graphics.Benchmarks
{
	[MemoryDiagnoser]
	public class PathBenchmarker
	{
		const string ComplexPathData = "M13.952596,15.068143 C13.767538,15.066144 13.583578,15.095151 13.403586,15.157148 C12.252587,15.553147 11.725549,17.163162 12.224572,18.753189 C12.725547,20.342192 14.062582,21.309212 15.211566,20.914204 C16.362564,20.518204 16.889541,18.908188 16.390579,17.318163 C15.968584,15.977162 14.95058,15.077146 13.952596,15.068143 Z M7.7945876,6.1100698 C7.2026091,6.0760732 6.4365583,6.7850791 5.9736071,7.8550807 C5.4445558,9.0761004 5.5105953,10.302109 6.1215563,10.590106 C6.7316013,10.881108 7.65555,10.126112 8.1855779,8.9070922 C8.7145686,7.6860881 8.6485896,6.4610711 8.036592,6.1710754 C7.9606028,6.1350642 7.8795486,6.1150752 7.7945876,6.1100698 Z M15.404559,5.9590679 C15.383563,5.9580608 15.362566,5.9580608 15.34157,5.960075 C14.674579,6.0020671 14.194539,7.1220723 14.275593,8.4590903 C14.354573,9.7981063 14.962543,10.848119 15.631547,10.802114 C16.300554,10.759113 16.778579,9.6401005 16.700576,8.3020907 C16.622573,7.006074 16.049577,5.980064 15.404559,5.9590679 Z M12.317589,1.4699259E-05 C15.527545,0.0050196948 18.757579,1.2870288 21.236579,3.8010436 C24.038576,6.6430793 25.533567,12.005127 25.825559,15.861164 C26.09155,19.371191 27.844537,19.518194 30.765552,22.228211 C31.592515,22.995216 33.904521,25.825243 28.733512,26.053242 C26.619564,26.146244 25.60156,25.739243 21.732549,22.850226 C21.235542,22.545214 20.664558,22.733219 20.373542,22.885214 C20.017526,23.07122 19.741586,23.925232 19.851572,24.215227 C20.16456,25.583237 22.25855,25.135235 23.427553,26.313253 C24.41156,27.305252 22.795536,29.807287 18.926586,29.29027 C18.926586,29.29027 16.343582,28.587277 13.853597,25.258236 C11.910547,25.242245 9.6305823,25.258236 9.6305823,25.258236 C9.6305823,25.258236 9.6025672,26.705256 9.6425452,27.10626 C10.271573,27.256254 10.777553,27.021252 13.298544,27.736271 C14.150593,27.978262 16.663589,31.170292 8.7236018,30.424282 C7.0135832,30.263287 7.1875944,30.721283 5.2576051,26.025242 C4.2626119,23.604229 2.0076115,22.396212 0.6345674,17.082169 C-0.27241354,14.207143 -0.21040192,11.068107 0.84159805,8.2280856 C0.97556992,7.8450862 1.1235799,7.5130826 1.2786091,7.1980773 C1.8406196,6.0020671 2.5815849,4.8720523 3.5156043,3.863056 C5.9166007,1.2680314 9.107573,-0.0049901602 12.317589,1.4699259E-05 Z";

		[Benchmark]
		public PathF BuildPath() => PathBuilder.Build(ComplexPathData);
	}
}
