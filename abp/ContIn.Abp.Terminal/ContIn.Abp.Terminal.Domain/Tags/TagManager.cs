using ContIn.Abp.Terminal.Domain.Shared;
using ContIn.Abp.Terminal.Domain.Shared.Enum;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Timing;

namespace ContIn.Abp.Terminal.Domain.Tags
{
    public class TagManager : DomainService
    {
        private readonly ITagRepository _repository;
        private readonly IClock _clock;
        public TagManager(ITagRepository repository, IClock clock)
        { 
            _repository = repository;
            _clock = clock;
        }

        public async Task<Tag> CreateAsync(string name, string? description, StatusEnum status)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var existingTag = await _repository.FindByTagNameAsync(name);
            if (existingTag != null)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "标签名称已存在");
            }
            return new Tag
            {
                Name = name,
                Description = description,
                Status = status,
                CreateTime = _clock.Now.ToMillisecondsTimestamp(),
                UpdateTime = _clock.Now.ToMillisecondsTimestamp()
            };
        }

        public async Task ChangeNameAsync(Tag tag, string newName)
        {
            Check.NotNull(tag, nameof(tag));
            Check.NotNullOrWhiteSpace(newName, nameof(newName));

            var existingTag = await _repository.FindByTagNameAsync(newName);
            if (existingTag != null && existingTag.Id != tag.Id)
            {
                throw new CustomException(TerminalErrorCodes.EntityAlreadyExists, "标签名称已存在");
            }
            tag.Name = newName;
        }
    }
}
