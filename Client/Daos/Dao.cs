using Client.Exceptions;

namespace Client.Dao
{
    public interface IDao<TDto>
    {
        Task Create(TDto dto);
        Task<TDto> Read(int id);
        Task<IEnumerable<TDto>> ReadMultiple();
        Task Update(TDto dto);
        Task Delete(TDto dto);
        void SeTDto(TDto dto);
    }

    public abstract class Dao<TDto> : IDao<TDto> where TDto : class
    {
        protected TDto? model;

        public virtual async Task Create(TDto dto)
        {
            try
            {
                await OnCreate(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DaoException($"Failed to {typeof(TDto).Name}");
            }
        }

        public virtual async Task<TDto> Read(int id)
        {
            try
            {
                return await OnRead(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DaoException($"Failed to read {typeof(TDto).Name}");
            }
        }

        public virtual async Task<IEnumerable<TDto>> ReadMultiple()
        {
            try
            {
                return await OnReadMultiple();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DaoException($"Failed to read {typeof(TDto).Name}s");
            }
        }

        public virtual async Task Update(TDto dto)
        {
            try
            {
                await OnUpdate(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DaoException($"Failed to update {typeof(TDto).Name}");
            }
        }

        public virtual async Task Delete(TDto dto)
        {
            try
            {
                await OnDelete(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new DaoException($"Failed to delete {typeof(TDto).Name}");
            }
        }

        public abstract Task OnCreate(TDto dto);
        public abstract Task<TDto> OnRead(int id);
        public abstract Task<IEnumerable<TDto>> OnReadMultiple();
        public abstract Task OnUpdate(TDto dto);
        public abstract Task OnDelete(TDto dto);

        public virtual void SeTDto(TDto dto)
        {
            this.model = model;
        }
    }
}