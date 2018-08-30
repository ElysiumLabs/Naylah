namespace Naylah.Data.Abstractions
{
    public interface IEntityDataActionCRUDModel<TModel> :
        IEntityDataActionRegisterModel<TModel>,
        IEntityDataActionUpdateModel<TModel>,
        IEntityDataActionDeleteModel<TModel>,
        IEntityDataActionReadModel<TModel>
    {
    }

    public interface IEntityDataActionRegisterModel<TModel>
    {
        bool CreateFrom(TModel objParam);
    }

    public interface IEntityDataActionUpdateModel<TModel>
    {
        bool UpdateFrom(TModel objParam);
    }

    public interface IEntityDataActionReadModel<TModel>
    {
        TModel ReadTo();
    }

    public interface IEntityDataActionDeleteModel<TModel>
    {
        bool DeleteFrom(TModel objParam, bool softDelete);
    }
}