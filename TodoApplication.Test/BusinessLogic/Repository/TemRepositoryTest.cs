using BusinessLogic.Repository;
using DataAccess;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication.Test.BusinessLogic.Repository
{
    [TestClass]
    public class TemRepositoryTest
    {
        private List<Item> _items = new List<Item>();

        [TestInitialize]
        public void Initialize()
        {
            mockRepository = new Mock<IItemRepository>();
            _items.Add(new Item()
            {
                Date = DateTime.Now,
                Description = "Description 1",
                Id = 1,
                Name = "Name 1"
            });
            _items.Add(new Item()
            {
                Date = DateTime.Now,
                Description = "Description 2",
                Id = 2,
                Name = "Name 2"
            });
            _items.Add(new Item()
            {
                Date = DateTime.Now,
                Description = "Description 3",
                Id = 3,
                Name = "Name 3"
            });
        }

        [TestMethod]
        public async Task TestGetAllItems()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("dbName")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.AddRange(_items);                
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                ItemRepository itemRepo = new ItemRepository(context);
                List<Item> items = await itemRepo.ToListAsync();

                Assert.AreEqual(3, items.Count);
            }

        }

        [TestMethod]
        public async Task TestGetItem()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("dbName")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.AddRange(_items);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                ItemRepository itemRepo = new ItemRepository(context);
                var item = await itemRepo.GetAsync(1);

                Assert.IsNotNull(item);
            }

        }

        [TestMethod]
        public async Task TestUpdateItem()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("dbName")
            .Options;

            var item = new Item()
            {
                Date = DateTime.Now,
                Description = "Description",
                Id = 1,
                Name = "Name"
            };

            using (var context = new ApplicationDbContext(options))
            {
                context.Items.Add(item);
                context.SaveChanges();
            }

            item.Name = "New Name";

            using (var context = new ApplicationDbContext(options))
            {
                ItemRepository itemRepo = new ItemRepository(context);
                await itemRepo.UpdateAsync(item);

                Assert.AreEqual(item.Name, "New Name");
            }

        }

        [TestMethod]
        public void TestItemExist()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("dbName")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.AddRange(_items);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                ItemRepository itemRepo = new ItemRepository(context);
                var exist = itemRepo.ItemExists(1);

                Assert.IsTrue(exist);
            }

        }

        [TestMethod]
        public async Task TestAddItem()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("dbName")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.AddRange(_items);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                ItemRepository itemRepo = new ItemRepository(context);
                var newItem = new Item()
                {
                    Date = DateTime.Now,
                    Description = "Description 4",
                    Id = 4,
                    Name = "Name 4"
                };
                await itemRepo.AddAsync(newItem);

                var items = await itemRepo.ToListAsync();
                var itemAdded = await itemRepo.GetAsync(4);

                Assert.AreEqual(4, items.Count);
                Assert.IsNotNull(itemAdded);
                Assert.AreEqual(itemAdded.Name, newItem.Name);
                Assert.AreEqual(itemAdded.Description, newItem.Description);
                Assert.AreEqual(itemAdded.Date, newItem.Date);
            }

        }

        [TestMethod]
        public async Task TestDeleteItem()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("dbName")
            .Options;


            using (var context = new ApplicationDbContext(options))
            {
                context.AddRange(_items);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                ItemRepository itemRepo = new ItemRepository(context);
                await itemRepo.DeleteAsync(1);
                var items = await itemRepo.ToListAsync();

                Assert.AreEqual(2, items.Count);
            }

        }
    }
}
