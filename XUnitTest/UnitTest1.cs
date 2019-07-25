
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebShop.Controllers;
using WebShop.Data;
using WebShop.Models;
using Xunit;


namespace XUnitTest
{
    public class UnitTest1
    {

        private DbContextOptions<DbShop> options;
        private DbShop context;

        public UnitTest1()
        {
            options = new DbContextOptionsBuilder<DbShop>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            context = new DbShop(options);
        }


        [Fact]
        public void Test1()
        {

        }



        [Fact(DisplayName ="TestAddNewProduct")]
        public void Test_Add_Product()
        {

                Product product = new Product() { Name="Test", Available=true, Price=11, MeasuringUnit=null, ProductId = Guid.NewGuid()};
                context.Product.Add(product);
                context.SaveChanges();
                int result =  context.Product.CountAsync().Result;       
                Assert.IsType<Product>(product);
                Assert.Equal(1, result);
        }
                


        [Fact]
        public void Test_Product_Controller_Index()
        {
            ProductController productController = new ProductController(context);
            var response =  productController.Index().Result;

            var result = response as ViewResult;

            Assert.IsType<List<Product>>(result.Model);
            Assert.IsType<ViewResult>(response);
        }



        [Fact]
        public void Test_Product_Controller_Details()
        {

            Product product = new Product() { Name = "Test", Available = true, Price = 11, MeasuringUnit = null, ProductId = Guid.NewGuid() };
            context.Product.Add(product);
            context.SaveChanges();

            Product pp = new Product { Name = "Test", Available = true, Price = 11, MeasuringUnit = null, ProductId = Guid.NewGuid() };
            
            var controller = new ProductController(context);
            var result = controller.Details(product.ProductId).Result as ViewResult;
            var p = (Product) result.ViewData.Model;

            Assert.IsType<Product>(p);
            Assert.Equal(product.ProductId, p.ProductId);
            Assert.NotEqual(pp.ProductId, p.ProductId);

        }
         
        
        [Fact]
        public void Test_Product_Controller_Create()
        {
            MeasuringUnit measuringUnit = new MeasuringUnit() { MeasuringUnitId = Guid.NewGuid(), Name = "Pero", Unit = "P", CreatedDate = DateTime.Today, ModifiedDate = DateTime.Today };

            Product product = new Product() { Name = "Test", Available = true, Price = 11, MeasuringUnit = measuringUnit, CreatedDate = measuringUnit.CreatedDate, ModifiedDate = measuringUnit.ModifiedDate, Image= null, Quantity=5,  };
           // context.Product.Add(product);
           // context.SaveChanges();

            ProductController controller = new ProductController(context);
            var res = controller.Create();

            var model = res as ViewResult;

            Assert.IsType<ViewResult>(res);
            Assert.IsType<Product>(model.Model);

        }
            





    }
}
           










