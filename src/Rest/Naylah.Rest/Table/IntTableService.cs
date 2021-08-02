namespace Naylah.Rest.Table
{
    public class IntTableService<TModel> : TableService<TModel, int>
        where TModel : class, IEntity<int>
    {
        public IntTableService(NaylahRestClient client) : base(client)
        {
        }
    }






}
