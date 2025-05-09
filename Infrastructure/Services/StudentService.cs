using System.Net;
using AutoMapper;
using Domain.DTOs.Student;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Domainn.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class StudentService(DataContext context, IMapper mapper) : IStudentService
{
    public async Task<Response<List<GetStudentDto>>> GetAllAsync(StudentFilter filter)
    {


        var validFilter = new ValidFilter(filter.PageNumber, filter.PageSize);

        var students = context.Students.AsQueryable();

        if (filter.Name != null)
        {
            students = students.Where(s => string.Concat(s.FirstName, " ", s.LastName).ToLower().Contains(filter.Name.ToLower()));
        }

        if (filter.From != null)
        {
            var year = DateTime.UtcNow.Year;
            students = students.Where(s => year - s.BirthDate.Year >= filter.From);
        }

        if (filter.To != null)
        {
            var year = DateTime.UtcNow.Year;
            students = students.Where(s => year - s.BirthDate.Year <= filter.To);
        }

        var maped = mapper.Map<List<GetStudentDto>>(students);

        var totalRecords = maped.Count;

        var data = maped
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        return new PagedResponse<List<GetStudentDto>>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);

    }


    public async Task<Response<GetStudentDto>> GetByIdAsync(int id)
    {

        var student = await context.Students.FindAsync(id);
        if (student == null)
        {
            return new Response<GetStudentDto>(HttpStatusCode.NotFound, "Student not found");
        }
        var data = mapper.Map<GetStudentDto>(student);
        return new Response<GetStudentDto>(data);
    }


    public async Task<Response<GetStudentDto>> AddAsync(CreateStudentDto createStudentDto)
    {
        var student = mapper.Map<Student>(createStudentDto);

        await context.Students.AddAsync(student);

        var result = await context.SaveChangesAsync();

        var data = mapper.Map<GetStudentDto>(student);

        return result == 0
            ? new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not added!")
            : new Response<GetStudentDto>(data);
    }


    public async Task<Response<GetStudentDto>> UpDateAsync(int id, UpdateStudentDto updateStudentDto)
    {
        var student = await context.Students.FindAsync(id);
        if (student == null)
        {
            return new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not found");
        }

        student.FirstName = updateStudentDto.FirstName;
        student.LastName = updateStudentDto.LastName;
        student.BirthDate = updateStudentDto.BirthDate;

        var result = await context.SaveChangesAsync();

        var data = mapper.Map<GetStudentDto>(student);

        return result == 0
             ? new Response<GetStudentDto>(HttpStatusCode.BadRequest, "Student not updated")
             : new Response<GetStudentDto>(data);

    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        var student = await context.Students.FindAsync(id);
        if (student == null)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "Student not found");
        }

        context.Remove(student);

        var result = await context.SaveChangesAsync();

        return result == 0
          ? new Response<string>(HttpStatusCode.BadRequest, "Student not deleted")
          : new Response<string>(HttpStatusCode.OK, "Student deleted");
    }

}
