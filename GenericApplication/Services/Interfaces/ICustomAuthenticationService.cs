using MiniMarket_API.Application.DTOs.Requests.Credentials;

namespace WebApplication1.Services.Interfaces
{
    public interface ICustomAuthenticationService
    {
        // Método para autenticar al usuario con la información de inicio de sesión
        // Recibe un objeto LoginRequestDTO con los datos de inicio de sesión
        // Devuelve una tarea que resulta en una cadena (el token de autenticación, por ejemplo) o null si la autenticación falla
        Task<string?> Authenticate(LoginRequestDTO loginRequest);

        // Método para manejar solicitudes de recuperación de contraseña
        // Recibe el correo electrónico del usuario que solicita la recuperación
        // Devuelve una tarea que representa la operación asincrónica
        Task HandlePasswordRecoveryRequest(string email);

        // Método para hashear contraseñas
        // Recibe una cadena de contraseña y devuelve un array de bytes representando la contraseña hasheada
        byte[] PasswordHasher(string password);
    }
}
