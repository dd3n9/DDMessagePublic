using FluentAssertions;
using MessageService.Domain.MessageAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageService.Tests.Domain.ValueObjects
{
    public class DeliveryDateTests
    {
        [Fact]
        public void Constructor_ValidDate_CreatesDaliveryDate()
        {
            //Arrange
            var dateTime = DateTime.UtcNow.AddDays(1);

            //Act
            var deliveryDate = new DeliveryDate(dateTime);

            //Assert
            deliveryDate.Value.Should().Be(dateTime);
        }

        [Fact]
        public void Equality_SameDate_AreEqual()
        {
            // Arrange
            var dateTime = DateTime.UtcNow.AddDays(1);
            var deliveryDate1 = new DeliveryDate(dateTime);
            var deliveryDate2 = new DeliveryDate(dateTime);

            // Act & Assert
            deliveryDate1.Should().Be(deliveryDate2);
        }

        [Fact]
        public void Inequality_DifferentDate_AreNotEqual()
        {
            // Arrange
            var dateTime1 = DateTime.UtcNow.AddDays(1);
            var dateTime2 = DateTime.UtcNow.AddDays(2);
            var deliveryDate1 = new DeliveryDate(dateTime1);
            var deliveryDate2 = new DeliveryDate(dateTime2);

            // Act & Assert
            deliveryDate1.Should().NotBe(deliveryDate2);
        }
    }
}
