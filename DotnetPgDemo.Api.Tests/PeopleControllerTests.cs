using System;
using System.Threading.Tasks;
using DotnetPgDemo.Api.Controllers;
using DotnetPgDemo.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DotnetPgDemo.Api.Tests
{
    public class PeopleControllerTests
    {
        private AppDbContext CreateTestContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreatePerson_WithValidData_ReturnsOkResult()
        {
            // Arrange
            using var context = CreateTestContext();
            var controller = new PeopleController(context);
            var person = new Person 
            { 
                FirstName = "John", 
                LastName = "Doe" 
            };

            // Act
            var result = await controller.CreatePerson(person);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            
            var returnedPerson = Assert.IsType<Person>(okResult.Value);
            Assert.Equal("John", returnedPerson.FirstName);
            Assert.Equal("Doe", returnedPerson.LastName);
        }

        [Fact]
        public async Task CreatePerson_WithValidData_SavesToDatabase()
        {
            // Arrange
            using var context = CreateTestContext();
            var controller = new PeopleController(context);
            var person = new Person 
            { 
                FirstName = "Jane", 
                LastName = "Smith" 
            };

            // Act
            await controller.CreatePerson(person);

            // Assert
            var savedPerson = await context.People.FirstOrDefaultAsync(p => p.FirstName == "Jane");
            Assert.NotNull(savedPerson);
            Assert.Equal("Jane", savedPerson.FirstName);
            Assert.Equal("Smith", savedPerson.LastName);
        }

        [Fact]
        public async Task CreatePerson_WithEmptyFirstName_ReturnsBadRequest()
        {
            // Arrange
            using var context = CreateTestContext();
            var controller = new PeopleController(context);
            controller.ModelState.AddModelError("FirstName", "Required");
            var person = new Person 
            { 
                FirstName = "", 
                LastName = "Doe" 
            };

            // Act
            var result = await controller.CreatePerson(person);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public async Task CreatePerson_WithEmptyLastName_ReturnsBadRequest()
        {
            // Arrange
            using var context = CreateTestContext();
            var controller = new PeopleController(context);
            controller.ModelState.AddModelError("LastName", "Required");
            var person = new Person 
            { 
                FirstName = "John", 
                LastName = "" 
            };

            // Act
            var result = await controller.CreatePerson(person);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public async Task CreatePerson_WithNameExceedingMaxLength_ReturnsBadRequest()
        {
            // Arrange
            using var context = CreateTestContext();
            var controller = new PeopleController(context);
            controller.ModelState.AddModelError("FirstName", "MaxLength exceeded");
            var person = new Person 
            { 
                FirstName = new string('a', 31), // Exceeds MaxLength of 30
                LastName = "Doe" 
            };

            // Act
            var result = await controller.CreatePerson(person);

            // Assert
            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badResult.StatusCode);
        }

        [Fact]
        public async Task CreatePerson_WithMultiplePeople_SavesAll()
        {
            // Arrange
            using var context = CreateTestContext();
            var controller = new PeopleController(context);
            var person1 = new Person { FirstName = "John", LastName = "Doe" };
            var person2 = new Person { FirstName = "Jane", LastName = "Smith" };

            // Act
            await controller.CreatePerson(person1);
            await controller.CreatePerson(person2);

            // Assert
            var allPeople = await context.People.ToListAsync();
            Assert.Equal(2, allPeople.Count);
        }
    }
}
