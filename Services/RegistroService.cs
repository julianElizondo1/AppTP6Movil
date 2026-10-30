
using System.Text.Json;

namespace AppTP6Movil.Services
{
    public class RegistroService
    {
        private readonly SesionService _sesion;
        private readonly SemaphoreSlim _bloqueo = new(1, 1);

        private readonly string _rutaArchivo;

        public RegistroService(SesionService sesion)
        {
            _sesion = sesion;

            _rutaArchivo = Path.Combine(
                FileSystem.AppDataDirectory,
                "registros.jsonl"
            );
        }

        public async Task RegistrarAuditoriaAsync(
            string accion,
            string detalle = "",
            string resultado = "Correcto")
        {
            var registro = new RegistroLog
            {
                Fecha = DateTimeOffset.UtcNow,
                Tipo = "AUDITORIA",
                Usuario = _sesion.Username ?? "No identificado",
                Accion = accion,
                Detalle = detalle,
                Resultado = resultado
            };

            await GuardarRegistroAsync(registro);
        }

        public async Task RegistrarErrorAsync(
            string operacion,
            Exception excepcion)
        {
            var registro = new RegistroLog
            {
                Fecha = DateTimeOffset.UtcNow,
                Tipo = "ERROR",
                Usuario = _sesion.Username ?? "No identificado",
                Accion = operacion,
                Detalle = excepcion.Message,
                Resultado = excepcion.ToString()
            };

            await GuardarRegistroAsync(registro);
        }

        private async Task GuardarRegistroAsync(RegistroLog registro)
        {
            await _bloqueo.WaitAsync();

            try
            {
                var json = JsonSerializer.Serialize(registro);

                await File.AppendAllTextAsync(
                    _rutaArchivo,
                    json + Environment.NewLine
                );
            }
            finally
            {
                _bloqueo.Release();
            }
        }

        // Obtiene todos los registros guardados.
        public async Task<List<RegistroLog>> ObtenerRegistrosAsync()
        {
            await _bloqueo.WaitAsync();

            try
            {
                var registros = new List<RegistroLog>();

                if (!File.Exists(_rutaArchivo))
                    return registros;

                var lineas = await File.ReadAllLinesAsync(_rutaArchivo);

                foreach (var linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea))
                        continue;

                    try
                    {
                        var registro =
                            JsonSerializer.Deserialize<RegistroLog>(linea);

                        if (registro != null)
                            registros.Add(registro);
                    }
                    catch (JsonException)
                    {
                        // Ignora una línea dañada y continúa leyendo.
                    }
                }

                return registros
                    .OrderByDescending(r => r.Fecha)
                    .ToList();
            }
            finally
            {
                _bloqueo.Release();
            }
        }
    }

    // Modelo que representa cada registro de auditoría o error.
    public class RegistroLog
    {
        public DateTimeOffset Fecha { get; set; }
        public string Tipo { get; set; } = "";
        public string Usuario { get; set; } = "";
        public string Accion { get; set; } = "";
        public string Detalle { get; set; } = "";
        public string Resultado { get; set; } = "";
    }
}
