| Stable | Preview |
|:--:|:--:|
|![Nuget](https://img.shields.io/nuget/v/AutoSFaP) | ![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/AutoSFaP) |

| Master | Develop |
|:--:|:--:|
|[![Build Status](https://embeprojects.visualstudio.com/PublicShowProject/_apis/build/status/AutoSFaP%20CI?branchName=master)](https://embeprojects.visualstudio.com/PublicShowProject/_build/latest?definitionId=8&branchName=master) | [![Build Status](https://embeprojects.visualstudio.com/PublicShowProject/_apis/build/status/AutoSFaP%20CI?branchName=develop)](https://embeprojects.visualstudio.com/PublicShowProject/_build/latest?definitionId=8&branchName=develop) |
|![Azure DevOps tests](https://img.shields.io/azure-devops/tests/embeprojects/PublicShowProject/8/master) | ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/embeprojects/PublicShowProject/8/develop) |

[![License](https://img.shields.io/:license-mit-blue.svg)](https://embeprojects.visualstudio.com/PublicShowProject/_git/AutoSFaP?path=%2FLICENSE)

# Introduction 
AutoSFaP is easy way to create Filetering, Sorting and Paging from WebAPI Controller to Entity Framework. 
You can find working project here: https://github.com/MartinBialkowski/Spike/tree/develop

# Description
AutoSFaP lets you implement server-side sorting, filtering and paging for your WebAPI. It uses IQuerybale to make it possible for EF to use LinQ to SQL. Solution is generic it will take fields to sort and filter from your DTO.

# Query convention
GET: /api/students?pageNumber=1&pageLimit=3&Name=Martin&sort=CourseId,Name-
This will get students, page 1 and 3 students per page. Additionally it will filter to get only students with name Martin. Finally it will sort by Course ascending and Name descending (mind "-" sign at the end).
Package will expect you to use FilterField<T>, SortField<T> and Paging types. It will use reflection to get properties for sorting and filtering.

# How to use
## Setup DI
You need to configure DI Container.

builder.RegisterType<DataLimiter<Student>>().As<IDataLimiter<Student>>();

It is painful, because every type need to be configured separately (mb I'll find better solution for it, you can leave me message if you know how to deal with it.)

## Map models
First of all you have to specify mappers. Package has prepared converters using AutoMapper, all you need to do is create mapping profiles (I'll think about making it less painful).

```
CreateMap<StudentFilterDto, FilterField<Student>[]>()
    .ConvertUsing(x => FilterFieldsConverter<StudentFilterDto, Student>.Convert(x));

CreateMap<PagingDto, Paging>(MemberList.Source);

CreateMap<string, SortField<Student>[]>()
    .ConvertUsing(x => SortFieldsConverter<Student>.Convert(x));
```
### Example models
```
public class StudentFilterDto
{
    public int? Id { get; set; }
    public int? CourseId { get; set; }
    public string Name { get; set; }
}
```
```
public class Student: IEntityBase
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    [Required, StringLength(50)]
    public string Name { get; set; }
    public Course Course { get; set; }
}
```
As you can see, both models are similar. Package will not limit you. You don't have to use DTO, in example I'll use DTOs.

## Usage with paging
```
public async Task<ActionResult<Model>> GetStudents([FromQuery] PagingDto pagingDto, [FromQuery] StudentFilterDto filterDto, [FromQuery] string sort = "Id")
{
    var sortFields = mapper.Map<string, SortField<Student>[]>(sort);
    var filterFields = mapper.Map<StudentFilterDto, FilterField<Student>[]>(filterDto);
	var paging = mapper.Map<PagingDto, Paging>(pagingDto);

	IQueryable<Student> query = Context.Students.Include(s => s.Course);
	var pagedResult = await dataLimiter.LimitDataAsync(query, sortFields, filterFields, paging);

	return Ok(pagedResult);
}
```

First of all you need to map DTOs to models that are needed for AutoSFaP. Then call Entity Framework to load data. LimitDataAsync will load data from database and pack it into PagedResult<T> which look like that:

```
public class PagedResult<T>
{
    public PagedResult();

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalNumberOfPages { get; set; }
    public int TotalNumberOfRecords { get; set; }
    public List<T> Results { get; set; }
}
```
It is common response that client would expect.

## Usage without paging
```
public async Task<ActionResult<Model>> GetStudents([FromQuery] StudentFilterDto filterDto, [FromQuery] string sort = "Id")
{
    var sortFields = mapper.Map<string, SortField<Student>[]>(sort);
    var filterFields = mapper.Map<StudentFilterDto, FilterField<Student>[]>(filterDto);

	IQueryable<Student> query = Context.Students.Include(s => s.Course);
	var results = dataLimiter.LimitData(query, sortFields, filterFields).ToList();

	return Ok(results);
}

Loading data without paging look the same, there is only one difference. LimitData method will return IQueryable, so you can call ToList() by yourself. It's, because I didn't want to force anything on users.
```

## Advanced Query
It is also possible to create more advanced queries and then pass that query to AutoSFaP just to page/filter or sort your query.
It is great for grid views, when you need complicated query to get needed data from database, but you want to give user ability to sort or filter your view. In that case you don't need to think about it, just pass your query down to AutoSFap and it will add filtering/paging/sorting at the end. Just make sure that your model will match the model that is used to create AutoSFaP object.

```
var query = context.MLIndexes
.Join(context.Discounts, products => products.Id, discounts => discounts.ProductId, (products, discounts) => new { products })
.Select(x => x.products)
.Distinct()
.GroupJoin(context.Discounts, products => products.Id, discounts => discounts.ProductId, (products, discounts) => new { products, discounts })
.SelectMany(result => result.discounts.Where(x =>
(x.BeginDate <= DateTime.Now && x.EndDate >= DateTime.Now) ||
(x.BeginDate <= DateTime.Now && x.EndDate == null)).DefaultIfEmpty(),
    (result, discount) => new { result.products, discount })
.Where(x => x.products.Price > 100.00)
.Select(x => new ProductsDiscountDto
{
    Id = x.products.Id,
    Name = x.products.Name,
    Price = x.products.Price,
    DiscountRate = x.discount.DiscountRate
});

return await dataLimiter.LimitDistinctDataAsync(query, request.SortFields, request.FilterFields, request.Paging);
```
If you have any question don't hasitate to ask. You can find me here: https://progressdesire.com/welcome/ or on Github.