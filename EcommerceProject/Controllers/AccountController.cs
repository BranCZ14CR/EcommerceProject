using EcommerceProject.Data;
using EcommerceProject.Data.Static;
using EcommerceProject.Data.ViewModel;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EcommerceProject.Controllers
{
    public class AccountController : Controller
    {
        //Variables
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EcommerceDbContext _context;

        //Constructor
        public AccountController(UserManager<ApplicationUser> userManager, EcommerceDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //Obtener todos los usuarios
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        //Ir a la página de Inicio de Sesión
        public IActionResult Login() => View(new LoginVM());

        //Método para ingresar a la sesión
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //Comprobar si no esta validado
            if (!ModelState.IsValid) return View(loginVM);

            //Encontrar un usuario con el correo que ingresa
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            //Comprobar que el usuario no este nulo
            if (user != null)
            {
                //Comprobar la contraseña este correcta
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);

                //Si es correcta
                if (passwordCheck)
                {
                    //Obtener el resultado y regresarlo al Index
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }

                //Enviar un mensaje de error
                TempData["Error"] = "Credenciales incorrectar, intente otra vez";
                return View(loginVM);
            }

            //Enviar un mensaje de error
            TempData["Error"] = "Credenciales incorrectar, intente otra vez";
            return View(loginVM);
        }


        //Ir a la página de Registro
        public IActionResult Register() => View(new RegisterVM());


        //Método para ingresar una nueva cuenta
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            //Si hay algún campo sin validar
            if (!ModelState.IsValid) return View(registerVM);

            //Encontrar un correo existente
            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            
            //Si no es nulo el correo ya existe en la BD
            if (user != null)
            {
                TempData["Error"] = "El Correo ya existe";
                return View(registerVM);
            }

            //Agregar a un nuevo usuario
            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress
            };

            //Crear el usuario en la BD
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            //Si es correcto se le asinga el rol usuario por defecto
            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }

            return View("RegisterCompleted");
        }


        //Método para salirse de la sesión
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }

        //Método del Acceso Denegado
        public IActionResult AccessDenied(string ReturnUrl)
        {
            return View();
        }
    }
}
