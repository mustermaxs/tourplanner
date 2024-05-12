namespace Client.Dao
{
    public interface IDao<TModel>
    {
        Task Create();
        Task<TModel> Read(TModel model);
        Task<IEnumerable<TModel>> ReadMultiple();
        Task Update();
        Task Delete();
        void SetModel(TModel model);
    }

    public abstract class Dao<TModel> : IDao<TModel> where TModel : class
    {
        protected TModel? model;

        public abstract Task Create();
        public abstract Task<TModel> Read(TModel model);
        public abstract Task<IEnumerable<TModel>> ReadMultiple();
        public abstract Task Update();
        public abstract Task Delete();
        public virtual void SetModel(TModel model)
        {
            this.model = model;
        }
    }
}