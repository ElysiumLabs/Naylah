﻿namespace Naylah.Rest.Table
{
    public class StringTableService<TModel> : TableService<TModel, string>
        where TModel : class, IEntity<string>
    {
        public StringTableService(NaylahRestClient client) : base(client)
        {
        }

      
    }






}