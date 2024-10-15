using ACDS.RevBill.Service.Contracts;
using ACDS.RevBill.Shared.RequestFeatures;
using Audit.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Entities.Models.DTOs;


namespace ACDS.RevBill.Presentation.Controllers
{

    //[Authorize]
    //[AuditApi(EventTypeName = "{controller}/{action} ({verb})")]
    [Route("api/department")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Produces("application/json")]
    public class DepartmentController: ControllerBase
    {
        private readonly IServiceManager _service;
        public DepartmentController(IServiceManager service) => _service = service;

        private readonly DataContext dbContext;

        public DepartmentController(DataContext dbContext)
        {
         
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllDepartments()
        {
            var departmentsDomain = dbContext.Departments.ToList();

            //Map Domain models to DTOs

            var departmentDto = new List<DepartmentDto>();
            foreach (var departmentDomain in departmentsDomain)
            {
                departmentDto.Add(new DepartmentDto()
                {
                    DepartmentId = departmentDomain.DepartmentId,
                    DepartmentCode = departmentDomain.DepartmentCode,
                    DepartmentName = departmentDomain.DepartmentName,

                });
            }
            return Ok(departmentDto);
             
            
        }

        //Get single department
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetSingleDepartment([FromRoute] int id)
        {
            //var department = dbContext.Departments.FirstorDefault(x => x.Id == id);
            var departmentDomain = dbContext.Departments.Find(id);           
       
            if (departmentDomain== null)
            {
                return NotFound();
            }


            //Map department domain model to department dto
            var departmentDtos = new DepartmentDto
            {
                DepartmentId = departmentDomain.DepartmentId,
                DepartmentCode = departmentDomain.DepartmentCode,
                DepartmentName = departmentDomain.DepartmentName,
                DepartmentStatus = departmentDomain.DepartmentStatus,
            };
            //return departmentDto back to client
            return Ok(departmentDtos);


        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody]AddRequestDto addRequestDto)
        {
            //Map or convert Domain model to DTO
            var departmentDomain = new Department //This department(entity) is coming from Domain model folder
            {
                DepartmentName = addRequestDto.DepartmentName,
                DepartmentCode = addRequestDto.DepartmentCode,
                DepartmentStatus = addRequestDto.DepartmentStatus,
            };

            dbContext.Departments.Add(departmentDomain);
            dbContext.SaveChanges();


            //Map domain model back to DTO
            var departmentDTO = new DepartmentDto
            {
                DepartmentId = departmentDomain.DepartmentId,
                DepartmentCode = departmentDomain.DepartmentCode,
                DepartmentName = departmentDomain.DepartmentName,
                DepartmentStatus = departmentDomain.DepartmentStatus,
            };
            return CreatedAtAction(nameof(GetSingleDepartment), new { id = departmentDTO.DepartmentId }, departmentDTO);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateDepartment([FromRoute]int id, [FromBody] UpdateDepartmentDto updateDepartment)
        {
            var departmentDomain = dbContext.Departments.Find(id);
            if(departmentDomain == null)
            {
                return NotFound();
            }

            departmentDomain.DepartmentCode = updateDepartment.DepartmentCode;
            departmentDomain.DepartmentName = updateDepartment.DepartmentName;
            departmentDomain.DepartmentStatus = updateDepartment.DepartmentStatus;

            dbContext.SaveChanges();

            var updateDto = new Department
            {
                DepartmentId = departmentDomain.DepartmentId,
                DepartmentCode = departmentDomain.DepartmentCode,
                DepartmentName = departmentDomain.DepartmentName,
                DepartmentStatus = departmentDomain.DepartmentStatus,
            };

            return Ok(updateDto);
        
        }




































    }
}
