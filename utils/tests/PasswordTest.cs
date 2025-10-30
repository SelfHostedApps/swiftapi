using Xunit;

namespace swiftuibackend.Tests
{
    public class PasswordTest
    {
        [Fact]
        public void ValidatePassword()
        {
            string psw = "secret123";
            string cypher = Utils.Password.Hash(psw);

            Assert.True(Utils.Password.Validate(psw, cypher));
            Assert.False(Utils.Password.Validate("secret12", cypher));
        }
    }
}
