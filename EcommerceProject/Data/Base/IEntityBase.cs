namespace EcommerceProject.Data.Base
{
    //Obtener un ID de la base del repositorio para las demás entidades
    public interface IEntityBase
    {
        int Id { get; set; }
    }
}
