using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAuth.Data;
using MovieAuth.Models;
using MvcMovie.Controllers;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;

namespace MovieAuth.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationContext authContext;
        public AuthenticationController(AuthenticationContext authenticationContext)
        {
            authContext = authenticationContext;
        }
        public IActionResult RegisterAuth(User newUser)
        {
            var isExistingEmailId = authContext.Users.Any(user => user.UserEmail == newUser.UserEmail);
            
            if (!isExistingEmailId)
            {
                var hasher = new PasswordHasher<User>();
                newUser.Password = hasher.HashPassword(newUser, newUser.Password);
                newUser.UserId = Guid.NewGuid();
                authContext.Users.Add(newUser);
                authContext.SaveChanges();
            }

            return View("Start");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MultipleModels multipleModels)
        {
            var existingUser = multipleModels.UserModel;
            var loggedUser = authContext.Users.FirstOrDefault(user => user.UserEmail == existingUser.UserEmail);

            if (loggedUser != null)
            {
                var hasher = new PasswordHasher<User>();
                var passwordState = hasher.VerifyHashedPassword(loggedUser, loggedUser.Password, existingUser.Password);

                if (passwordState == PasswordVerificationResult.Success)
                {
                    //Do Something!
                    var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, loggedUser.UserEmail),
                        new Claim(ClaimTypes.Name, loggedUser.UserEmail),
                        new Claim("UserId", loggedUser.UserId.ToString())
                    };

                    var userPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity> { new ClaimsIdentity(userClaims, "User Identity") });
                    await HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", "Movies");
                }
            }

            return View("Start");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return View("Start");
        }

        public IActionResult Start()
        {
            var movieController = new MoviesController(authContext);
            var users = authContext.Users.ToList();
            var globallyVisibleMovies = (movieController.GetGlobalMovies()).ConvertAll<MovieUserInfo>(x => new MovieUserInfo() 
            {
                Email = users.First(user => user.UserId.Equals(x.UserId)).UserEmail,
                Title = x.Title,
                ReleaseDate = x.ReleaseDate,
                Genre = x.Genre,
                Price = x.Price
            });
            var multipleModels = new MultipleModels()
            {
                MovieModels = globallyVisibleMovies
            };
            return View(multipleModels);
        }
    }
}
