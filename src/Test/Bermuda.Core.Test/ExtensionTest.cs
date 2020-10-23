using System;
using Xunit;

namespace Bermuda.Core.Test
{
    public class ExtensionTest
    {
        [Fact]
        public void PasswordToSHA256()
        {
            string password = Extension.PasswordToSHA256("SH???_XZ2020#", "ff46fc7c-a34c-4d7e-bbf6-01d4df532516");
        }
    }
}
