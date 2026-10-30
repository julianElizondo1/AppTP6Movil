namespace AppTP6Movil.Services
{
    public class SesionService
    {
        public string? Token { get; private set; }
        public string? Username { get; private set; }
        public string? Rol { get; private set; }

        public DateTimeOffset? Expiracion { get; private set; }

        // Notifica a los componentes cuando cambia la sesión.
        public event Action? EstadoSesionCambiado;

        public bool EstaAutenticado =>
            ObtenerTokenValido() is not null;

        public void GuardarSesion(
            string token,
            string username,
            string rol,
            DateTimeOffset expiracion)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException(
                    "El token no puede estar vacío.", nameof(token));

            Token = token;
            Username = username;
            Rol = rol;
            Expiracion = expiracion;

            EstadoSesionCambiado?.Invoke();
        }

        public string? ObtenerTokenValido()
        {
            if (string.IsNullOrWhiteSpace(Token) ||
                Expiracion is null)
            {
                return null;
            }

            if (Expiracion.Value <=
                DateTimeOffset.UtcNow.AddSeconds(30))
            {
                CerrarSesion();
                return null;
            }

            return Token;
        }

        public void CerrarSesion()
        {
            Token = null;
            Username = null;
            Rol = null;
            Expiracion = null;

            EstadoSesionCambiado?.Invoke();
        }
    }
}
