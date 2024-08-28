using AuthUser.Data;
using AuthUser.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace AuthUser.Controllers
{

    public class AuthController : Controller
    {
        private readonly AuthDbContext _context;

        public AuthController(AuthDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthUsers model)
        {

            // Check if the user exists in the AuthUsers table based on the email
            var user = _context.AuthUsers.FirstOrDefault(u => u.Email == model.Email);

            if (user != null)
            {
                // Hash the input password with the stored salt
                string hashedPassword = HashPassword(model.Password, user.Salt);

                // Check if the hashed password matches the stored hash
                if (hashedPassword == user.Password)
                {
                    HttpContext.Session.SetString("UserEmail", model.Email);

                    // Redirecting to dashboard based on user role
                    if (user.Role == "Employee")
                    {
                        return RedirectToAction("Demo", "Auth");
                    }
                   /* else if (user.Role == "Manager")
                    {
                        return RedirectToAction("Index", "Manager");
                    }*/
                }
            }

            TempData["AlertMessage"] = "Invalid email or password";


            ViewBag.Error = "Invalid Email or Password";
            return View(model);
        }


        /* [HttpPost]
         public IActionResult Login(AuthUser model)
         {
             if (ModelState.IsValid)
             {
                 // Check if user email and password exists in AuthUsers table
                 var user = _context.AuthUsers.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                 if (user != null)
                 {
                     // Hash the input password with the user's salt
                     string hashedPassword = HashPassword(user.Password, GenerateSalt());

                     // Check if the hashed password matches the stored hash
                     if (hashedPassword == user.Password)
                     {
                         HttpContext.Session.SetString("UserEmail", model.Email);

                         // Redirect to respective dashboard based on user role
                         if (user.Role == "Employee")
                         {
                             return RedirectToAction("Index", "Employee");
                         }
                         else if (user.Role == "Manager")
                         {
                             return RedirectToAction("Index", "Manager");
                         }
                     }
                 }

                 TempData["AlertMessage"] = "Invalid email or password";
             }

             ViewBag.Error = "Invalid Email or Password";
             return View(model);
         }*/

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(AuthUsers model)
        {
            var existingUser = _context.AuthUsers.FirstOrDefault(u => u.Email == model.Email);

            if (existingUser != null)
            {
                TempData["AlertMessage"] = "User Already Exists !!!";
            }
            else
            {
                // Generating a salt
                model.Salt = GenerateSalt();

                // Hash the password with the salt
                model.Password = HashPassword(model.Password, model.Salt);

                _context.AuthUsers.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(model);

        }
        public IActionResult Demo()
        {
/*            return RedirectToAction("Demo", "Auth");
*/            return View();
        }


        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            // Hash the entered password with the stored salt
            using (var sha256 = SHA256.Create())
            {
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(enteredPassword + storedSalt);
                byte[] enteredHashBytes = sha256.ComputeHash(saltedPasswordBytes);
                string enteredHash = Convert.ToBase64String(enteredHashBytes);

                // Compare the computed hash with the stored hash
                return enteredHash == storedHash;
            }
        }
    }
}

