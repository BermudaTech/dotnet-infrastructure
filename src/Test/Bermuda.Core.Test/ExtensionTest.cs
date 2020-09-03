using System;
using Xunit;

namespace Bermuda.Core.Test
{
    public class ExtensionTest
    {
        [Fact]
        public void PasswordToSHA256()
        {
            string password = Extension.PasswordToSHA256("123456", "tdayi@hotmail.com");
        }
    }
}
