using Xunit;
using ZenFlow.UserService.Models;

namespace ZenFlow.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void User_Model_Should_Create_User_With_Correct_Properties()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = "hashedpassword"
            };

            // Act & Assert
            Assert.Equal("test@example.com", user.Email);
            Assert.Equal("hashedpassword", user.PasswordHash);
            Assert.True(user.IsActive);
        }

        [Fact]
        public void UserProfile_Model_Should_Create_UserProfile_With_Correct_Properties()
        {
            // Arrange
            var userProfile = new UserProfile
            {
                UserId = 1,
                FirstName = "John",
                LastName = "Doe",
                Age = 30,
                Gender = "Male",
                Height = 180,
                Weight = 75
            };

            // Act & Assert
            Assert.Equal(1, userProfile.UserId);
            Assert.Equal("John", userProfile.FirstName);
            Assert.Equal("Doe", userProfile.LastName);
            Assert.Equal(30, userProfile.Age);
            Assert.Equal("Male", userProfile.Gender);
            Assert.Equal(180, userProfile.Height);
            Assert.Equal(75, userProfile.Weight);
        }
    }
}