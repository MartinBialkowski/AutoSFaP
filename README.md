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
    .ConvertUsing(new FilterFieldsConverter<StudentFilterDto, Student>());

CreateMap<PagingDto, Paging>(MemberList.Source);

CreateMap<string, SortField<Student>[]>()
    .ConvertUsing(new SortFieldsConverter<Student>());
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

If you have any question don't hasitate to ask. You can find me here: https://progressdesire.com/welcome/ or on Github.