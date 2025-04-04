using System.Linq.Expressions;
using AutoMapper;
using NotificationService.Core.RepositoryContracts;

namespace NotificationService.Core.Services.CommonServiceContract
{
    public class Service<T, TDto> : IService<T, TDto> where T : class
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;

        public Service(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Lấy tất cả bản ghi dưới dạng DTO
        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        // Lấy bản ghi theo ID dưới dạng DTO
        public async Task<TDto?> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<TDto>(entity) : default(TDto);
        }

        // Tìm kiếm bản ghi theo predicate và trả về dưới dạng DTO
        public async Task<IEnumerable<TDto>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _repository.FindAsync(predicate);
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        // Thêm hoặc cập nhật bản ghi và trả về DTO
        public async Task<TDto> UpsertAsync(TDto dto, Expression<Func<T, bool>> predicate)
        {
            // Chuyển DTO sang Entity
            var entity = _mapper.Map<T>(dto);
            var updatedEntity = await _repository.UpsertAsync(entity, predicate);

            // Chuyển Entity về DTO và trả về
            return _mapper.Map<TDto>(updatedEntity);
        }

        // Xóa bản ghi
        public async Task DeleteAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                await _repository.Delete(entity);
            }
        }
    }
}
