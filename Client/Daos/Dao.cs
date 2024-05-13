namespace Client.Dao
{
    
    public interface IDao<TDto>
    {
        Task Create(TDto dto);
        Task<TDto> Read(TDto tourLog);
        Task<IEnumerable<TDto>> ReadMultiple();
        Task Update(TDto dto);
        Task Delete(TDto dto);
        void SeTDto(TDto dto);
    }

    public abstract class Dao<TDto> : IDao<TDto> where TDto : class
    {
        protected TDto? model;

        public abstract Task Create(TDto dto);
        public abstract Task<TDto> Read(TDto tourLog);
        public abstract Task<IEnumerable<TDto>> ReadMultiple();
        public abstract Task Update(TDto dto);
        public abstract Task Delete(TDto dto);
        public virtual void SeTDto(TDto dto)
        {
            this.model = model;
        }
    }
}