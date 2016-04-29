using System.Threading.Tasks;
using Exrin.Abstraction;
using TeslaDefinition.Interfaces.Model;
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Tesla.ViewModelOperation;
using Xunit;
using Tesla.Model;
using Exrin.Framework;
using Xunit.Extensions;
using Tesla.Tests;
using Tesla.Control;
using System.Collections.Generic;
using System.Threading;

namespace Tesla.ViewModelOperation.Tests
{
    public partial class PinLoginOperationTest
    {

        public PinLoginOperationTest()
        {

        }

        public IAuthModel PinModel
        {
            get
            {
                return new AuthModel(CommonService.DisplayService, CommonService.ApplicationInsights, CommonService.ErrorHandlingService);
            }
        }

        public Func<IList<IResult>, object, CancellationToken, Task> GetOperation()
        {
            return new PinLoginOperation(PinModel, Keypad.BackCharacter).Function;
        }

        public static TheoryData<string[]> SimplePin { get { return new TheoryData<string[]>() { new string[] { "1", "2", "3", "4" } }; } }
        public static TheoryData<string[]> SimplePinWithBackspace { get { return new TheoryData<string[]>() { new string[] { "1", "2", Keypad.BackCharacter, "4", "5" } }; } }
        public static TheoryData<string[]> StartingBackspacePin { get { return new TheoryData<string[]>() { new string[] { Keypad.BackCharacter, "2", "1", "4", "5" } }; } }

        [Theory]
        [MemberData(nameof(SimplePin))]
        [MemberData(nameof(SimplePinWithBackspace))]
        [MemberData(nameof(StartingBackspacePin))]
        public async void OperationTest(string[] characters)
        {
            var function = GetOperation();

            int count = 0;
            foreach (var character in characters)
            {
                IList<IResult> results = new List<IResult>();

                await function(results, character, new CancellationToken());

                Assert.NotNull(results);

                Assert.Equal(1, results.Count);

                if (results.Count == 1)
                {
                    var result = results[0];

                    if (count == BusinessRules.PinLength - 1)
                        Assert.Equal(ResultType.Navigation, result.ResultAction);
                    else
                        Assert.Equal(ResultType.None, result.ResultAction);
                }

                if (character == Keypad.BackCharacter && count != 0)
                    count--;
                else if (character != Keypad.BackCharacter)
                    count++;
            }
        }


    }
}
