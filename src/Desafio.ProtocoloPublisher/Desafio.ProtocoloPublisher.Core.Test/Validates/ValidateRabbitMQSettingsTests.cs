using Desafio.ProtocoloPublisher.Core.Models;
using Desafio.ProtocoloPublisher.Core.Validates;

namespace Desafio.ProtocoloPublisher.Core.Test.Validates
{
    public class ValidateRabbitMQSettingsTests
    {
        [Fact(DisplayName = "When settings is null")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenSettingsIsNull()
        {
            // Arrange
            RabbitMQSettings settings = null;

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "When hostName is missing")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenHostNameIsMissing()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "",
                Port = 5672,
                UserName = "user",
                Password = "password",
                QueueName = "queue"
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "When port is zero")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenPortIsZero()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "localhost",
                Port = 0,
                UserName = "user",
                Password = "password",
                QueueName = "queue"
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "When user name is missing")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenUserNameIsMissing()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "",
                Password = "password",
                QueueName = "queue"
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "When password is missing")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenPasswordIsMissing()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "user",
                Password = "",
                QueueName = "queue"
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "When queueName is missing")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenQueueNameIsMissing()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "user",
                Password = "password",
                QueueName = ""
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "When multiple settings are missing")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenMultipleSettingsAreMissing()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "",
                Port = 0,
                UserName = "",
                Password = "",
                QueueName = ""
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Validate when all settings are valid")]
        [Trait("ValidateRabbitMQSettingsTests", "ValidateRabbitMQSettings Unit Tests")]
        public void ValidateWhenAllSettingsAreValid()
        {
            // Arrange
            var settings = new RabbitMQSettings
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "user",
                Password = "password",
                QueueName = "queue"
            };

            // Act
            var result = ValidateRabbitMQSettings.Validate(settings);

            // Assert
            Assert.True(result);
        }
    }
}
