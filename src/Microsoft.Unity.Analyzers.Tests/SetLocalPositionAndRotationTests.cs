/*--------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *-------------------------------------------------------------------------------------------*/

using System.Threading.Tasks;
using Xunit;

namespace Microsoft.Unity.Analyzers.Tests;

public class SetLocalPositionAndRotationTests : BaseCodeFixVerifierTest<SetLocalPositionAndRotationAnalyzer, SetLocalPositionAndRotationCodeFix>
{
	// For extensive testing, see SetPositionAndRotationTests.cs
	[Fact]
	public async Task UpdateLocalPositionAndRotationMethod()
	{
		const string test = @"
using UnityEngine;

class Camera : MonoBehaviour
{
    void Update()
    {
        transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
        transform.localRotation = transform.localRotation;
    }
}
";

		var diagnostic = ExpectDiagnostic().WithLocation(8, 9);

		await VerifyCSharpDiagnosticAsync(test, diagnostic);

		const string fixedTest = @"
using UnityEngine;

class Camera : MonoBehaviour
{
    void Update()
    {
        transform.SetLocalPositionAndRotation(new Vector3(0.0f, 1.0f, 0.0f), transform.localRotation);
    }
}
";

		await VerifyCSharpFixAsync(test, fixedTest);
	}
}
