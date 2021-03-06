//
// Encog(tm) Core v3.3 - .Net Version (unit test)
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2014 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using Encog.Util.Normalize.Input;
using Encog.Util.Normalize.Output.Mapped;
using Encog.Util.Normalize.Target;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Encog.Util.Normalize
{
    [TestClass]
    public class TestMapped
    {
        public static readonly double[][] ARRAY_2D = {
                                                         new[] {1.0, 2.0, 3.0, 4.0, 5.0},
                                                         new[] {6.0, 7.0, 8.0, 9.0}
                                                     };

        private DataNormalization Create(double[][] arrayOutput)
        {
            IInputField a, b;


            var target = new NormalizationStorageArray2D(arrayOutput);
            OutputFieldEncode a1;
            OutputFieldEncode b1;

            var norm = new DataNormalization();
            norm.Report = new NullStatusReportable();
            norm.Storage = target;
            norm.AddInputField(a = new InputFieldArray2D(false, ARRAY_2D, 0));
            norm.AddInputField(b = new InputFieldArray2D(false, ARRAY_2D, 1));
            norm.AddOutputField(a1 = new OutputFieldEncode(a));
            norm.AddOutputField(b1 = new OutputFieldEncode(b));
            a1.AddRange(1.0, 2.0, 0.1);
            b1.AddRange(0, 100, 0.2);

            return norm;
        }

        public void Check(double[][] arrayOutput)
        {
            Assert.AreEqual(arrayOutput[0][0], 0.1, 0.1);
            Assert.AreEqual(arrayOutput[1][0], 0.0, 0.1);
            Assert.AreEqual(arrayOutput[0][1], 0.1, 0.1);
            Assert.AreEqual(arrayOutput[1][1], 0.2, 0.1);
        }

        [TestMethod]
        public void TestOutputFieldEncode()
        {
            double[][] arrayOutput = EngineArray.AllocateDouble2D(2, 2);
            DataNormalization norm = Create(arrayOutput);
            norm.Process();
            Check(arrayOutput);
        }

        [TestMethod]
        public void TestOutputFieldEncodeSerialize()
        {
            double[][] arrayOutput = EngineArray.AllocateDouble2D(2, 2);
            DataNormalization norm = Create(arrayOutput);
            norm = (DataNormalization) SerializeRoundTrip.RoundTrip(norm);
            arrayOutput = ((NormalizationStorageArray2D) norm.Storage).GetArray();
            norm.Process();
            Check(arrayOutput);
        }
    }
}
