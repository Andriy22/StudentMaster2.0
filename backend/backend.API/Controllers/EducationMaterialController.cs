﻿using backend.BLL.Common.DTOs.EducationMaterials;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Teacher,Admin")]
    [ApiController]
    public class EducationMaterialController : ControllerBase
    {
        private IEducationMaterialService educationMaterialService;

        public EducationMaterialController(IEducationMaterialService educationMaterialService)
        {
            this.educationMaterialService = educationMaterialService;
        }

        [HttpGet("get-education-materials/{subjectId}/{groupId}")]
        public async Task<ActionResult<CrudEducationMaterialDto>> GetMaterialsAsync(int subjectId, int? groupId)
        {
            return Ok(await educationMaterialService.GetEducationMaterialsAsync(subjectId, groupId));
        }

        [HttpPost("create-education-material")]
        public async Task<ActionResult> CreateEducationMaterialAsync(CrudEducationMaterialDto entity)
        {
            entity.UserId = User.Identity.Name;

            await educationMaterialService.CreateEducationMaterialAsync(entity);

            return Ok();
        }

        [HttpPut("edit-education-material")]
        public async Task<ActionResult> EditEducationMaterialAsync(CrudEducationMaterialDto entity)
        {
            entity.UserId = User.Identity.Name;

            await educationMaterialService.EditEducationMaterialAsync(entity);

            return Ok();
        }

        [HttpDelete("delete-education-material")]
        public async Task<ActionResult> DeleteEducationMaterialAsync(int Id)
        {
            await educationMaterialService.DeleteEducationMaterialAsync(Id);

            return Ok();
        }
    }
}
