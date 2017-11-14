using System.Text;
using NUnit.Framework;
using TypeMock;
using TypeMock.ArrangeActAssert;

namespace TypemockEncodingExample
{
    public static class SomeInterface_Old
    {
        public static void SomeDataTransferMethod(string data)
        {
            var encoder = new ASCIIEncoding();
            byte[] ascii = encoder.GetBytes(data);

            // pretend we send out this data to some web service or interface, etc.
            // SendDataToAnotherWebService(ascii);
        }
    }

    public static class SomeInterface1
    {
        /// <summary>
        /// This implementation is **not** testable via Typemock, or any other mocking provider
        /// </summary>
        /// <param name="data"></param>
        public static void SomeDataTransferMethod(string data)
        {
            byte[] utf8 = Encoding.UTF8.GetBytes(data);

            // pretend we send out this data to some web service or interface, etc.
        }
    }

    public static class SomeInterface2
    {
        public static void SomeDataTransferMethod(string data)
        {
            byte[] utf8 = new CompanyNameUTF8Encoder().GetBytes(data);

            // pretend we send out this data to some web service or interface, etc.
        }
    }

    /// <summary>
    /// Proxy for System.Text.Encoding.UTF8.
    /// </summary>
    /// <remarks>
    /// Useful so that we can write tests around our code that encodes/decodes UTF8 data.
    /// </remarks>
    public class CompanyNameUTF8Encoder
    {
        public byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
    }

    [TestFixture, Isolated]
    public class SomeInterfaces_Tests
    {
        [Test]
        public void faking_types_from_mscorlib_results_in_TypemockException()
        {
            // arrange
            Assert.Throws<TypeMockException>(() => Isolate.WhenCalled(() => Encoding.UTF8.GetBytes(default(string))).CallOriginal());

            // act and assert are commented because the test is actually over @ line 17.
            // but I'm leaving these below to show you what the test would look like if we could fake Encoding.UTF8.GetBytes(string).

            // act
            //SomeInterface1.SomeDataTransferMethod("my name is!");

            // assert
            //Isolate.Verify.WasCalledWithExactArguments(() => Encoding.UTF8.GetBytes("my name is!"));
        }

        [Test]
        public void test_failure()
        {
            // arrange
            Isolate.WhenCalled(() => Encoding.UTF8.GetBytes(default(string))).CallOriginal();            
        }

        /// <summary>
        /// Note that we are **not** testing the encoder here, but verifying our Code Under Test simply uses the encoder
        /// so we know that the data is getting encoded via UTF8.
        /// </summary>
        [Test]
        public void verify_CompanyNameUTF8Encoder_is_used_by_our_interface_calls()
        {
            // arrange
            CompanyNameUTF8Encoder encoder = Isolate.Swap.NextInstance<CompanyNameUTF8Encoder>().WithRecursiveFake();
            
            // act
            SomeInterface2.SomeDataTransferMethod("my name is!");

            // assert
            Isolate.Verify.WasCalledWithExactArguments(() => encoder.GetBytes("my name is!"));
        }
    }

    [TestFixture]
    public class CompanyNameUTF8Encoder_Tests
    {
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("my name is!")]
        public void encoder_calls_to_System_Text_Encoding_GetBytes(string data)
        {
            // arrange
            byte[] expected = Encoding.UTF8.GetBytes(data);

            // act
            var encoder = new CompanyNameUTF8Encoder();
            byte[] actual = encoder.GetBytes(data);

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}
