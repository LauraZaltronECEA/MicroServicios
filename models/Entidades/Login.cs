
namespace models.Entidades

{
    public class Login //mismas propiedades de la tabla
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string FechaAlta { get; set; }
        public string FechaLogin { get; set; }
        public string Estado { get; set; }
    }
}
