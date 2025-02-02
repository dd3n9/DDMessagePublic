using FluentAssertions;
using MessageService.Domain.MessageAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Tests.Domain.ValueObjects
{
    public class ContentTests
    {
        [Fact]
        public void Equality_SameValue_AreEqual()
        {
            //Arrange
            var value = "Test content";
            var content1 = new Content(value);
            var content2 = new Content(value);

            //Act & Assert
            content1.Should().Be(content2);
        }

        [Fact]
        public void Inequality_DifferentValue_AreNotEqual()
        {
            var value1 = "Test content 1";
            var value2 = "Test content 2";
            var content1 = new Content(value1);
            var content2 = new Content(value2);

            content1.Should().NotBe(content2);
        }
    }
}
