using AutoMapper;
using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces.Repositories;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Application.Mappings;
using CleanArchMvc.Application.Services;
using CleanArchMvc.Application.Tests.TestHelpers;
using CleanArchMvc.Domain.Entities;
using Moq;
using Xunit;

namespace CleanArchMvc.Application.Tests
{
    public class CategoryServiceTest
    {
        public Mock<ICategoryRepository> _repository;
        IMapper _mapper;
        ICategoryService _service;

        public CategoryServiceTest()
        {
            _repository = new Mock<ICategoryRepository>();
            _mapper = MapperHelper.GetMapper(typeof(DomainToDTOMappingProfile), typeof(DTOToCommandMappingProfile));
            _service = new CategoryService(_repository.Object, _mapper);
        }

        [Fact]
        public async void GetCategoryById_CategoryFound()
        {
            _repository.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new Category(1, "Livros"));

            var category = await _service.GetByIdAsync(1);

            Assert.Equal(1, category.Id);
            Assert.NotNull(category);
        }

        [Fact]
        public async void GetCategoryById_CategoryNotFound()
        {
            //Action<Task> action = async () => await _service.GetByIdAsync(1);

            //action.Should().Throw<DomainExceptionValidation>()
            //    .WithMessage("Category not found.");
        }

        [Fact]
        public async void AddCategorySucess()
        {
            _repository.Setup(repo => repo.CreateAsync(It.IsAny<Category>()))
               .ReturnsAsync(new Category(1, "Teste"));

            await _service.AddAsync(new CategoryDTO { Name = "Teste" });

            _repository.Verify(s => s.CreateAsync(It.IsAny<Category>()), Times.Once);
            _repository.VerifyNoOtherCalls();
        }
    }
}