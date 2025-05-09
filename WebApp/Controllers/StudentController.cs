using Domain.DTOs.Student;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController(IStudentService studentService) : ControllerBase
{
    [HttpPost]
    public async Task<Response<GetStudentDto>> AddAsync(CreateStudentDto createStudentDto)
    {
        return await studentService.AddAsync(createStudentDto);

    }


    [HttpGet("{Id:int}")]
    public async Task<Response<GetStudentDto>> GetByIdAsync(int id)
    {
        return await studentService.GetByIdAsync(id);

    }


    [HttpGet]
    public async Task<Response<List<GetStudentDto>>> GetAllAsync([FromQuery] StudentFilter filter)
    {
        return await studentService.GetAllAsync(filter);

    }


    [HttpPut("{Id:int}")]
    public async Task<Response<GetStudentDto>> UpDateAsync(int id, UpdateStudentDto updateStudentDto)
    {
        return await studentService.UpDateAsync(id, updateStudentDto);


    }


    [HttpDelete("{Id:int}")]
    public async Task<Response<string>> DeleteAsync(int id)
    {
        return await studentService.DeleteAsync(id);


    }
}