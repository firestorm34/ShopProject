//using System;
//using Xunit;
//using ShopProject.Controllers;
//using ShopProject.Models;
//using ShopProject.Data;
//using ShopProject.Areas;
//using Moq;
//using Microsoft.AspNetCore.Identity;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using ShopProject.ViewModels;

//namespace ShopProject.Tests
//{
//    public class AccountControllerTests
//    {
//        User setupUser = new User
//        {
//            Name = "parad",
//            LastName = "parad",
//            Email = "parad"
//        };
//        public Task<User> GetSetupUserAsync()
//        {
//            return Task.Factory.StartNew(() => {
//                return setupUser;
//            });
//        }
//        [Fact]
//        public void Test1()
//        {
//            //var userStore = new Mock<IUserStore<User>>();
//            //var userManager = new Mock<UserManager<User>>(userStore.Object,null,
//            //    null,null,null,null,null,null,null);
//            //var signInManager = new Mock<SignInManager<User>>();
//            var userManager = new Mock<FakeUserManager>();
//            var signInManager = new Mock<FakeSignInManager>(userManager);


//            var controller = new AccountController(userManager.Object, signInManager.Object);
//            var result = controller.Index();

//            var viewresult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsType<EditUserViewModel>(viewresult.Model);
//            Assert.Equal(setupUser.Name, model.Name);


//        }
//    }
//}